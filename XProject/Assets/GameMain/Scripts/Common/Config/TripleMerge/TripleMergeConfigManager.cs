using System.Collections.Generic;
using Framework;

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
        public bool TryGetRewardBoxReward(int index, out MergeTreasureChestRewards config)
        {
            //config = ThreeMergeRewardBoxRewardsList.Find(c => c.Index == index);
            //return config != null;
            //todo 新增配置
            DebugUtil.LogWarning("现在缺少配置，一直都是false");
            config = null;
            return false;
        }
    }
}