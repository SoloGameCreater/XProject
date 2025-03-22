#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config.TripleMerge;
using DG.Tweening;
using Framework;
using SaveFile.TripleMerge;
using UnityEngine;
using UnityEngine.Pool;
using TripleMerge;

namespace TripleMerge
{
    // 合成地块
    [RequireComponent(typeof(PolygonCollider2D))]
    [Obfuscation]
    public partial class MergeableCell : ItemBase
    {
#if UNITY_EDITOR
        [LabelText("所属地段ID")]
        [ValueDropdown(nameof(GetValidRegions))]
#endif
        public int BelongRegionId;

        public enum ECellStatus
        {
#if UNITY_EDITOR
            [LabelText("未设置")]
#endif
            Unset,
#if UNITY_EDITOR
            [LabelText("可合成")]
#endif
            Mergeable,
#if UNITY_EDITOR
            [LabelText("未净化")]
#endif
            UnPurified,
#if UNITY_EDITOR
            [LabelText("未解锁")]
#endif
            Locked,
        }

        public enum EVertexType
        {
            LeftTop,
            LeftBottom,
            RightBottom,
            RightTop,
        }
#if UNITY_EDITOR
        [LabelText("初始默认放置合成物品")]
        [ValueDropdown(nameof(GetMergeableItemConfigList))]
#endif
        public int InitialPlacedItemId;

#if UNITY_EDITOR
        [LabelText("所需净化值")]
#endif
        public int RequiredPurifiedNum;

        // 当前净化值
        public int CurrentPurificationNum { private set; get; }
#if UNITY_EDITOR
        [LabelText("净化优先级")]
#endif
        public int PurifiedPriority;

#if UNITY_EDITOR
        [LabelText("地块状态")]
#endif
        public ECellStatus CellStatus;

#if UNITY_EDITOR
        [LabelText("地块坐标")]
#endif
        public Vector2Int MapCoordinate;

        /// <summary>
        /// 所属三合区域
        /// </summary>
        public MergeableRegion BelongRegion { private set; get; }

        /// <summary>
        /// 当前地块上放置的三合物品
        /// </summary>
#if UNITY_EDITOR
        [ShowInInspector]
#endif
        private OnCellObject _placeableItem;

        public OnCellObject PlacedItem => _placeableItem;

        /// <summary>
        /// 左侧的地块
        /// </summary>
        public MergeableCell Left { set; get; }

        /// <summary>
        /// 右侧的地块
        /// </summary>
        public MergeableCell Right { set; get; }

        /// <summary>
        /// 上方的地块
        /// </summary>
        public MergeableCell Above { set; get; }

        /// <summary>
        /// 下方的地块
        /// </summary>
        public MergeableCell Below { set; get; }

        /// <summary>
        /// 交互范围
        /// </summary>
        public BoxCollider2D InteractiveTrigger { set; get; }

        /// <summary>
        /// 未净化状态地块
        /// </summary>
        private SpriteRenderer _unPurifiedRenderer;

        /// <summary>
        /// Collider
        /// </summary>
        private PolygonCollider2D _cellCollider;

        public Transform PlaceItemRoot { private set; get; }

        /// <summary>
        /// 可合成地块检查
        /// </summary>
        private readonly List<MergeableObject> _continuousCellsQueryResult = new();

        private bool _isMouseDown;

        private bool _isInitialized;

        // 存档数据
        private SaveFileTripleMergeCellData _saveData;
        public string SaveKey => $"{MapCoordinate.x}_{MapCoordinate.y}";
        public CellData CellConfig { get; set; }
        // 使用可配置的延迟值
        private const float MERGE_DELAY = 0.035f;

        public void Initialize(MergeableRegion hostRegion)
        {
            BelongRegion = hostRegion;

            _cellCollider = transform.GetComponent<PolygonCollider2D>();

            InteractiveTrigger = transform.Find("Center").GetComponent<BoxCollider2D>();

            PlaceItemRoot = new GameObject("PlaceItemRoot").transform;
            PlaceItemRoot.SetParent(transform);
            PlaceItemRoot.localPosition = Vector3.zero;
        }

        public void LoadData()
        {
            _isInitialized = false;
            if (!MapDataLoader.Instance.ApplyCellData(this))
            {
                Debug.LogWarning($"未找到坐标为 {MapCoordinate} 的地块数据");
            }

            _saveData = TripleMergeSystem.Instance.Model.GetOrCreateCellData(SaveKey);
            var isNeverStorageBefore = _saveData.State == (int)ECellStatus.Unset;
            InitCellStatus(isNeverStorageBefore);
            InitPlacedItem(isNeverStorageBefore);

            _isInitialized = true;
        }

