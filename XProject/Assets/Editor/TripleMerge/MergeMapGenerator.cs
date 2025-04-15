using CommonExtensions;
using TripleMerge;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TripleMerge.Editor
{
    [EditorTool("Map Generator")]
    public class MergeMapGenerator : EditorTool
    {
        private GameObject _mapRoot;
        private Transform _cellRoot;
        private Tilemap _tilemap;

        private GameObject _regionObj;
        
        // 选中的区域ID
        private int _selectedRegionId = 0;
        
        // 选中的优先级
        private int _selectedPriorityId = 0;
        
        // 当前区域内的所有优先级
        private int[] _currentPriorities = new int[0];

        // 标记是否是通过工具选择的
        private bool _isToolSelection = false;

        private const string EditorRootPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/Area/AreaRootEditor.prefab";
        private const string CellPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/MergeCell/MergeableCell.prefab";
        private GameObject _mergeCellPrefab;

        private Sprite _cellA;
        private Sprite _cellB;
        
        private const string CellAPath = "Assets/ExtraRes/TMatchRessss/World_Grass/grass_03_01.png";
        private const string CellBPath = "Assets/ExtraRes/TMatchRessss/World_Grass/grass_04_01.png";
        
        private readonly string _exportPath = "Assets/ExtraRes/Configs/TripleMapData";
        private string _fileName = "MapData";

        public override void OnActivated()
        {
            base.OnActivated();
            // 注册Selection变化事件
            Selection.selectionChanged += OnSelectionChanged;
        }

        public override void OnWillBeDeactivated()
        {
            base.OnWillBeDeactivated();
            // 注销Selection变化事件
            Selection.selectionChanged -= OnSelectionChanged;
        }

        /// <summary>
        /// 当场景中的选择发生变化时调用
        /// </summary>
        private void OnSelectionChanged()
        {
            // 如果是通过工具选择的，则不需要重置，并重置标记
            if (_isToolSelection)
            {
                _isToolSelection = false;
                return;
            }
            
            // 如果当前有选中区域或优先级，则重置选择
            if (_selectedRegionId > 0 || _selectedPriorityId > 0)
            {
                ResetSelections();
                // 强制重绘编辑器窗口以更新UI状态
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// 重置区域和优先级选择
        /// </summary>
        private void ResetSelections()
        {
            _selectedRegionId = 0;
            _selectedPriorityId = 0;
            _currentPriorities = new int[0];
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
                    
                    // 区域ID下拉框
                    GUILayout.Space(5);
                    GUILayout.Label("区域选择", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("区域ID");
                    
                    // 存储之前的选择
                    int previousRegionId = _selectedRegionId;
                    
                    // 创建0-9的选项数组
                    string[] options = new string[10];
                    for (int i = 0; i < 10; i++)
                    {
                        options[i] = i.ToString();
                    }
                    
                    // 显示下拉框
                    _selectedRegionId = EditorGUILayout.Popup(_selectedRegionId, options, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();
                    
                    // 如果选中的区域ID大于0，显示优先级下拉框
                    if (_selectedRegionId > 0)
                    {
                        // 如果区域ID发生变化，重新获取该区域内的所有优先级
                        if (previousRegionId != _selectedRegionId)
                        {
                            _currentPriorities = GetPrioritiesInRegion(_selectedRegionId);
                            _selectedPriorityId = 0; // 重置优先级选择
                        }
                        
                        // 显示优先级下拉框
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("优先级");
                        
                        // 存储之前的优先级选择
                        int previousPriorityId = _selectedPriorityId;
                        
                        // 创建优先级选项数组 (0代表全部)
                        string[] priorityOptions = new string[_currentPriorities.Length + 1];
                        priorityOptions[0] = "全部";
                        for (int i = 0; i < _currentPriorities.Length; i++)
                        {
                            priorityOptions[i + 1] = _currentPriorities[i].ToString();
                        }
                        
                        // 显示优先级下拉框
                        int selectedIndex = _selectedPriorityId == 0 ? 0 : System.Array.IndexOf(_currentPriorities, _selectedPriorityId) + 1;
                        selectedIndex = Mathf.Max(0, selectedIndex); // 防止找不到优先级时出现负数
                        
                        int newSelectedIndex = EditorGUILayout.Popup(selectedIndex, priorityOptions, GUILayout.Width(100));
                        
                        // 更新选中的优先级
                        _selectedPriorityId = newSelectedIndex == 0 ? 0 : _currentPriorities[newSelectedIndex - 1];
                        
                        EditorGUILayout.EndHorizontal();
                        
                        // 如果区域ID或优先级发生变化，更新选中状态
                        if (previousRegionId != _selectedRegionId || previousPriorityId != _selectedPriorityId)
                        {
                            HighlightCellsByRegionId(_selectedRegionId, _selectedPriorityId);
                        }
                    }
                    else
                    {
                        // 如果区域ID变化为0，更新选中状态
                        if (previousRegionId != _selectedRegionId)
                        {
                            _selectedPriorityId = 0;
                            HighlightCellsByRegionId(_selectedRegionId, _selectedPriorityId);
                        }
                    }
                    
                    // 检查是否选中了恰好两个MergeableCell对象
                    GUILayout.Space(10);
                    MergeableCell[] selectedCells = GetSelectedMergeableCells();
                    if (selectedCells != null && selectedCells.Length == 2)
                    {
                        GUILayout.Label("地块操作", EditorStyles.boldLabel);
                        
                        // 显示选中的两个地块信息
                        EditorGUILayout.BeginVertical("box");
                        
                        EditorGUILayout.LabelField("地块1:", EditorStyles.boldLabel);
                        DisplayCellInfo(selectedCells[0]);
                        
                        EditorGUILayout.Space(5);
                        
                        EditorGUILayout.LabelField("地块2:", EditorStyles.boldLabel);
                        DisplayCellInfo(selectedCells[1]);
                        
                        EditorGUILayout.EndVertical();
                        
                        // 交换按钮
                        GUILayout.Space(5);
                        if (GUILayout.Button("交换地块属性", GUILayout.Height(30)))
                        {
                            SwapCellProperties(selectedCells[0], selectedCells[1]);
                        }
                    }
                }
            }

            Handles.EndGUI();
        }
        
        /// <summary>
        /// 显示单个地块的信息
        /// </summary>
        private void DisplayCellInfo(MergeableCell cell)
        {
            if (cell == null) return;
            
            EditorGUILayout.LabelField($"坐标: [{cell.MapCoordinate.x}, {cell.MapCoordinate.y}]");
            EditorGUILayout.LabelField($"区域ID: {cell.BelongRegionId}");
            EditorGUILayout.LabelField($"优先级: {cell.PurifiedPriority}");
            EditorGUILayout.LabelField($"所需净化值: {cell.RequiredPurifiedNum}");
        }
        
        /// <summary>
        /// 获取当前选中的MergeableCell对象
        /// </summary>
        /// <returns>选中的MergeableCell数组</returns>
        private MergeableCell[] GetSelectedMergeableCells()
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
                return null;
            
            List<MergeableCell> cells = new List<MergeableCell>();
            
            foreach (GameObject obj in Selection.gameObjects)
            {
                MergeableCell cell = obj.GetComponent<MergeableCell>();
                if (cell != null)
                {
                    cells.Add(cell);
                }
            }
            
            return cells.ToArray();
        }
        
        /// <summary>
        /// 交换两个地块的属性
        /// </summary>
        /// <param name="cell1">地块1</param>
        /// <param name="cell2">地块2</param>
        private void SwapCellProperties(MergeableCell cell1, MergeableCell cell2)
        {
            if (cell1 == null || cell2 == null)
                return;
            
            // 交换区域ID
            int tempRegionId = cell1.BelongRegionId;
            cell1.BelongRegionId = cell2.BelongRegionId;
            cell2.BelongRegionId = tempRegionId;
            
            // 交换所需净化值
            int tempRequiredPurifiedNum = cell1.RequiredPurifiedNum;
            cell1.RequiredPurifiedNum = cell2.RequiredPurifiedNum;
            cell2.RequiredPurifiedNum = tempRequiredPurifiedNum;
            
            // 交换优先级
            int tempPurifiedPriority = cell1.PurifiedPriority;
            cell1.PurifiedPriority = cell2.PurifiedPriority;
            cell2.PurifiedPriority = tempPurifiedPriority;
            
            // 标记场景为已修改
            EditorUtility.SetDirty(cell1);
            EditorUtility.SetDirty(cell2);
            
            // 显示交换成功消息
            Debug.Log($"已交换地块 [{cell1.MapCoordinate.x},{cell1.MapCoordinate.y}] 和 [{cell2.MapCoordinate.x},{cell2.MapCoordinate.y}] 的属性");
            
            // 刷新编辑器
            SceneView.RepaintAll();
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
            _regionObj = GameObject.Find("Region");
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
                int tilesInColumn = 0; // 记录当前列中的瓦片数量
                // 这里采取倒序遍历，方便生成地块间隔
                for (int y = bounds.yMax - 1; y >= bounds.yMin ; y--)
                {
                    // 检查当前位置是否有瓦片
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = _tilemap.GetTile(cellPosition);
                    
                    // 跳过空白区域
                    if (tile == null)
                        continue;
                    
                    tilesInColumn++; // 增加当前列的瓦片计数
                    
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
                
                // 如果前一列的瓦片数量不为0且为偶数，则再次翻转isCellA的值
                if (tilesInColumn > 0 && tilesInColumn % 2 == 0)
                {
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
        
        /// <summary>
        /// 获取指定区域内的所有优先级并排序
        /// </summary>
        /// <param name="regionId">区域ID</param>
        /// <returns>从低到高排序的优先级数组</returns>
        private int[] GetPrioritiesInRegion(int regionId)
        {
            if (_regionObj == null || _regionObj.transform.childCount == 0)
                return new int[0];
            
            // 收集区域内所有不同的优先级
            HashSet<int> priorities = new HashSet<int>();
            
            foreach (Transform cellTransform in _regionObj.transform)
            {
                var cell = cellTransform.GetComponent<MergeableCell>();
                if (cell != null && cell.BelongRegionId == regionId)
                {
                    priorities.Add(cell.PurifiedPriority);
                }
            }
            
            // 转换为数组并从低到高排序
            int[] result = priorities.ToArray();
            System.Array.Sort(result);
            
            return result;
        }
        
        /// <summary>
        /// 高亮显示指定区域ID和优先级的地块
        /// </summary>
        /// <param name="regionId">区域ID</param>
        /// <param name="priorityId">优先级ID，0表示不筛选优先级</param>
        private void HighlightCellsByRegionId(int regionId, int priorityId = 0)
        {
            if (_regionObj == null || _regionObj.transform.childCount == 0)
                return;
            
            // 如果选择的区域ID为0，则清空选择
            if (regionId == 0)
            {
                Selection.objects = new Object[0];
                return;
            }
            
            // 收集属于指定区域ID的地块对象
            List<GameObject> selectedCells = new List<GameObject>();
            
            // 遍历所有地块，找出属于指定区域ID的地块
            foreach (Transform cellTransform in _regionObj.transform)
            {
                var cell = cellTransform.GetComponent<MergeableCell>();
                if (cell != null && cell.BelongRegionId == regionId)
                {
                    // 如果优先级ID为0或与地块优先级匹配，则选中该地块
                    if (priorityId == 0 || cell.PurifiedPriority == priorityId)
                    {
                        selectedCells.Add(cellTransform.gameObject);
                    }
                }
            }
            
            // 设置标记，表示这是工具选择，防止触发OnSelectionChanged重置选择
            _isToolSelection = true;
            
            // 设置Unity选择（会自动高亮显示）
            Selection.objects = selectedCells.ToArray();
            
            // 确保场景视图刷新
            SceneView.RepaintAll();
        }
    }
}