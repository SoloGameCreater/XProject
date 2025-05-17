using System;
using Config.TripleMerge;
using Sirenix.OdinInspector;
using UnityEngine;
using Framework;

namespace TripleMerge
{
    public partial class MapAreaRegion : MonoBehaviour
    {
#if UNITY_EDITOR
        [LabelText("所属地段ID")]
        [ValueDropdown(nameof(GetValidRegions))]
#endif
        public int ID = 0;
        public MapAreaComponent BelongArea { private set; get; }

        public Transform UnlockUITipPoint { private set; get; }

        public MapRegion CfgData { private set; get; }

        public bool IsUnlockPerformanceEnd { private set; get; }
        public GameObject LockedTip;
        public AreaRegionUnlockProgressView ProgressView { private set; get; }

        private void Awake()
        {
            LockedTip = transform.Find("CloudTipContent").gameObject;
            RegEventListener();
        }

        public void Initialize(MapAreaComponent mapArea)
        {
            BelongArea = mapArea;

            UnlockUITipPoint = transform.Find("UnlockTipPoint");

            foreach (var mapRegionCfg in TripleMergeConfigManager.Instance.MapRegionList)
            {
                if (mapRegionCfg.Id.Equals(ID))
                {
                    CfgData = mapRegionCfg;

                    break;
                }
            }

            LoadUnlockStatus();
        }

        private void LoadUnlockStatus()
        {
            if (!IsUnlock())
            {
                if (CfgData.PreviousRegion == 0)
                {
                    TripleMergeSystem.Instance.Model.AddRegionId(ID);
                    OnLockStatusChanged();
                }
            }
            else
            {
                OnLockStatusChanged();
            }
        }

        private void OnLockStatusChanged()
        {
            LockedTip?.SetActive(!IsUnlock());
        }

        public void BecomeUnlock()
        {
            ProgressView = null;
            DebugUtil.Log($"MapAreaRegion: {ID} 解锁");
        }

        /// <summary>
        /// 解锁进度条
        /// </summary>
        public void LoadUnlockProgressData()
        {
            var totalCellNumOfPreRegion = 0;
            var totalUnlockedCellNumOfPreRegion = 0;

            var playingMap = TripleMergeSystem.Instance.Gameplay.MapManager.MapArea;
            foreach (var cell in playingMap.MergeableCellsDictionary.Values)
            {
                if (cell.BelongRegionId > CfgData.PreviousRegion)
                {
                    continue;
                }

                totalCellNumOfPreRegion++;
                if (cell.CellStatus == MergeableCell.ECellStatus.Mergeable)
                {
                    totalUnlockedCellNumOfPreRegion++;
                }
            }
            ProgressView = TripleMergeSystem.Instance.Gameplay.MapUIManager.Show<AreaRegionUnlockProgressView>("UIMapAreaRegionUnlockProgressBar", UnlockUITipPoint.position);
            ProgressView.ViewRoot.SetParent(TripleMergeSystem.Instance.Gameplay.MapUIManager.GetOrCreateTransform("RegionUnlockProgressViews"));
            ProgressView.SetProgress(totalUnlockedCellNumOfPreRegion, totalCellNumOfPreRegion);
            ProgressView.BindRegion(this);
        }

        public bool IsUnlock()
        {
            return TripleMergeSystem.Instance.Model.IsRegionUnlocked(ID);
        }