        /// <summary>
        /// 地块状态初始化
        /// </summary>
        /// <param name="isNeverStorageBefore"></param>
        private void InitCellStatus(bool isNeverStorageBefore)
        {
            CurrentPurificationNum = _saveData.PurificationValue;
            // 如果从存档数据中读取的状态是未设置，则代表是初始状态
            if (isNeverStorageBefore)
            {
                //读取初始配置信息，如果初始配置时，该地块需要一定数量的净化值，则代表其初始状态是未净化状态
                CellStatus = RequiredPurifiedNum > 0 ? ECellStatus.UnPurified : ECellStatus.Mergeable;

                TripleMergeSystem.Instance.Model.SetCellState(_saveData, (int)CellStatus);
            }
            else
            {
                //首先从存档数据中读取状态
                CellStatus = (ECellStatus)_saveData.State;

                if (CellStatus == ECellStatus.Locked) //如果时锁定的地块，从新检查路段解锁状态，防止某些情况路段解锁了之后地块信息没正常更新
                {
                    // if (BelongRegion.BelongArea.AreaRegionDictionary.TryGetValue(HostRegionId, out var belongToAreaRegion) 
                    // && belongToAreaRegion.IsUnlock())
                    // {
                    //     //读取初始配置信息，如果初始配置时，该地块需要一定数量的净化值，则代表其初始状态是未净化状态
                    //     CellStatus = RequiredPurifiedNum > 0 ? ECellStatus.UnPurified : ECellStatus.Mergeable;
                    // }
                }
            }
            // 地块置灰
            if (CellStatus >= ECellStatus.UnPurified)
            {
                _unPurifiedRenderer = Utils.InstantiateWorldGameObject("TripleMerge/Prefabs/MergeCell/UnPurifiedCell", transform).GetComponent<SpriteRenderer>();
                _unPurifiedRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
            }
        }

        private void InitPlacedItem(bool isNeverStorageBefore)
        {
            var placedItemId = 0;

            // 如果从存档数据中读取的状态是未设置，则代表是初始状态
            if (isNeverStorageBefore)
            {
                // 则读取初始配置信息，如果初始配置时，该地块是否会默认初始放置任何合成物
                placedItemId = InitialPlacedItemId;
                if (placedItemId != 0)
                {
                    TripleMergeSystem.Instance.Model.SetCellPlaceItem(_saveData, placedItemId);
                }
            }
            else
            {
                // 首先从存档数据中读取当前地块上放置的合成物品
                placedItemId = _saveData.PlacedItem.ItemId;
            }

            if (placedItemId != 0 && TripleMergeConfigManager.Instance.TryGetItemConfig(placedItemId, out var placedItemCfg))
            {
                OnCellObject placedItem = OnCellObjectPool.GetItem(placedItemCfg);

                placedItem.SetCfgData(placedItemCfg);

                if (placedItem is MergeableObject mergeableObject)
                {
                    mergeableObject.IsGray = CellStatus >= ECellStatus.UnPurified;
                }

                PlaceItem(placedItem);
            }
        }

        public void PlaceItem(OnCellObject item, Action onPlacedAction = null, Action onMergedAction = null, bool showMoveTween = true)
        {
            if (item == null)
            {
                SetPlacedItem(null);
                TripleMergeSystem.Instance.Model.SetCellPlaceItem(_saveData, 0);
                return;
            }
            // 先清除其原本的地块信息
            if (item.BelongCell != null)
            {
#if UNITY_EDITOR
                DebugUtil.Log($"Clear placed item {item.GUID} belong cell: {item.BelongCell.MapCoordinate}");
#endif
                var oldCell = item.BelongCell;
                item.BelongCell = null;  // 先断开引用防止循环
                if (oldCell.PlacedItem == item)
                {
                    oldCell.SetPlacedItem(null);
                }
            }

            // 绑定地块和Item的关系
            if (_isInitialized)
            {
                item.BindItemSaveData(item.ItemSaveData != null
                    ? TripleMergeSystem.Instance.Model.SetCellPlaceItem(_saveData, item.ItemSaveData)
                    : TripleMergeSystem.Instance.Model.SetCellPlaceItem(_saveData, item.CfgData.Id));
            }
            else
            {
                item.BindItemSaveData(TripleMergeSystem.Instance.Model.GetCellPlacedItem(_saveData));
            }

            // 首先将合成物放到自身地块的位置中心
            PlaceItemRoot.localPosition = Vector2.zero;
            // 放置合成物品
            var itemTrans = item.transform;
            itemTrans.SetParent(PlaceItemRoot);
            if (showMoveTween)
            {
                itemTrans.DOLocalMove(new Vector3(0, 0, itemTrans.position.z), 0.15f).OnComplete(OnItemPlaced);
            }
            else
            {
                itemTrans.localPosition = Vector3.zero;
                OnItemPlaced();
            }

            return;

            void OnItemPlaced()
            {

#if UNITY_EDITOR
                DebugUtil.Log($"{item.GUID} Placed to cell: {MapCoordinate}");
#endif
                // 5. 处理放置逻辑
                onPlacedAction?.Invoke();

                if (PlacedItem == null)
                {
                    SetPlacedItem(item);
                    onMergedAction?.Invoke();
                    return;
                }

                if (item == PlacedItem) return;

                // 6. 处理合成逻辑
                if (item.CfgData.Id == PlacedItem.CfgData.Id || item.IsUniversalCard)
                {
                    int callerItemId = PlacedItem.CfgData.Id;
                    if (TryToMerge(item, (finalItemId) => { DebugUtil.Log($"[MERGE END] caller:{callerItemId} . final: {finalItemId}"); }))
                    {
                        onMergedAction?.Invoke();
                        return;
                    }
                }

                SetPlacedItem(item);
                onMergedAction?.Invoke();
            }
        }

