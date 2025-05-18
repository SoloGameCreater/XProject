using System.Collections.Generic;
using Framework;

namespace Config.TripleMerge
{
    public partial class TripleMergeConfigManager : GlobalSystem<TripleMergeConfigManager>
    {
        private Dictionary<int, MergeableItem> _itemCfgDict = new();
        
        public MergeableItem GetItemConfig(int itemId)
        {
            if (_itemCfgDict.TryGetValue(itemId, out var cfg)) return cfg;

            cfg = MergeableItemList.Find(c => c.Id == itemId);
            _itemCfgDict[itemId] = cfg;
            return cfg;
        }

        public bool TryGetItemConfig(int itemId, out MergeableItem config)
        {
            if (_itemCfgDict.TryGetValue(itemId, out config)) return true;

            config = MergeableItemList.Find(c => c.Id == itemId);
            if (config == null) return false;

            _itemCfgDict[itemId] = config;
            return true;
        }
        public bool TryGetRewardBoxReward(int index, out MergeTreasureChestRewards config)
        {
            //config = ThreeMergeRewardBoxRewardsList.Find(c => c.Index == index);

            //todo 生成临时配置,默认生成5个最基础的元素
            config = new MergeTreasureChestRewards();
            config.Index = index;
            config.Rewards = new List<int>() { 300028, 300028, 300028, 300028, 300028 };
            return config != null;
        }

        public MergeChain GetChainConfig(int cfgDataChainId)
        {
           return MergeChainList.Find(c => c.Id == cfgDataChainId);
        }
    }
}