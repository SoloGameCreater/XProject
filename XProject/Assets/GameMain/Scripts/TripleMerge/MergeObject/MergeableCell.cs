#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config.TripleMerge;
using SaveFile.TripleMerge;
using UnityEngine;
using UnityEngine.Pool;

namespace TripleMerge
{
    // 合成地块
    [RequireComponent(typeof(PolygonCollider2D))]
    [Obfuscation]
    public partial class MergeableCell : ItemBase
    {
#if UNITY_EDITOR
        [LabelText("所属地段ID")] [ValueDropdown(nameof(GetValidRegions))]
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
        [LabelText("初始默认放置合成物品")] [ValueDropdown(nameof(GetMergeableItemConfigList))]
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
        //[ShowInInspector]
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
        private string SaveKey => $"{MapCoordinate.x}_{MapCoordinate.y}";

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
                //第一次取预制体上的存储数据
                // if ()
                // {
                //     CellStatus = ECellStatus.Locked;
                // }
                // else
                // {
                //     //读取初始配置信息，如果初始配置时，该地块需要一定数量的净化值，则代表其初始状态是未净化状态
                //     CellStatus = RequiredPurifiedNum > 0 ? ECellStatus.UnPurified : ECellStatus.Mergeable;
                // }

                TripleMergeSystem.Instance.Model.SetCellState(_saveData, (int)CellStatus);
            }
            else
            {
                //首先从存档数据中读取状态
                CellStatus = (ECellStatus)_saveData.State;

                if (CellStatus == ECellStatus.Locked) //如果时锁定的地块，从新检查路段解锁状态，防止某些情况路段解锁了之后地块信息没正常更新
                {
                    // if (BelongRegion.BelongArea.AreaRegionDictionary.TryGetValue(HostRegionId, out var belongToAreaRegion) && belongToAreaRegion.IsUnlock())
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

        public void PlaceItem(OnCellObject item, Action onPlacedAction = null, Action onMergedAction = null)
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

            // 放置合成物品
            var itemTrans = item.transform;
            itemTrans.SetParent(PlaceItemRoot);
            // todo 后期会区分是否播放位移动画，现在直接放置到对应位置
            itemTrans.localPosition = Vector3.zero;

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
                if (TryToMerge(item, (finalItemId) =>
                {
                    Debug.Log($"[MERGE END] caller:{callerItemId} . final: {finalItemId}");
                }))
                {
                    onMergedAction?.Invoke();
                    return;
                }
            }

            SetPlacedItem(item);
            onMergedAction?.Invoke();
        }