        private void SetPlacedItem(OnCellObject item)
        {
            var tobeReplaceItems = ListPool<OnCellObject>.Get();

            if (_placeableItem != null)
            {
                if (item != null)
                {
                    tobeReplaceItems.Add(_placeableItem);
                }

                ClearItemReference();
            }

            _placeableItem = item;
            if (_placeableItem != null)
                _placeableItem.BelongCell = this;

            HandleItemPlacement(tobeReplaceItems);
        }

        // 清除item引用地块
        private void ClearItemReference()
        {
            var takeOffItem = _placeableItem;
            if (takeOffItem != null && takeOffItem.BelongCell != null)
            {
#if UNITY_EDITOR
                DebugUtil.Log($"item {takeOffItem.GUID} 从 :{takeOffItem.BelongCell.MapCoordinate} 被挤走");
#endif
                // 因为老的物品是从该地块被挤出去的，所以这时其实原本的地块已经有了新的物品了，就应该先清除老物品的地块信息，以免地块物品信息被错误清除
                takeOffItem.BelongCell = null;
            }
        }

        private void HandleItemPlacement(List<OnCellObject> displacedItems)
        {
            // 如果有被挤出的物品，尝试重新放置它们
            if (displacedItems.Count > 0)
            {
                var nearestEmptyCells = ListPool<MergeableCell>.Get();

                foreach (var displacedItem in displacedItems)
                {
                    FindNearestEmptyCells(1, ref nearestEmptyCells);

                    if (nearestEmptyCells.Count > 0)
                    {
                        nearestEmptyCells[0].PlaceItem(displacedItem);
#if UNITY_EDITOR
                        DebugUtil.Log($"item {displacedItem.GUID} 被放置到 :{nearestEmptyCells[0].MapCoordinate}");
#endif
                    }
                    else
                    {

                        DebugUtil.LogWarning("没有空余地块，尝试将多余的收集物储存起来");
                        // todo 如果找不到空格子，将物品放入存储气泡
                        // var storageBubble = ThreeMergeSystem.Instance.Gameplay.MapManager.MergeableItemsBubble;
                        // storageBubble.AddItem(displacedItem.CfgData.Id, displacedItem.transform.position);
                        // storageBubble.Save();

                        OnCellObjectPool.Recycle(displacedItem);
                    }
                }

                ListPool<MergeableCell>.Release(nearestEmptyCells);
            }

            ListPool<OnCellObject>.Release(displacedItems);

            // 更新碰撞体状态
            _cellCollider.enabled = CellStatus == ECellStatus.Locked || _placeableItem == null;
        }

        /// <summary>
        /// 找到最近的空地块
        /// </summary>
        /// <param name="targetCellNum">需要查找的空地块数量</param>
        /// <param name="result">结果列表</param>
        private void FindNearestEmptyCells(int targetCellNum, ref List<MergeableCell> result)
        {
            result.Clear();

            // 获取临时列表
            var tempList = ListPool<MergeableCell>.Get();
            try
            {
                // 确定查询范围
                Dictionary<Vector2Int, MergeableCell> targetCells = TripleMergeSystem.Instance.Gameplay.MapManager.MapArea.MergeableCellsDictionary;

                // 健壮性检查
                if (targetCells == null || targetCells.Count == 0)
                {
                    DebugUtil.LogWarning("目标单元格字典为空");
                    return;
                }

                // 预分配容量
                tempList.Capacity = targetCells.Count;

                // 收集所有空地块
                foreach (var cell in targetCells.Values)
                {
                    if (cell.PlacedItem == null && cell.CellStatus == ECellStatus.Mergeable)
                    {
                        tempList.Add(cell);
                    }
                }

                // 如果找到的空地块数量不足，直接返回所有找到的空地块
                if (tempList.Count <= targetCellNum)
                {
                    result.AddRange(tempList);
                    return;
                }
                // 按照与当前格子的距离对结果进行排序，距离近的优先
                tempList.Sort((left, right) =>
                {
                    var leftToSelf = Vector2.Distance(left.MapCoordinate, MapCoordinate);
                    var rightToSelf = Vector2.Distance(right.MapCoordinate, MapCoordinate);
                    return leftToSelf < rightToSelf ? -1 : 1;
                });

                // 只取前N个结果
                for (int i = 0; i < targetCellNum; i++)
                {
                    result.Add(tempList[i]);
                }
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"查找最近空地块时发生异常: {e}");
            }
            finally
            {
                // 释放临时列表
                ListPool<MergeableCell>.Release(tempList);
            }
        }

