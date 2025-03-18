using System.Collections.Generic;
using Framework;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace TripleMerge
{
    /// <summary>
    /// 三合地图数据加载器
    /// </summary>
    public class MapDataLoader : GlobalSystem<MapDataLoader>
    {
        private const string MapDataAssetName = "Configs/TripleMapData/MapData.json";
        private static string MapDataPath = $"{Application.dataPath}/ExtraRes/Configs/TripleMapData/MapData.json";
        private MapData _mapData;
        private bool _isLoaded = false;

        /// <summary>
        /// 直接获取地图数据（编辑器使用）
        /// </summary>
        /// <returns></returns>
        public static MapData GetMapData()
        {
            if (File.Exists(MapDataPath))
            {
                var mapData = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(MapDataPath));
                if(mapData != null)
                {
                    mapData.OnAfterDeserialize();
                    return mapData;
                }
            }
            DebugUtil.LogError($"无法加载地图数据: {MapDataPath}");
            return null;
        }
        /// <summary>
        /// 加载地图数据
        /// </summary>
        /// <param name="mapName">地图名称，默认为MapData</param>
        /// <returns>是否加载成功</returns>
        public bool LoadMapData()
        {
            if (_isLoaded) return true;

            var ta = ResourcesManager.Instance.LoadResource<TextAsset>(MapDataAssetName);
            var mapDataCfg = JsonConvert.DeserializeObject<MapData>(ta.text); 
            if (mapDataCfg == null)
            {
                DebugUtil.LogError($"无法加载地图数据: {MapDataAssetName}");
                return false;
            }

            _mapData = mapDataCfg;
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
} 