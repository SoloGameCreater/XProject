using CommonExtensions;
using TripleMerge;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;

namespace TripleMerge.Editor
{
    [EditorTool("Map Generator")]
    public class MergeMapGenerator : EditorTool
    {
        private GameObject _mapRoot;
        private Transform _cellRoot;
        private Tilemap _tilemap;

        private GameObject _regionObj;

        private const string EditorRootPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/Area/AreaRootEditor.prefab";
        private const string CellPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/MergeCell/MergeableCell.prefab";
        private GameObject _mergeCellPrefab;

        private Sprite _cellA;
        private Sprite _cellB;
        
        private const string CellAPath = "Assets/ExtraRes/TMatchRessss/World_Grass/grass_03_01.png";
        private const string CellBPath = "Assets/ExtraRes/TMatchRessss/World_Grass/grass_04_01.png";
        
        private string _exportPath = "Assets/Resources/TripleMapData";
        private string _fileName = "MapData";

        private void InitializeMapRoot()
        {
            _mapRoot = GameObject.Find("AreaRootEditor");
            if (_mapRoot == null)
            {
                _mapRoot = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EditorRootPrefabPath));
                _mapRoot.name = "AreaRootEditor";
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            InitializeMapRoot();

            _cellRoot = _mapRoot?.transform.Find("LogicNode/MergeableRegion");
            _tilemap = _mapRoot?.transform.Find("Terrain/Grass").GetComponent<Tilemap>();
            _mergeCellPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CellPrefabPath);
            Handles.BeginGUI();