        private int Partition(List<MergeableCell> cells, int left, int right)
        {
            var pivot = cells[right];
            var pivotDistance = Vector2.Distance(pivot.transform.position, transform.position);
            var i = left - 1;

            for (int j = left; j < right; j++)
            {
                var distance = Vector2.Distance(cells[j].transform.position, transform.position);
                if (distance < pivotDistance)
                {
                    i++;
                    (cells[i], cells[j]) = (cells[j], cells[i]);
                }
            }

            (cells[i + 1], cells[right]) = (cells[right], cells[i + 1]);
            return i + 1;
        }

        public bool CanPlaceTargetSizeItem()
        {
            return CellStatus == ECellStatus.Mergeable;
        }
        public bool TryToMerge(OnCellObject item = null, Action<int> onFinish = null)
        {
            if (item == null)
            {
                item = PlacedItem;
            }

            if (item is not MergeableObject mergeableObject)
            {
                return false;
            }

            if (item.CfgData.ChainId == 0)
            {
                return false;
            }

            try
            {
                var continuousCell = GetContinuousSameItems(mergeableObject);
                var continuousCnt = continuousCell.Count;
                if (continuousCnt >= 3)
                {
                    DebugUtil.Log($"三合开始");

                    // 能够进行合成的情况，先进行合成计算，合成完毕后，再对合成完毕后的合成物进行新的位置分配
                    DoMerge(ref continuousCell, (finalMergeItemId, mergeResult) =>
                        OnMergeCompleted(finalMergeItemId, mergeResult, onFinish));

                    return true;
                }
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"尝试合成时发生异常: {e}");
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = false;
            }

