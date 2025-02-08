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

        // 修改地块状态
        public void SetCellState(SaveFileTripleMergeCellData cellData, int statusValue)
        {
            cellData.State = statusValue;
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

        public SaveFileTripleMergeItemData SetCellPlaceItem(SaveFileTripleMergeCellData cellData, int itemId)
        {
            var item = cellData.PlacedItem;
            item.ItemId = itemId;
            
            if (cellData.State == 1)
            {
                TryUnlockMergeableItemAndNotify(itemId);
            }

            var itemData = new SaveFileTripleMergeItemData();
            itemData.ItemId = itemId;
            return itemData;
        }
        public SaveFileTripleMergeItemData SetCellPlaceItem(SaveFileTripleMergeCellData cellData, SaveFileTripleMergeItemData itemData)
        {
            var item = cellData.PlacedItem;
            item.ItemId = itemData.ItemId;
            
            if (cellData.State == 1)
            {
                TryUnlockMergeableItemAndNotify(itemData.ItemId);
            }

            var itemDataCopy = new SaveFileTripleMergeItemData();
            itemDataCopy.ItemId = itemData.ItemId;
            return itemData;
        }
        public SaveFileTripleMergeItemData GetCellPlacedItem(SaveFileTripleMergeCellData storageData)
        {
            var copyModel = new SaveFileTripleMergeItemData();
            copyModel.ItemId = storageData.PlacedItem.ItemId;

            return copyModel;
        }
        private void TryUnlockMergeableItemAndNotify(int itemId)
        {
            // if (TripleMergeConfigManager.Instance.GetItemConfig(itemId) != null 
            //     && !_unlockedMergeableItems.Contains(itemId))
            // {
            //     AddUnlockMergeableItems(itemId);
            // }
        }
    }
}