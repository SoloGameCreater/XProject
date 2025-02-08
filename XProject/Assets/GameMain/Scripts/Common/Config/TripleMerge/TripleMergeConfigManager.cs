using System.Collections.Generic;

namespace Config.TripleMerge
{
    public partial class TripleMergeConfigManager
    {
        private Dictionary<int, MergeableItemCfg> _itemCfgDict = new();
        
        public MergeableItemCfg GetItemConfig(int itemId)
        {
            if (_itemCfgDict.TryGetValue(itemId, out var cfg)) return cfg;

            cfg = MergeableItemCfgList.Find(c => c.Id == itemId);
            _itemCfgDict[itemId] = cfg;
            return cfg;
        }

        public bool TryGetItemConfig(int itemId, out MergeableItemCfg config)
        {
            if (_itemCfgDict.TryGetValue(itemId, out config)) return true;

            config = MergeableItemCfgList.Find(c => c.Id == itemId);
            if (config == null) return false;

            _itemCfgDict[itemId] = config;
            return true;
        }
    }
}