            return false;
        }

        private void OnMergeCompleted(int finalMergeItemId, Dictionary<int, int> mergeResult, Action<int> onFinish)
        {
            DebugUtil.Log($"三合完成");
            StartCoroutine(DelayedKeepMerge(finalMergeItemId, onFinish));
        }

        private IEnumerator DelayedKeepMerge(int finalMergeItemId, Action<int> onFinish)
        {
            yield return new WaitForSeconds(MERGE_DELAY);

            // 检查对象是否仍然有效
            if (this == null || !gameObject.activeInHierarchy)
            {
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = false;
                yield break;
            }

            ProcessAfterMerge(finalMergeItemId, onFinish);
        }

        private void ProcessAfterMerge(int finalMergeItemId, Action<int> onFinish)
        {
            TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = false;
            // 如果合出的是万能卡，不允许combo合成
            if (CheckAndHandleMaxLevelItem())
            {
                return;
            }

            // 使用迭代而非递归方式处理连续合成
            StartCoroutine(ContinuousMergeCoroutine(finalMergeItemId, onFinish));
        }

        private bool CheckAndHandleMaxLevelItem()
        {
            if (_placeableItem != null && _placeableItem.IsUniversalCard)
            {
                var chainCfg = TripleMergeConfigManager.Instance.GetChainConfig(_placeableItem.CfgData.ChainId);
                if (chainCfg.Chain[^1] == _placeableItem.CfgData.Id)
                {
                    _placeableItem.PlayMaxTipAnim();
                }
                return true;
            }

            if (_placeableItem != null)
            {
                var chainCfg = TripleMergeConfigManager.Instance.GetChainConfig(_placeableItem.CfgData.ChainId);
                if (chainCfg.Chain[^1] == _placeableItem.CfgData.Id)
                {
                    _placeableItem.PlayMaxTipAnim();
                    return true;
                }
            }

            return false;
        }

        private IEnumerator ContinuousMergeCoroutine(int finalMergeItemId, Action<int> onFinish)
        {
            bool canContinue = true;
            int currentMergeItemId = finalMergeItemId;

            while (canContinue)
            {
                canContinue = TryToMerge(null, null);

                if (!canContinue)
                {
                    onFinish?.Invoke(currentMergeItemId);
                    CheckAndHandleMaxLevelItem();
                    break;
                }

                yield return new WaitForSeconds(MERGE_DELAY);
            }
        }


        private const float BeforeMergePerformDuration = 0.15f;
        private const float AfterMergePerformDuration = 0.15f;
        /// <summary>
        /// 执行三合一合并操作
        /// </summary>
        /// <param name="continuousItems">连续的相同物品列表</param>
        /// <param name="onMergeCompleted">合并完成后的回调</param>
        private void DoMerge(ref IReadOnlyList<MergeableObject> continuousItems, Action<int, Dictionary<int, int>> onMergeCompleted = null)
        {
            // 用于存储最终合成物品的ID
            var finalMergeItemId = 0;
            // 资源池初始化
            var afterMergeItems = ListPool<MergeableObject>.Get();
            var tobeMergeItems = ListPool<MergeableObject>.Get();
            var unlockNewItems = HashSetPool<int>.Get();
            var mergeResults = DictionaryPool<int, int>.Get();

            // 获取当前格子的位置，用于合成物品的动画效果
            var cellPosition = transform.position;
            try
            {
                // 设置合并状态标记
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = true;

                // 第一步：准备合成物品
                // 将参与合成的物品从各自格子中移除，并添加到待合成列表
                for (var i = 0; i < continuousItems.Count; i++)
                {
                    var item = continuousItems[i];
                    // 标记物品正在合成中
                    item.IsMerging = true;
                    tobeMergeItems.Add(item);

                    if (item.BelongCell != null)
                    {
                        // 从原格子中移除物品
                        item.BelongCell.PlaceItem(null);
                        item.BelongCell = null;

                        // 如果不跳过合成进度动画，则播放物品移动到合成中心的动画
                        //if (!skipMergeProgress)
                        {
                            item.transform.DOKill();
                            // 物品移动到合成中心位置
                            item.transform.DOMove(new Vector3(cellPosition.x, cellPosition.y, item.transform.position.z), BeforeMergePerformDuration).SetEase(Ease.InSine).SetAutoKill(true);
                            // 物品缩小至消失的动画
                            item.transform.DOScale(0, BeforeMergePerformDuration).SetEase(Ease.InSine).SetAutoKill(true);
                            // 物品透明度降低的动画，最后一个物品完成时触发合成计算
                            item.ItemRenderer.DOFade(0, BeforeMergePerformDuration).SetEase(Ease.InSine).SetAutoKill(true).OnComplete(i == continuousItems.Count - 1 ?
                                CalculateMerge : null);
                        }
                    }
                }

                void CalculateMerge()
                {
                    // 执行合并计算
                    finalMergeItemId = PerformMergeCalculation(tobeMergeItems, afterMergeItems, mergeResults, unlockNewItems);
                    // 放置合并后的物品
                    PlaceMergedItems(afterMergeItems, cellPosition);
                    // 调用合并完成回调
                    onMergeCompleted?.Invoke(finalMergeItemId, mergeResults);

                    // 释放资源
                    ListPool<MergeableObject>.Release(afterMergeItems);
                    ListPool<MergeableObject>.Release(tobeMergeItems);
                    HashSetPool<int>.Release(unlockNewItems);
                    DictionaryPool<int, int>.Release(mergeResults);
                }
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"三合合成时发生异常:{e}");
                // 发生异常时重置合并状态
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = false;

                // 确保异常情况下也释放资源
                ListPool<MergeableObject>.Release(afterMergeItems);
                ListPool<MergeableObject>.Release(tobeMergeItems);
                HashSetPool<int>.Release(unlockNewItems);
                DictionaryPool<int, int>.Release(mergeResults);
            }
        }

        /// <summary>
        /// 执行合并计算逻辑
        /// </summary>
        /// <returns>最终合并出的物品ID</returns>
        private int PerformMergeCalculation(List<MergeableObject> tobeMergeItems, List<MergeableObject> afterMergeItems,
            Dictionary<int, int> mergeResults, HashSet<int> unlockNewItems)
        {
            // 获取合并链和基础物品
            var (targetChain, mergeBaseItem) = GetMergeChainAndBaseItem(tobeMergeItems);
            if (targetChain == null || IsMaxLevelItem(targetChain, tobeMergeItems[0].CfgData.Id))
            {
                return 0;
            }

            int finalMergeItemId = 0;

            // 执行合并循环
            while (tobeMergeItems.Count >= 3)
            {
                finalMergeItemId = MergeItemsBatch(tobeMergeItems, targetChain, mergeBaseItem,
                    mergeResults, unlockNewItems, afterMergeItems);

                // 更新基础物品为新生成的物品
                if (tobeMergeItems.Count >= 3)
                {
                    mergeBaseItem = tobeMergeItems[0].CfgData;
                }
            }

            return finalMergeItemId;
        }

        /// <summary>
        /// 获取合并链和基础物品
        /// </summary>
        private (MergeChain chain, MergeableItem baseItem) GetMergeChainAndBaseItem(List<MergeableObject> items)
        {
            foreach (var item in items.Where(item => !item.IsUniversalCard))
            {
                var chain = TripleMergeConfigManager.Instance.GetChainConfig(item.CfgData.ChainId);
                return (chain, item.CfgData);
            }

            // 如果全是万能卡，返回空
            return (null, null);
        }

        /// <summary>
        /// 检查物品是否已是最高级别
        /// </summary>
        private bool IsMaxLevelItem(MergeChain chain, int itemId)
        {
            if (chain.Chain.IndexOf(itemId) == chain.Chain.Count - 1)
            {
                DebugUtil.LogError($"三合合成物ID[{itemId}]已经是最高级别物品了");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 合并一批物品（每3个合成1个高级物品）
        /// </summary>
        /// <returns>合并出的物品ID</returns>
        private int MergeItemsBatch(List<MergeableObject> tobeMergeItems,
                                    MergeChain targetChain,
                                    MergeableItem mergeBaseItem,
                                    Dictionary<int, int> mergeResults,
                                    HashSet<int> unlockNewItems,
                                    List<MergeableObject> afterMergeItems)
        {
            var itemId = mergeBaseItem.Id;

            // 计算合并结果
            var nextLevelItemNumAfterMerge = tobeMergeItems.Count / 3;
            var remainCurrentLevelNum = tobeMergeItems.Count % 3;

            // 记录剩余物品
            if (remainCurrentLevelNum > 0)
            {
                mergeResults[itemId] = remainCurrentLevelNum;
            }

            // 移除已合成的物品
            for (var i = tobeMergeItems.Count - 1; i >= remainCurrentLevelNum; i--)
            {
                var tobeDestroyItem = tobeMergeItems[i];
                tobeDestroyItem.IsMerging = false;
                // 从待合成列表中移除
                tobeMergeItems.Remove(tobeDestroyItem);
                // 回收物品对象
                OnCellObjectPool.Recycle(tobeDestroyItem);
            }
            // 将剩余的当前级别物品添加到合成后物品列表
            foreach (var mergeableObject in tobeMergeItems)
            {
                mergeableObject.IsMerging = false;
                afterMergeItems.Add(mergeableObject);
            }

            // 清空待合成列表准备下一轮合成
            tobeMergeItems.Clear();

            // 生成下一级物品
            var nextLevelItemId = GenerateNextLevelItems(targetChain, itemId, nextLevelItemNumAfterMerge,
                tobeMergeItems, unlockNewItems);

            // 处理剩余物品
            if (tobeMergeItems.Count < 3)
            {
                afterMergeItems.AddRange(tobeMergeItems);
                mergeResults[nextLevelItemId] = tobeMergeItems.Count;
                tobeMergeItems.Clear();
            }

            return nextLevelItemId;
        }

        /// <summary>
        /// 生成下一级物品
        /// </summary>
        /// <returns>生成的物品ID</returns>
        private int GenerateNextLevelItems(MergeChain targetChain, int currentItemId, int count,
            List<MergeableObject> targetList, HashSet<int> unlockNewItems)
        {
            // 获取下一级物品配置
            int currentIndex = targetChain.Chain.IndexOf(currentItemId);
            var nextLevelItemCfg = TripleMergeConfigManager.Instance.GetItemConfig(targetChain.Chain[currentIndex + 1]);

            // 生成物品
            for (int i = 0; i < count; i++)
            {
                unlockNewItems.Add(nextLevelItemCfg.Id);
                var nextLevelItem = OnCellObjectPool.GetItem(nextLevelItemCfg) as MergeableObject;
                nextLevelItem.SetCfgData(nextLevelItemCfg);
                nextLevelItem.transform.position = new Vector3(transform.position.x, transform.position.y, nextLevelItem.transform.position.z);
                targetList.Add(nextLevelItem);
            }

            return nextLevelItemCfg.Id;
        }

        /// <summary>
        /// 放置合并后的物品
        /// </summary>
        private void PlaceMergedItems(List<MergeableObject> afterMergeItems, Vector3 cellPosition)
        {
            foreach (var item in afterMergeItems)
            {
                // 查找最近的空单元格
                var nearestEmptyCells = ListPool<MergeableCell>.Get();
                FindNearestEmptyCells(1, ref nearestEmptyCells);

                if (nearestEmptyCells.Count > 0)
                {
                    // 放置物品
                    nearestEmptyCells[0].PlaceItem(item);
                    // 播放物品出现动画
                    //if (!skipMergeProgress)
                    {
                        var originColor = item.ItemRenderer.color;
                        originColor.a = 0;
                        item.ItemRenderer.color = originColor;
                        var itemTransform = item.transform;
                        itemTransform.localScale = Vector3.zero;
                        itemTransform.position = new Vector3(cellPosition.x, cellPosition.y, itemTransform.position.z);
                        item.transform.DOLocalMove(new Vector3(0, 0, item.transform.position.z), AfterMergePerformDuration).SetEase(Ease.OutCubic).SetAutoKill(true);
                        item.transform.DOScale(1, AfterMergePerformDuration).SetEase(Ease.OutCubic).SetAutoKill(true);
                        item.ItemRenderer.DOFade(1, AfterMergePerformDuration).SetEase(Ease.OutCubic).SetAutoKill(true);
                    }
                }
                else
                {
                    // 没有空单元格，回收物品
                    OnCellObjectPool.Recycle(item);
                }

                ListPool<MergeableCell>.Release(nearestEmptyCells);
            }
        }

        /// <summary>
        /// 以指定三合地块为中心锚点，查询与其连续的并且放置了指定合成物的地块列表
        /// </summary>
        public IReadOnlyList<MergeableObject> GetContinuousSameItems(MergeableObject referenceItem)
        {
            _continuousCellsQueryResult.Clear();

            // 记录参考物品是否为万能卡
            var referenceItemIsUniversalCard = referenceItem.IsUniversalCard;
            var queriedCells = HashSetPool<MergeableCell>.Get();

            _continuousCellsQueryResult.Add(referenceItem);

            var callerItem = referenceItem;

            // 处理当前格子上的物品
            if (PlacedItem != null && PlacedItem != referenceItem && PlacedItem is MergeableObject mergeableItem)
            {
                // 如果参考物品不是万能卡
                if (!referenceItemIsUniversalCard)
                {
                    // 如果是普通物品，检查ID是否相同
                    if (PlacedItem.CfgData.Id == referenceItem.CfgData.Id)
                    {
                        _continuousCellsQueryResult.Add(mergeableItem);
                    }
                }
            }
            else
            {
                // 如果参考物品是万能卡且当前格子为空，直接返回
                if (referenceItemIsUniversalCard)
                {
                    return _continuousCellsQueryResult;
                }
            }

            // 递归查询相邻格子
            QueryCell(this);

            HashSetPool<MergeableCell>.Release(queriedCells);

            // 处理万能卡的特殊合成规则
            if (_continuousCellsQueryResult.Count > 3 && callerItem.IsUniversalCard)
            {
                // 根据距离和万能卡优先级排序
                _continuousCellsQueryResult.Sort((left, right) =>
                {
                    var leftPriority = 0f;
                    var rightPriority = 0f;

                    if (left == callerItem)
                    {
                        leftPriority = float.MinValue;
                    }
                    else
                    {
                        if (left.IsUniversalCard && Mathf.Approximately(Vector2.Distance(left.BelongCell.MapCoordinate, referenceItem.BelongCell.MapCoordinate), 1))
                        {
                            leftPriority += 1.5f;
                        }

                        leftPriority += Vector2.Distance(_placeableItem.BelongCell.MapCoordinate, left.BelongCell.MapCoordinate);
                    }

                    if (right == callerItem)
                    {
                        rightPriority = float.MinValue;
                    }
                    else
                    {
                        if (right.IsUniversalCard && Mathf.Approximately(Vector2.Distance(left.BelongCell.MapCoordinate, referenceItem.BelongCell.MapCoordinate), 1))
                        {
                            rightPriority += 1.5f;
                        }

                        rightPriority += Vector2.Distance(_placeableItem.BelongCell.MapCoordinate, right.BelongCell.MapCoordinate);
                    }

                    if (leftPriority < rightPriority)
                    {
                        return -1;
                    }
                    else if (leftPriority > rightPriority)
                    {
                        return 1;
                    }

                    return 0;
                });

                // 只保留前3个物品
                for (var i = _continuousCellsQueryResult.Count - 1; i >= 3; i--)
                {
                    _continuousCellsQueryResult.Remove(_continuousCellsQueryResult[i]);
                }
            }

            return _continuousCellsQueryResult;

            // 递归查询相邻格子的函数
            void QueryCell(MergeableCell queryCell)
            {
                if (queriedCells.Contains(queryCell))
                {
                    return;
                }

                queriedCells.Add(queryCell);

                // 查询上下左右四个方向的相邻格子
                QueryContinuousCell(queryCell.Left);
                QueryContinuousCell(queryCell.Right);
                QueryContinuousCell(queryCell.Above);
                QueryContinuousCell(queryCell.Below);

                // 检查相邻格子是否可合成
                void QueryContinuousCell(MergeableCell continuousCell)
                {
                    if (IsTargetCellMergeable(continuousCell))
                    {
                        if (continuousCell._placeableItem is MergeableObject mergeableObject)
                        {
                            if (!_continuousCellsQueryResult.Contains(mergeableObject))
                            {
                                _continuousCellsQueryResult.Add(mergeableObject);
                            }

                            QueryCell(continuousCell);
                        }
                    }
                }

                bool IsTargetCellMergeable(MergeableCell cell)
                {
                    // 基础检查：格子是否有效、状态是否正确、是否有物品等
                    if (cell == null)
                    {
                        return false;
                    }

                    // 检查格子状态是否为可合成状态
                    if (cell.CellStatus != ECellStatus.Mergeable)
                    {
                        return false;
                    }

                    // 检查格子上是否有物品且物品配置有效
                    if (cell._placeableItem == null || cell._placeableItem.CfgData == null)
                    {
                        return false;
                    }

                    // 如果物品正在被拖拽中，则不参与合成
                    if (cell._placeableItem.IsDragging)
                    {
                        return false;
                    }

                    // 检查物品所属的合成链配置是否存在
                    var chainCfg = TripleMergeConfigManager.Instance.GetChainConfig(cell._placeableItem.CfgData.ChainId);
                    if (chainCfg == null)
                    {
                        return false;
                    }

                    // 如果物品是合成链中的最高级物品且不是万能卡，则不能参与合成
                    if (chainCfg.Chain[^1] == cell._placeableItem.CfgData.Id && !cell._placeableItem.IsUniversalCard)
                    {
                        return false;
                    }

                    // 检查物品ID是否与参考物品相同（只有相同物品才能合成）
                    if (cell._placeableItem.CfgData.Id != referenceItem.CfgData.Id)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        #region input listener

        public void OnMouseDown()
        {
#if UNITY_EDITOR
            OnPointerDown();
#endif
        }

        public override void OnPointerDown(int touchIndex = 0)
        {
            base.OnPointerDown(touchIndex);

            _isMouseDown = false;

            if (CommonUtils.IsTouchUGUI()) return;

            _isMouseDown = true;

            if (CellStatus == ECellStatus.Mergeable)
            {
                //todo 可以合成
            }
        }

        public void OnMouseUpAsButton()
        {
#if UNITY_EDITOR
            OnPointerClick();
#endif
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();

            if (_isMouseDown)
            {
                _isMouseDown = false;
                //todo 应该弹出状态信息
                DebugUtil.Log($"点击item {gameObject.name}");
            }
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // 设置 Gizmos 颜色为绿色
            Gizmos.color = Color.green;

            // 获取当前对象的位置
            Vector3 position = transform.position;

            // 在场景视图中绘制一个小球体标记位置
            Gizmos.DrawSphere(position, 0.1f);

            // 使用 UnityEditor.Handles 绘制文本
            Handles.color = Color.yellow;

            // 计算文本位置（稍微偏上一点）
            Vector3 textPosition = position + Vector3.up * 0.3f;
            Vector3 textPosition2 = position + Vector3.up * 0.4f;

            // 绘制坐标文本
            Handles.Label(textPosition, $"({MapCoordinate.x}, {MapCoordinate.y})");
            if (PlacedItem != null)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                style.fontSize = 12;
                style.fontStyle = FontStyle.Bold;
                Handles.Label(textPosition2, $"GUID: {PlacedItem.GUID}", style);
            }
            if (Application.isPlaying) return;

            // 绘制所属区域ID（左侧）
            if (BelongRegionId > 0)
            {
                Vector3 regionIdPosition = position + Vector3.left * 0.7f;
                GUIStyle regionStyle = new GUIStyle();
                regionStyle.fontSize = 12;
                regionStyle.fontStyle = FontStyle.Bold;
                if (BelongRegionId == 1)
                {
                    regionStyle.normal.textColor = Color.gray;
                    Handles.Label(regionIdPosition, "初始区域", regionStyle);
                }
                else
                {
                    regionStyle.normal.textColor = Color.cyan;
                    Handles.Label(regionIdPosition, $"区域: {BelongRegionId}", regionStyle);
                }
            }

            // 绘制所需净化值（右侧）
            if (RequiredPurifiedNum > 0)
            {
                Vector3 purifyValuePosition = position + Vector3.right * 0.3f;
                GUIStyle purifyStyle = new GUIStyle();
                purifyStyle.normal.textColor = Color.magenta;
                purifyStyle.fontSize = 12;
                purifyStyle.fontStyle = FontStyle.Bold;
                purifyStyle.alignment = TextAnchor.MiddleRight;
                Handles.Label(purifyValuePosition, $"净化值: {RequiredPurifiedNum}", purifyStyle);
            }

            // 绘制净化优先级（下方）
            if (PurifiedPriority > 0)
            {
                Vector3 priorityPosition = position + Vector3.down * 0.3f;
                GUIStyle priorityStyle = new GUIStyle();
                priorityStyle.normal.textColor = Color.green;
                priorityStyle.alignment = TextAnchor.MiddleCenter;
                priorityStyle.fontSize = 12;
                priorityStyle.fontStyle = FontStyle.Bold;
                Handles.Label(priorityPosition, $"优先级: {PurifiedPriority}", priorityStyle);
            }
        }
#endif
    }
}