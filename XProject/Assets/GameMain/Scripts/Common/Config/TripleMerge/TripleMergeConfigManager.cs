using System.Collections.Generic;
using Framework;
using TripleMerge;
using UnityEngine;

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
            // 获取当前剧情阶段
            var curPhase = TripleMergeSystem.Instance.Model.CurrentStoryPhase;
            // 根据当前剧情阶段获取对应的奖励
            var cfg = ChestOutputList.Find(c => c.Id == curPhase + 1);
            if (cfg == null)
            {
                config = null;
                DebugUtil.LogError($"没有找到剧情阶段{curPhase + 1}的奖励配置");
                return false;
            }

            // 根据奖励配置生成奖励
            config = new MergeTreasureChestRewards();
            // 开启次数(暂时没用，后期可能会根据开启次数来调整奖励)
            config.Index = index;
            config.Rewards = new List<int>();
            // 获取所有解锁的物品
            var allUnlockItemIds = MergeableItemList.FindAll(c => c.ItemType == 2);
            
            for (int i = 0; i < cfg.OutputCount; i++)
            {
                // 根据权重随机选择索引，然后获取对应等级
                var levelIndex = CommonUtils.GetRandomIndexByWeight(cfg.ItemLevelRatio);
                var itemLevel = cfg.ItemLevelRange[levelIndex];
                
                // 根据等级找到对应的物品ID
                var availableItems = allUnlockItemIds.FindAll(item => item.Level == itemLevel);
                if (availableItems.Count > 0)
                {
                    // 随机选择一个该等级的物品
                    var randomItem = availableItems[Random.Range(0, availableItems.Count)];
                    config.Rewards.Add(randomItem.Id);
                }
                else
                {
                    DebugUtil.LogWarning($"没有找到等级为{itemLevel}的物品，使用默认物品");
                    config.Rewards.Add(300028); // 使用默认物品ID
                }
            }

            return true;
        }

        public MergeChain GetChainConfig(int cfgDataChainId)
        {
            return MergeChainList.Find(c => c.Id == cfgDataChainId);
        }
    }
}