        private void OnCellBecomeMergeable(BaseEvent baseEvent)
        {
            var cell = baseEvent.datas.Length > 0 ? baseEvent.datas[0] as MergeableCell : null;
            if (!cell) return;
            if (CfgData.PreviousRegion != 0 && cell.BelongRegionId == CfgData.PreviousRegion)
            {
                ProgressView.AddProgress();
                
                if (IsUnlock()) return;

                var totalCellNumOfPreRegion = 0;
                var totalUnlockedCellNumOfPreRegion = 0;

                var playingMap = TripleMergeSystem.Instance.Gameplay.MapManager.MapArea;
                foreach (var c in playingMap.MergeableCellsDictionary.Values)
                {
                    if (c.BelongRegionId > CfgData.PreviousRegion)
                    {
                        continue;
                    }

                    totalCellNumOfPreRegion++;
                    if (c.CellStatus == MergeableCell.ECellStatus.Mergeable)
                    {
                        totalUnlockedCellNumOfPreRegion++;
                    }
                }

                if (totalUnlockedCellNumOfPreRegion >= totalCellNumOfPreRegion)
                {
                    InvokeUnlock();
                }
            }
        }

        private void InvokeUnlock()
        {
            var storageModel = TripleMergeSystem.Instance.Model;

            storageModel.AddRegionId(ID);

            PushRegionUnlockPerformance();

            if (CfgData.UnlockMergeableItems?.Count > 0)
            {
                DebugUtil.Log($"这里应该发放气泡，功能缺失");
                // todo 气泡存储
                // var bubble = TripleMergeSystem.Instance.Gameplay.MapManager.MergeableItemsBubble;
                // var rewardDic = DictionaryPool<int, int>.Get();
                // for (var i = 0; i < CfgData.UnlockMergeableItems.Count; i++)
                // {
                //     var unlockItemId = CfgData.UnlockMergeableItems[i];
                //     var rewardNum = CfgData.UnlockMergeableItemNums[i];
                //     storageModel.AddUnlockMergeableItems(unlockItemId);
                //     for (int j = 0; j < rewardNum; j++)
                //     {
                //         bubble.AddItem(unlockItemId, UnlockUITipPoint.position, false);
                //     }

                //     if (rewardDic.ContainsKey(unlockItemId))
                //         rewardDic[unlockItemId] += rewardNum;
                //     else
                //         rewardDic[unlockItemId] = rewardNum;
                // }

                // DictionaryPool<int, int>.Release(rewardDic);
                // bubble.Save();
            }

            if (CfgData.UnlockRewardItems != null)
            {
                DebugUtil.Log($"这里应该发放解锁奖励，功能缺失");
                // todo 发放解锁奖励
                // for (var i = 0; i < CfgData.UnlockRewardItems.Count; i++)
                // {
                //     var rewardItemId = CfgData.UnlockRewardItems[i];
                //     var rewardItemNum = CfgData.UnlockRewardItemNums[i];

                //     CommonUtils.AddRewards(new List<ItemData> {new() {id = rewardItemId, cnt = rewardItemNum}}, new()
                //     {
                //         reason = BiEventMergeMatch.Types.ItemChangeReason.AreaUnlock,
                //         data1 = CfgData.Id.ToString()
                //     });
                // }
            }
        }

        private void PushRegionUnlockPerformance()
        {
            // 区域解锁表现
            // 如果当前解锁地块位于摄像机中心点，直接播放解锁表现
            // todo 否则先将摄像机移动到对应位置，再播放解锁表现
            var isCenter = true;
            if (isCenter)
            {
                OnLockStatusChanged();
                EventDispatcher.Instance.DispatchEventImmediately(EventEnum.TripleMergeOnRegionUnlocked, ID);

                TripleMergeSystem.Instance.Gameplay.MapManager.MapArea.UpdateNextUnlockRegion();

                IsUnlockPerformanceEnd = true;
            }
            else
            {

            }
        }

        private void OnDestroy()
        {
            UnRegEventListener();
        }

        private void RegEventListener()
        {
            EventDispatcher.Instance.AddEventListener(EventEnum.TripleMergeOnCellBecomeMergeable, OnCellBecomeMergeable);
        }

        private void UnRegEventListener()
        {
            EventDispatcher.Instance.RemoveEventListener(EventEnum.TripleMergeOnCellBecomeMergeable, OnCellBecomeMergeable);
        }
    }
}