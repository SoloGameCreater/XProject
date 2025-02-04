using System.Collections.Generic;
using Config.TripleMerge;
using SaveFile.TripleMerge;

namespace TripleMerge
{
    public class TripleMergeModel
    {
        public SaveFileTripleMerge SaveFileTripleMerge { get; } = new();

        public void ClearData()
        {
            SaveFileTripleMerge.Clear();
        }

        public void AddRegionId(int regionId)
        {
            if (!SaveFileTripleMerge.OpenRegionIds.Contains(regionId))
                SaveFileTripleMerge.OpenRegionIds.Add(regionId);
        }

        public SaveFileTripleMergeCellData GetOrCreateCellData(string cellKey)
        {
            if (SaveFileTripleMerge.Cells.TryGetValue(cellKey, out var cellData)) return cellData;

            cellData = new SaveFileTripleMergeCellData();
            SaveFileTripleMerge.Cells.Add(cellKey, cellData);
            return cellData;
        }

        /// <summary>
        /// 通过ID获取合成物
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MergeableItemCfg GetMergeableItem(int id)
        {
            return TripleMergeConfigManager.Instance.MergeableItemCfgList.Find(x => x.Id == id);
        }

        /// <summary>
        /// 通过合成链ID获取初始合成物
        /// </summary>
        /// <param name="mergeChainId"></param>
        /// <returns></returns>
        public MergeableItemCfg GetMergeableItemByChainId(int mergeChainId)
        {
            foreach (var itemCfg in TripleMergeConfigManager.Instance.MergeableItemCfgList)
            {
                if (itemCfg.ChainId != mergeChainId) continue;
                if (itemCfg.Level == 1) return itemCfg;
            }

            return null;
        }
    }
}