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

        // 添加菜单项，可以从菜单中直接访问地图生成工具
        [MenuItem("Tools/三合一/地图生成工具")]
        public static void ShowMapGenerator()
        {
            ToolManager.SetActiveTool<MergeMapGenerator>();
        }

        // 添加菜单项，可以直接导出当前地图数据
        [MenuItem("Tools/三合一/导出地图数据")]
        public static void ExportMapDataFromMenu()
        {
            var mapGenerator = CreateInstance<MergeMapGenerator>();
            mapGenerator.InitializeMapRoot();
            mapGenerator.ExportMapData();
        }

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
        
        private bool GenerateMap()
        {
            if (_mapRoot == null) return false;
            if (_cellRoot == null) return false;
            if (_tilemap == null) return false;
            if (_mergeCellPrefab == null) return false;
            if (_regionObj != null)
            {
                DestroyImmediate(_regionObj);
            }
            _cellA = AssetDatabase.LoadAssetAtPath<Sprite>(CellAPath);
            _cellB = AssetDatabase.LoadAssetAtPath<Sprite>(CellBPath);
            _regionObj = new GameObject("Region");
            _regionObj.transform.SetParent(_cellRoot);
            _regionObj.transform.Reset();
            // 合成地块管理
            _regionObj.AddComponent<MergeableRegion>();

            // 获取地图grid信息
            BoundsInt bounds = _tilemap.cellBounds;
            bool isCellA = true;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = _tilemap.GetTile(cellPosition);

                    if (tile != null)
                    {
                        Vector3 worldPos = _tilemap.GetCellCenterWorld(cellPosition);

                        // 在对应位置创建cell
                        CreateCell(worldPos, x, y,isCellA);
                        isCellA = !isCellA;
                    }
                }
            }

            return true;
        }

        void CreateCell(Vector3 position, int x, int y, bool isCellA)
        {
            var cellObj = Instantiate(_mergeCellPrefab, position,
                Quaternion.identity, _regionObj.transform);
            
            cellObj.name = $"Cell_[{x},{y}]";
            cellObj.GetComponent<SpriteRenderer>().sprite = isCellA ? _cellA : _cellB;
            var cell = cellObj.AddComponent<MergeableCell>();
            cell.CellStatus = MergeableCell.ECellStatus.Locked;
            cell.MapCoordinate = new Vector2Int(x, y);
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