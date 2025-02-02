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

        public List<MergeableItemCfg> GetMergeableItem(int id)
        {
            return TripleMergeConfigManager.Instance.MergeableItemCfgList.FindAll(x => x.Id == id);
        }
    }
}