        private void SetPlacedItem(OnCellObject item)
        {
            var tobeReplaceItems = ListPool<OnCellObject>.Get();

            if (_placeableItem != null)
            {
                if (item != null)
                    tobeReplaceItems.Add(item);

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
            if (takeOffItem != null)
            {
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
                    FindNearestEmptyCells(1, ref nearestEmptyCells,
                        Utils.ParseTripleMergeItemType(displacedItem.CfgData.ItemType) == TripleMergeItemType.TreasureChest);

                    if (nearestEmptyCells.Count > 0)
                    {
                        nearestEmptyCells[0].PlaceItem(displacedItem);
                    }
                    else
                    {
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
        /// <param name="targetCellNum"></param>
        /// <param name="result"></param>
        /// <param name="allowQueryFromAllAreas">如果是宝箱则搜索全部区域</param>
        private void FindNearestEmptyCells(int targetCellNum, ref List<MergeableCell> result, bool allowQueryFromAllAreas = false)
        {
            result.Clear();

            var tempList = ListPool<MergeableCell>.Get();
            try
            {
                // 提前计算容量并只添加有效的格子

                // 获取目标单元格字典并预分配容量
                var targetCells = allowQueryFromAllAreas
                    ? TripleMergeSystem.Instance.Gameplay.MapManager.MapArea.MergeableCellsDictionary
                    : BelongRegion.BelongArea.MergeableCellsDictionary;

                // 健壮性检查
                if (targetCells == null)
                {
                    throw new InvalidOperationException("Target cells dictionary is null.");
                }

                // 预分配容量并填充空单元格
                tempList.Capacity = targetCells.Count;
                AddEmptyCells(targetCells.Values, tempList);

                // 添加空单元格
                void AddEmptyCells(IEnumerable<MergeableCell> cells, List<MergeableCell> targetList)
                {
                    targetList.AddRange(cells.Where(IsCellEmpty));
                }
                if (!allowQueryFromAllAreas)
                {
                    tempList.Capacity = BelongRegion.BelongArea.MergeableCellsDictionary.Count;
                    foreach (var cell in BelongRegion.BelongArea.MergeableCellsDictionary.Values)
                    {
                        if (!IsCellEmpty(cell))continue;
                        
                        tempList.Add(cell);
                    }
                }
                else
                {
                    var totalCells = TripleMergeSystem.Instance.Gameplay.MapManager.MapArea.MergeableCellsDictionary;
                    tempList.Capacity = totalCells.Count;
                    var area = TripleMergeSystem.Instance.Gameplay.MapManager.MapArea;
                    foreach (var cell in area.MergeableCellsDictionary.Values)
                    {
                        if (!IsCellEmpty(cell)) continue;
                        
                        tempList.Add(cell);
                    }
                }

                if (tempList.Count < targetCellNum)
                {
                    result.AddRange(tempList);
                    return;
                }

                QuickSelectByDistance(tempList, 0, tempList.Count - 1, targetCellNum);
                result.AddRange(tempList.Take(targetCellNum));
            }
            finally
            {
                ListPool<MergeableCell>.Release(tempList);
            }
        }

        private bool IsCellEmpty(MergeableCell cell)
        {
            return cell.PlacedItem == null &&
                   cell.CellStatus == ECellStatus.Mergeable;
        }

        // 优化5：使用快速选择算法，只排序需要的部分
        private void QuickSelectByDistance(List<MergeableCell> cells, int left, int right, int k)
        {
            while (true)
            {
                if (left >= right) return;

                int pivot = Partition(cells, left, right);

                if (pivot == k - 1) return;
                if (pivot > k - 1)
                {
                    right = pivot - 1;
                }
                else
                {
                    left = pivot + 1;
                }
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

            if (item is not MergeableObject OnCellObject)
            {
                return false;
            }

            if (item.CfgData.ChainId == 0)
            {
                return false;
            }


            var continuousCell = GetContinuousSameItems(OnCellObject);
            var continuousCnt = continuousCell.Count;
            if (continuousCnt >= 3)
            {
                Debug.Log($"三合开始");
                void OnMergeCompleted(int finalMergeItemId, Dictionary<int,int> mergeResult)
                {
                    Debug.Log($"三合完成");
                    
                    // 处理合成后的逻辑
                    StartCoroutine(DelayedKeepMerge(finalMergeItemId));
                }
                
                // 延迟处理合成后的逻辑
                IEnumerator DelayedKeepMerge(int finalMergeItemId)
                {
                    yield return new WaitForSeconds(0.035f);
                    ProcessAfterMerge(finalMergeItemId);
                }
                // 处理合成后的逻辑
                void ProcessAfterMerge(int finalMergeItemId)
                {
                    TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = false;
                    Debug.Log($"set mark is merging false");
                    
                    // 如果合出的是万能卡，不允许combo合成
                    if (_placeableItem != null && _placeableItem.IsUniversalCard)
                    {
                        var chainCfg = TripleMergeConfigManager.Instance.GetChainConfig(_placeableItem.CfgData.ChainId);
                        if (chainCfg.Chain[^1] == _placeableItem.CfgData.Id)
                        {
                            _placeableItem.PlayMaxTipAnim();
                        }
                        return;
                    }
                    
                    // 尝试继续合成，如果不能继续合成则执行后续操作
                    if (!TryToMerge(null, onFinish))
                    {
                        onFinish?.Invoke(finalMergeItemId);

                        if (_placeableItem != null)
                        {
                            // 检查是否是最高级物品
                            var chainCfg = TripleMergeConfigManager.Instance.GetChainConfig(_placeableItem.CfgData.ChainId);
                            if (chainCfg.Chain[^1] == _placeableItem.CfgData.Id)
                            {
                                _placeableItem.PlayMaxTipAnim();
                            }
                        }
                    }
                }
                
                // 能够进行合成的情况，先进行合成计算，合成完毕后，再对合成完毕后的合成物进行新的位置分配
                DoMerge(ref continuousCell, OnMergeCompleted);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 执行三合一合并操作
        /// </summary>
        /// <param name="continuousItems">连续的相同物品列表</param>
        /// <param name="onMergeCompleted">合并完成后的回调</param>
        private void DoMerge(ref IReadOnlyList<MergeableObject> continuousItems, Action<int, Dictionary<int, int>> onMergeCompleted = null)
        {
            // 资源池初始化
            var afterMergeItems = ListPool<MergeableObject>.Get();
            var tobeMergeItems = ListPool<MergeableObject>.Get();
            var unlockNewItems = HashSetPool<int>.Get();
            var mergeResults = DictionaryPool<int, int>.Get();
            
            try
            {
                // 设置合并状态标记
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = true;
                Debug.Log($"设置合并状态为true");
                
                // 准备合并物品
                PrepareItemsForMerge(continuousItems, tobeMergeItems);
                
                // 执行合并计算
                int finalMergeItemId = PerformMergeCalculation(tobeMergeItems, afterMergeItems, mergeResults, unlockNewItems);
                
                // 放置合并后的物品
                PlaceMergedItems(afterMergeItems);
                
                // 调用合并完成回调
                onMergeCompleted?.Invoke(finalMergeItemId, mergeResults);
            }
            catch (Exception e)
            {
                Debug.LogError($"三合合成时发生异常:{e}");
            }
            finally
            {
                // 释放资源
                ListPool<MergeableObject>.Release(afterMergeItems);
                ListPool<MergeableObject>.Release(tobeMergeItems);
                HashSetPool<int>.Release(unlockNewItems);
                DictionaryPool<int, int>.Release(mergeResults);
                
                // 注意：这里不重置IsMerging状态，应该在连锁合并完成后重置
            }
        }

        /// <summary>
        /// 准备要合并的物品
        /// </summary>
        private void PrepareItemsForMerge(IReadOnlyList<MergeableObject> continuousItems, List<MergeableObject> tobeMergeItems)
        {
            // 将连续物品添加到待合并列表
            for (var i = 0; i < continuousItems.Count; i++)
            {
                var item = continuousItems[i];
                item.IsMerging = true;
                tobeMergeItems.Add(item);
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
        private (MergeChain chain, MergeableItemCfg baseItem) GetMergeChainAndBaseItem(List<MergeableObject> items)
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
                Debug.LogError($"三合合成物ID[{itemId}]已经是最高级别物品了");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 合并一批物品（每3个合成1个高级物品）
        /// </summary>
        /// <returns>合并出的物品ID</returns>
        private int MergeItemsBatch(List<MergeableObject> tobeMergeItems, MergeChain targetChain, 
            MergeableItemCfg mergeBaseItem, Dictionary<int, int> mergeResults, 
            HashSet<int> unlockNewItems, List<MergeableObject> afterMergeItems)
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
            
            // 回收要销毁的物品
            RecycleItemsToDestroy(tobeMergeItems, remainCurrentLevelNum);
            
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
        /// 回收要销毁的物品
        /// </summary>
        private void RecycleItemsToDestroy(List<MergeableObject> tobeMergeItems, int remainCount)
        {
            for (var i = tobeMergeItems.Count - 1; i >= remainCount; i--)
            {
                var tobeDestroyItem = tobeMergeItems[i];
                tobeDestroyItem.IsMerging = false;
                tobeMergeItems.Remove(tobeDestroyItem);
                OnCellObjectPool.Recycle(tobeDestroyItem);
            }
            
            // 清空待合并列表，保留剩余物品
            var remainItems = tobeMergeItems.Take(remainCount).ToList();
            tobeMergeItems.Clear();
            tobeMergeItems.AddRange(remainItems);
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
        private void PlaceMergedItems(List<MergeableObject> afterMergeItems)
        {
            foreach (var item in afterMergeItems)
            {
                // 查找最近的空单元格
                var nearestEmptyCells = ListPool<MergeableCell>.Get();
                FindNearestEmptyCells(1, ref nearestEmptyCells, false);
                
                if (nearestEmptyCells.Count > 0)
                {
                    // 放置物品
                    nearestEmptyCells[0].PlaceItem(item);
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
            var queriedCells = HashSetPool<MergeableCell>.Get();
            try
            {
                // 添加初始参考物品
                _continuousCellsQueryResult.Add(referenceItem);

                // 检查当前格子的物品
                CheckCurrentCellItem(referenceItem, ref queriedCells);

                // 如果当前检查未返回，继续递归检查相邻格子
                if (_continuousCellsQueryResult.Count > 0)
                {
                    QueryConnectedCells(this, ref queriedCells);
                }

                return _continuousCellsQueryResult;
            }
            finally
            {
                HashSetPool<MergeableCell>.Release(queriedCells);
            }
        }

        private void CheckCurrentCellItem(MergeableObject referenceItem, ref HashSet<MergeableCell> queriedCells)
        {
            // 如果格子为空或其他无效情况，直接返回
            if (!IsCurrentCellValid(referenceItem)) return;

            var currentItem = PlacedItem as MergeableObject;

            if (referenceItem.IsUniversalCard)
            {
                HandleUniversalMerge(ref referenceItem, currentItem, ref queriedCells);
            }
            else
            {
                HandleNormalMerge(referenceItem, currentItem);
            }
        }

        private bool IsCurrentCellValid(MergeableObject referenceItem)
        {
            return PlacedItem != null
                && PlacedItem != referenceItem
                && PlacedItem is MergeableObject;
        }

        private void HandleUniversalMerge(ref MergeableObject referenceItem, MergeableObject currentItem, ref HashSet<MergeableCell> queriedCells)
        {
            // 检查是否满足万能卡合成的限制条件
            if (currentItem.IsUniversalCard)
            {
                _continuousCellsQueryResult.Clear();
                return;
            }

            // 检查目标物品是否可以被万能卡合成
            if (Utils.ParseTripleMergeItemType(currentItem.CfgData.ItemType) == TripleMergeItemType.MergeableNormal)
            {
                queriedCells.Add(referenceItem.BelongCell);
                referenceItem = currentItem;
                _continuousCellsQueryResult.Add(currentItem);
            }
        }

        private void HandleNormalMerge(MergeableObject referenceItem, MergeableObject currentItem)
        {
            if (PlacedItem.CfgData.Id == referenceItem.CfgData.Id)
            {
                _continuousCellsQueryResult.Add(currentItem);
            }
        }

        private void QueryConnectedCells(MergeableCell cell, ref HashSet<MergeableCell> queriedCells)
        {
            if (queriedCells.Contains(cell)) return;

            queriedCells.Add(cell);

            // 检查四个方向的相邻格子
            CheckNeighborCell(cell.Left, ref queriedCells);
            CheckNeighborCell(cell.Right, ref queriedCells);
            CheckNeighborCell(cell.Above, ref queriedCells);
            CheckNeighborCell(cell.Below, ref queriedCells);
        }

        private void CheckNeighborCell(MergeableCell neighborCell, ref HashSet<MergeableCell> queriedCells)
        {
            if (neighborCell == null || !IsCellMergeable(neighborCell)) return;

            var neighborItem = neighborCell._placeableItem as MergeableObject;
            if (neighborItem != null && !_continuousCellsQueryResult.Contains(neighborItem))
            {
                _continuousCellsQueryResult.Add(neighborItem);
                QueryConnectedCells(neighborCell, ref queriedCells);
            }
        }

        private bool IsCellMergeable(MergeableCell cell)
        {
            return cell.CellStatus == ECellStatus.Mergeable
                && cell._placeableItem != null
                && !cell._placeableItem.IsDragging;
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
                // todo 点击事件
                Debug.Log($"点击item {gameObject.name}");
            }
        }

        #endregion
    }
}