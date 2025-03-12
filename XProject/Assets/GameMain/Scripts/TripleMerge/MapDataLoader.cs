using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace TripleMerge
{
    /// <summary>
    /// 三合地图数据加载器
    /// </summary>
    public class MapDataLoader
    {
        private static MapDataLoader _instance;
        public static MapDataLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MapDataLoader();
                }
                return _instance;
            }
        }

        private MapData _mapData;
        private bool _isLoaded = false;

        /// <summary>
        /// 加载地图数据
        /// </summary>
        /// <param name="mapName">地图名称，默认为MapData</param>
        /// <returns>是否加载成功</returns>
        public bool LoadMapData(string mapName = "MapData")
        {
            if (_isLoaded) return true;

            TextAsset mapDataAsset = Resources.Load<TextAsset>($"TripleMapData/{mapName}");
            if (mapDataAsset == null)
            {
                DebugUtil.LogError($"无法加载地图数据: TripleMapData/{mapName}");
                return false;
            }

            _mapData = JsonUtility.FromJson<MapData>(mapDataAsset.text);
            if (_mapData == null)
            {
                DebugUtil.LogError("地图数据解析失败");
                return false;
            }

            _isLoaded = true;
            DebugUtil.Log($"成功加载地图数据，共 {_mapData.cells.Count} 个地块");
            return true;
        }

        /// <summary>
        /// 获取指定坐标的地块数据
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>地块数据，如果不存在则返回null</returns>
        public CellData GetCellData(int x, int y)
        {
            if (!_isLoaded)
            {
                DebugUtil.LogWarning("地图数据尚未加载");
                return null;
            }

            string key = $"{x}_{y}";
            if (_mapData.cells.TryGetValue(key, out CellData cellData))
            {
                return cellData;
            }

            return null;
        }

        /// <summary>
        /// 获取指定坐标的地块数据
        /// </summary>
        /// <param name="coordinate">坐标</param>
        /// <returns>地块数据，如果不存在则返回null</returns>
        public CellData GetCellData(Vector2Int coordinate)
        {
            return GetCellData(coordinate.x, coordinate.y);
        }

        /// <summary>
        /// 获取所有地块数据
        /// </summary>
        /// <returns>地块数据字典</returns>
        public Dictionary<string, CellData> GetAllCellData()
        {
            if (!_isLoaded)
            {
                DebugUtil.LogWarning("地图数据尚未加载");
                return new Dictionary<string, CellData>();
            }

            return _mapData.cells;
        }

        /// <summary>
        /// 应用地块数据到指定地块
        /// </summary>
        /// <param name="cell">目标地块</param>
        /// <returns>是否应用成功</returns>
        public bool ApplyCellData(MergeableCell cell)
        {
            if (!_isLoaded || cell == null)
            {
                return false;
            }

            CellData cellData = GetCellData(cell.MapCoordinate);
            if (cellData == null)
            {
                DebugUtil.LogWarning($"未找到坐标为 {cell.MapCoordinate} 的地块数据");
                return false;
            }

            // 应用数据
            cell.BelongRegionId = cellData.belongRegionId;
            cell.CellStatus = (MergeableCell.ECellStatus)cellData.cellStatus;
            cell.PurifiedPriority = cellData.purifiedPriority;
            cell.RequiredPurifiedNum = cellData.requiredPurifiedNum;
            cell.InitialPlacedItemId = cellData.initialPlacedItemId;

            return true;
        }
    }

    /// <summary>
    /// 地图数据类
    /// </summary>
    [System.Serializable]
    public class MapData : UnityEngine.ISerializationCallbackReceiver
    {
        // 由于Unity的JsonUtility不支持直接序列化Dictionary，我们需要一个自定义的序列化器
        [System.Serializable]
        public class CellDictionary
        {
            public string key;
            public CellData value;
        }

        public List<CellDictionary> cellList = new List<CellDictionary>();

        // 这个字段不会被序列化，仅用于内部处理
        [System.NonSerialized]
        public Dictionary<string, CellData> cells = new Dictionary<string, CellData>();

        // 在序列化前将Dictionary转换为List
        public void OnBeforeSerialize()
        {
            cellList.Clear();
            if (cells != null)
            {
                foreach (var pair in cells)
                {
                    cellList.Add(new CellDictionary { key = pair.Key, value = pair.Value });
                }
            }
        }

        // 在反序列化后将List转换回Dictionary
        public void OnAfterDeserialize()
        {
            cells = new Dictionary<string, CellData>();
            foreach (var item in cellList)
            {
                if (item.key != null && item.value != null)
                {
                    cells[item.key] = item.value;
                }
            }
        }
    }

    /// <summary>
    /// 单元格数据类
    /// </summary>
    [System.Serializable]
    public class CellData
    {
        public int belongRegionId;
        public int cellStatus;
        public int purifiedPriority;
        public int requiredPurifiedNum;
        public int initialPlacedItemId;
        public int[] mapCoordinate; // [x, y]
    }
} 