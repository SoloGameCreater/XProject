using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace TripleMerge
{
    /// <summary>
    /// 地图数据应用器 - 用于在游戏启动时加载和应用地图数据
    /// </summary>
    public class MapDataApplier : MonoBehaviour
    {
        [SerializeField] private string _mapDataName = "MapData";
        [SerializeField] private bool _autoApplyOnStart = true;
        
        private MergeableRegion _mergeableRegion;
        
        private void Start()
        {
            if (_autoApplyOnStart)
            {
                ApplyMapData();
            }
        }
        
        /// <summary>
        /// 应用地图数据
        /// </summary>
        public void ApplyMapData()
        {
            // 1. 加载地图数据
            if (!MapDataLoader.Instance.LoadMapData(_mapDataName))
            {
                DebugUtil.LogError("加载地图数据失败");
                return;
            }
            
            // 2. 获取地图区域组件
            _mergeableRegion = GetComponentInChildren<MergeableRegion>();
            if (_mergeableRegion == null)
            {
                DebugUtil.LogError("未找到MergeableRegion组件");
                return;
            }
            
            // 3. 获取所有地块
            var cells = new List<MergeableCell>();
            foreach (Transform child in _mergeableRegion.transform)
            {
                var cell = child.GetComponent<MergeableCell>();
                if (cell != null)
                {
                    cells.Add(cell);
                }
            }
            
            if (cells.Count == 0)
            {
                DebugUtil.LogWarning("未找到任何地块");
                return;
            }
            
            // 4. 应用数据到每个地块
            int appliedCount = 0;
            foreach (var cell in cells)
            {
                if (MapDataLoader.Instance.ApplyCellData(cell))
                {
                    appliedCount++;
                }
            }
            
            DebugUtil.Log($"成功应用地图数据到 {appliedCount}/{cells.Count} 个地块");
        }
        
        /// <summary>
        /// 重新加载地图数据
        /// </summary>
        public void ReloadMapData()
        {
            ApplyMapData();
        }
    }
} 