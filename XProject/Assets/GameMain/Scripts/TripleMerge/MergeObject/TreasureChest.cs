using System;
using Config.TripleMerge;
using Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace TripleMerge
{
    public class TreasureChest : OnCellObject
    {
        public const int TreasureChestID = 99999; 
        protected SpriteRenderer IconSp { get; set; }
        private GameObject _draggingTips;
        protected override void OnInitialize()
        {
            IconSp = transform.Find("Icon").GetComponent<SpriteRenderer>();
            _draggingTips = transform.Find("DraggingTips").gameObject;
            _draggingTips.SetActive(false);
        }

        protected override void OnRecycle()
        {
            DebugUtil.Log(" 宝箱回收 ");
        }

        protected override void OnSelect()
        {
            _draggingTips.SetActive(true);
        }

        protected override void OnDeselect()
        {
            _draggingTips.SetActive(false);
        }

        protected override void OnCfgDataUpdated() { }

        protected override void OnClicked()
        {
            OpenChest();
        }

        protected override void OnDragBegin()
        {
            IconSp.sortingOrder = 10;
        }

        protected override void OnDragEnd()
        {
            OnBelongCellUpdated();
        }

        protected override void OnLongPressTrigger()
        {
            
        }

        protected override void OnBelongCellUpdated()
        {
            base.OnBelongCellUpdated();

            if (BelongCell != null)
            {
                IconSp.sortingOrder = -BelongCell.MapCoordinate.y;
            }
        }

        private const int GenerateItemNum = 10;
        public void OpenChest()
        {
            // 打开宝箱
            Deselect();
            var belongCell = BelongCell;
            if(belongCell != null)
            {
                belongCell.PlaceItem(null);
            }
            
            OnCellObjectPool.Recycle(this);

            var model = TripleMergeSystem.Instance.Model;
            
            // 记录打开次数，前15次开出的物品是通过配置固定的
            if (TryReleaseChestFromConfig(belongCell))
            {
                model.AddTreasureChestOpenTimes();
                return;
            }
            //如果超过15次则执行下面这段逻辑
            var resultCfg = ListPool<MergeableItemCfg>.Get();
            
            //todo 功能待定： 开启宝箱后，会获得11件物品，其中必然出现一个编号最靠前的，未完成的1级人物
            // 另外会出现3种合计10个已解锁的1级景观
            var skipNum = 0;
            var unlockedItems = model.UnlockedMergeableItems;
            var categoryWeightRandomList = ListPool<(MergeableItemCfg, int)>.Get();
            foreach (var threeMergeableItemId in unlockedItems)
            {
                if (!TripleMergeConfigManager.Instance.TryGetItemConfig(threeMergeableItemId, out var threeMergeableItem))continue;
                
                if (threeMergeableItem.Level != 1)
                    continue;

                if (Utils.ParseTripleMergeItemType(threeMergeableItem.ItemType) != TripleMergeItemType.MergeableNormal)
                    continue;

                if (threeMergeableItem.ProduceWeight <= 0)
                    continue;

                // 如果该合成链的最高级别物品已经通过合成解锁过
                var chain = TripleMergeConfigManager.Instance.GetChainConfig(threeMergeableItem.ChainId);
                var finalItemOfChain = chain.Chain[^1];
                if (model.SaveFileTripleMerge.UnlockedMergeableItems.ContainsKey(finalItemOfChain))
                {
                    Debug.Log($"物品[{threeMergeableItem.Id}]所处的合成链中，最高级别的物品[{finalItemOfChain}]已经通过合成得到过，不再开出该物品!");
                    skipNum++;
                    continue;
                }

                categoryWeightRandomList.Add((threeMergeableItem, threeMergeableItem.ProduceWeight));
            }

            if (categoryWeightRandomList.Count >= 3 || skipNum <= 0)
            {
                var itemCategoryNum = Mathf.Min(3, categoryWeightRandomList.Count);
                for (int i = 0; i < itemCategoryNum; i++)
                {
                    var randomIdx = CommonUtils.GetRandomWeightIndex(categoryWeightRandomList.ConvertAll(e => e.Item2));
                    resultCfg.Add(categoryWeightRandomList[randomIdx].Item1);
                    categoryWeightRandomList.RemoveAt(randomIdx);
                }
            }
            // 如果经过过滤已合成最高级别物品之后，宝箱可以开出的1级景观种类已经不足3种了
            else
            {
                foreach (var item in categoryWeightRandomList)
                {
                    resultCfg.Add(item.Item1);
                }

                // var lackOfNum = 3 - categoryWeightRandomList.Count;
                // var gotCnt = 0;
                // foreach (var chain in TripleMergeConfigManager.Instance.MergeChainList)
                // {
                //     if (gotCnt >= lackOfNum) break;
                //
                //     var item = TripleMergeConfigManager.Instance.GetItemConfig(chain.Chain[0]);
                //     if (item is not {ItemType: (int) TripleMergeItemType.MergeableNormal}) continue;
                //
                //     resultCfg.Add(item);
                //     gotCnt++;
                // }
            }

            ListPool<(MergeableItemCfg, int)>.Release(categoryWeightRandomList);

            var addToBubbleNum = 0;
            var cells = GetMergeableEmptyCellsByDistance(GenerateItemNum + 1, belongCell.transform.position);
            var remainItemNum = GenerateItemNum;

            var resultDict = DictionaryPool<int, int>.Get();
            for (var i = 0; i < resultCfg.Count; i++)
            {
                var itemCfg = resultCfg[i];
                var itemType = Utils.ParseTripleMergeItemType(itemCfg.ItemType);
                //var num = itemType == ThreeMergeItemType.Charactor ? 1 : i < result.Count - 1 ? Random.Range(1, remainItemNum - (result.Count - i)) : remainItemNum;
                int num;
                // 如果物品不是角色类型
                if (i < resultCfg.Count - 1) 
                {
                    // 如果当前索引 i 小于结果列表的倒数第二个索引，生成随机数量
                    num = Random.Range(1, remainItemNum - (resultCfg.Count - i));
                } 
                else 
                {
                    // 如果当前索引 i 是结果列表的最后一个物品，num 设置为剩余物品数量
                    num = remainItemNum;
                }
                if (!resultDict.TryAdd(itemCfg.Id, num))
                {
                    resultDict[itemCfg.Id] += num;
                }
                //if (itemType != ThreeMergeItemType.Charactor)
                remainItemNum -= num;

                for (int j = 0; j < num; j++)
                {
                    if (cells.Count == 0)
                    {
                        addToBubbleNum++;
                        // todo 加入气泡
                        //ThreeMergeSystem.Instance.Gameplay.MapManager.MergeableItemsBubble.AddItem(itemCfg.Id, hostCell.transform.position, skipProgress);
                    }
                    else
                    {
                        var cell = cells[0];
                        cells.Remove(cell);

                        var item = OnCellObjectPool.GetItem(itemCfg, cell.transform);
                        item.SetCfgData(itemCfg);
                        cell.PlaceItem(item);
                    }
                }
            }

            ListPool<MergeableItemCfg>.Release(resultCfg);
            ListPool<MergeableCell>.Release(cells);

            if (addToBubbleNum > 0)
            {
                //ThreeMergeSystem.Instance.Gameplay.MapManager.MergeableItemsBubble.Save();
            }

            TripleMergeSystem.Instance.Model.AddTreasureChestOpenTimes();
            DictionaryPool<int, int>.Release(resultDict);
            
            Debug.Log("OpenChest!!");
        }

        private bool TryReleaseChestFromConfig(MergeableCell belongCell)
        {
            var openTimes = TripleMergeSystem.Instance.Model.GetTreasureChestOpenTimes();
            if (!TripleMergeConfigManager.Instance.TryGetRewardBoxReward(openTimes + 1, out var chestRewardsCfg))
                return false;
            var addToBubbleNum = 0;
            var cells = GetMergeableEmptyCellsByDistance(chestRewardsCfg.Rewards.Count, belongCell.transform.position);
            var rewardItemDic = DictionaryPool<int, int>.Get();
            foreach (var rewardId in chestRewardsCfg.Rewards)
            {
                var reward = TripleMergeConfigManager.Instance.GetItemConfig(rewardId);
                if (cells.Count == 0)
                {
                    addToBubbleNum++;
                    DebugUtil.LogWarning("地块满了，需要加入气泡缓存起来");
                    //gamePlay.MapManager.MergeableItemsBubble.AddItem(reward.Id, hostCell.transform.position, skipProgress);
                }
                else
                {
                    var cell = cells[0];
                    cells.Remove(cell);

                    var item = OnCellObjectPool.GetItem(reward, cell.transform);
                    item.SetCfgData(reward);
                    cell.PlaceItem(item);
                }

                if (rewardItemDic.ContainsKey(reward.Id))
                    rewardItemDic[reward.Id]++;
                else
                    rewardItemDic[reward.Id] = 1;
            }

            DictionaryPool<int, int>.Release(rewardItemDic);

            if (addToBubbleNum > 0)
            {
                DebugUtil.LogWarning("有缓存物品，需要加入存档");
                //gamePlay.MapManager.MergeableItemsBubble.Save();
            }

            return true;
        }
        public void OnFlyBegin()
        {
            IconSp.sortingLayerName = "Top";
        }

        public void OnFlyEnd()
        {
            IconSp.sortingLayerName = "TripleMergeItem";
        }
    }
}