            using (new GUILayout.VerticalScope("三合地图工具", "window", GUILayout.Height(300), GUILayout.Width(250)))
            {
                GUILayout.Label("地图生成", EditorStyles.boldLabel);
                
                if (GUILayout.Button("生成地图"))
                {
                    if (GenerateMap())
                        Debug.Log("地图生成成功");
                    else
                        Debug.LogWarning("地图生成失败");
                }
                
                GUILayout.Space(10);
                
                GUILayout.Label("导出设置", EditorStyles.boldLabel);
                
                EditorGUILayout.BeginVertical("box");
                
                // 导出路径（只读）
                EditorGUILayout.LabelField("导出路径:");
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.SelectableLabel(_exportPath, EditorStyles.textField, GUILayout.Height(20));
                EditorGUI.EndDisabledGroup();
                
                // 文件名输入（不含后缀）
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("文件名");
                _fileName = EditorGUILayout.TextField(_fileName, GUILayout.Width(120));
                EditorGUILayout.LabelField(".json", GUILayout.Width(40));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();
                
                GUILayout.Space(5);
                
                if (GUILayout.Button("导出地图数据"))
                {
                    ExportMapData();
                }
                
                // 显示地图信息
                if (_regionObj != null)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("地图信息", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"地块数量: {_regionObj.transform.childCount}");
                }
            }

            Handles.EndGUI();
        }
        
        public override bool IsAvailable()
        {
            return SceneManager.GetActiveScene().name.Equals("MergeScene");
        }
        
        /// <summary>
        /// 生成合并地图
        /// </summary>
        /// <returns>是否成功生成地图</returns>
        private bool GenerateMap()
        {
            // 验证必要组件是否存在
            if (!ValidateMapComponents())
                return false;

            // 准备地图区域
            PrepareRegionObject();
            
            // 加载地图数据
            var mapData = LoadMapData();
            
            // 根据Tilemap信息生成地块
            GenerateCellsFromTilemap(mapData);
            
            return true;
        }

        /// <summary>
        /// 验证地图生成所需的组件是否存在
        /// </summary>
        private bool ValidateMapComponents()
        {
            if (_mapRoot == null)
            {
                Debug.LogError("错误: 地图根节点为空");
                return false;
            }
            
            if (_cellRoot == null)
            {
                Debug.LogError("错误: 地块根节点为空");
                return false;
            }
            
            if (_tilemap == null)
            {
                Debug.LogError("错误: Tilemap为空");
                return false;
            }
            
            if (_mergeCellPrefab == null)
            {
                Debug.LogError("错误: 合并地块预制体为空");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 准备地图区域对象
        /// </summary>
        private void PrepareRegionObject()
        {
            // 移除已存在的区域对象
            if (_regionObj != null)
            {
                DestroyImmediate(_regionObj);
            }
            
            // 加载地块精灵图
            _cellA = AssetDatabase.LoadAssetAtPath<Sprite>(CellAPath);
            _cellB = AssetDatabase.LoadAssetAtPath<Sprite>(CellBPath);
            
            // 创建新的区域对象
            _regionObj = new GameObject("Region");
            _regionObj.transform.SetParent(_cellRoot);
            _regionObj.transform.Reset();
            
            // 添加合成地块管理组件
            _regionObj.AddComponent<MergeableRegion>();
        }

        /// <summary>
        /// 加载地图数据，如果不存在则创建新的
        /// </summary>
        private MapData LoadMapData()
        {
            var mapData = MapDataLoader.GetMapData();
            
            if (mapData == null)
            {
                Debug.LogWarning("没有地图数据，将创建新的地图数据");
                mapData = new MapData
                {
                    cells = new Dictionary<string, CellData>()
                };
            }
            
            return mapData;
        }

        /// <summary>
        /// 从Tilemap生成地块
        /// </summary>
        /// <param name="mapData">地图数据</param>
        private void GenerateCellsFromTilemap(MapData mapData)
        {
            // 获取地图边界信息
            BoundsInt bounds = _tilemap.cellBounds;
            bool isCellA = true;
            
            // 遍历所有地图单元
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    // 检查当前位置是否有瓦片
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = _tilemap.GetTile(cellPosition);
                    
                    // 跳过空白区域
                    if (tile == null)
                        continue;
                    
                    // 获取瓦片的世界坐标
                    Vector3 worldPos = _tilemap.GetCellCenterWorld(cellPosition);
                    string cellKey = $"{x}_{y}";
                    
                    // 根据现有数据或创建新的地块
                    if (mapData.cells.TryGetValue(cellKey, out CellData cellData))
                    {
                        // 使用现有数据创建地块
                        CreateCell(worldPos, x, y, isCellA, cellData);
                    }
                    else
                    {
                        // 创建新地块
                        CreateCell(worldPos, x, y, isCellA);
                    }
                    
                    // 交替地块样式
                    isCellA = !isCellA;
                }
            }
        }
        
        /// <summary>
        /// 创建合并地块
        /// </summary>
        /// <param name="position">世界坐标位置</param>
        /// <param name="x">地图X坐标</param>
        /// <param name="y">地图Y坐标</param>
        /// <param name="isCellA">是否使用A类型精灵图</param>
        /// <param name="cellData">单元格数据(可选)</param>
        private void CreateCell(Vector3 position, int x, int y, bool isCellA, CellData cellData = null)
        {
            // 实例化地块对象
            var cellObj = Instantiate(_mergeCellPrefab, position,
                Quaternion.identity, _regionObj.transform);
            
            // 设置地块名称
            cellObj.name = $"Cell_[{x},{y}]";
            
            // 设置地块精灵图
            cellObj.GetComponent<SpriteRenderer>().sprite = isCellA ? _cellA : _cellB;
            
            // 添加可合并单元格组件
            var cell = cellObj.AddComponent<MergeableCell>();
            
            // 设置基本属性
            cell.CellStatus = MergeableCell.ECellStatus.Locked;
            cell.MapCoordinate = new Vector2Int(x, y);

            // 如果有预设数据，则应用预设数据
            if (cellData == null) 
                return;
            
            // 应用预设数据
            cell.BelongRegionId = cellData.belongRegionId;
            cell.CellStatus = (MergeableCell.ECellStatus)cellData.cellStatus;
            cell.PurifiedPriority = cellData.purifiedPriority;
            cell.RequiredPurifiedNum = cellData.requiredPurifiedNum;
            cell.InitialPlacedItemId = cellData.initialPlacedItemId;
        }
        
        private void ExportMapData()
        {
            if (_mapRoot == null)
            {
                EditorUtility.DisplayDialog("错误", "请先生成地图", "确定");
                return;
            }

            Transform regionTransform = _mapRoot.transform.Find("LogicNode/MergeableRegion/Region");
            if (regionTransform == null || regionTransform.childCount == 0)
            {
                EditorUtility.DisplayDialog("错误", "未找到有效的地块数据", "确定");
                return;
            }

            // 创建导出目录
            if (!Directory.Exists(_exportPath))
            {
                Directory.CreateDirectory(_exportPath);
            }

            // 收集地块数据
            var mapData = new MapData();
            mapData.cells = new Dictionary<string, CellData>();

            foreach (Transform cellTransform in regionTransform)
            {
                var cell = cellTransform.GetComponent<MergeableCell>();
                if (cell != null)
                {
                    // 使用坐标作为键
                    string key = $"{cell.MapCoordinate.x}_{cell.MapCoordinate.y}";
                    
                    // 创建单元格数据
                    var cellData = new CellData
                    {
                        belongRegionId = cell.BelongRegionId,
                        cellStatus = (int)cell.CellStatus,
                        purifiedPriority = cell.PurifiedPriority,
                        requiredPurifiedNum = cell.RequiredPurifiedNum,
                        initialPlacedItemId = cell.InitialPlacedItemId,
                        mapCoordinate = new int[] { cell.MapCoordinate.x, cell.MapCoordinate.y }
                    };
                    
                    // 添加到字典
                    mapData.cells.Add(key, cellData);
                }
            }

            // 序列化为JSON
            string json = JsonUtility.ToJson(mapData, true);
            
            // 保存到文件
            string fullPath = Path.Combine(_exportPath, _fileName + ".json");
            File.WriteAllText(fullPath, json);
            
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("成功", $"地图数据已导出到: {fullPath}", "确定");
            Debug.Log($"地图数据已导出到: {fullPath}");
        }
    }
}