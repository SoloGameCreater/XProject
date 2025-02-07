#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using SaveFile.TripleMerge;
using UnityEditor;
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
#if UNITY_EDITOR

        private ValueDropdownList<int> _regionCfgIdList = new();

        private const uint MAX_REGION_COUNT = 9;
        private ValueDropdownList<int> GetValidRegions()
        {
            _regionCfgIdList.Clear();
            _regionCfgIdList.Add(new ValueDropdownItem<int>($"未配置所属地段", 0));

            for (int i = 1; i <= MAX_REGION_COUNT; i++)
            {
                _regionCfgIdList.Add(new ValueDropdownItem<int>($"地段ID:[{i}]", i));
            }

            return _regionCfgIdList;
        }

#endif
        
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
            // 如果从存档数据中读取的状态是未设置，则代表是初始状态
            if (isNeverStorageBefore)
            {
                // if (BelongRegion.BelongArea.AreaRegionDictionary.TryGetValue(HostRegionId, out var belongToAreaRegion) && !belongToAreaRegion.IsUnlock())
                // {
                //     CellStatus = ECellStatus.Locked;
                // }
                // else
                {
                    //读取初始配置信息，如果初始配置时，该地块需要一定数量的净化值，则代表其初始状态是未净化状态
                    CellStatus =  ECellStatus.Mergeable;
                }
            
                TripleMergeSystem.Instance.Model.SetCellState(_saveData, (int) CellStatus);
            }
            else
            {
                //首先从存档数据中读取状态
                CellStatus = (ECellStatus) _saveData.State;
            
                if (CellStatus == ECellStatus.Locked) //如果时锁定的地块，从新检查路段解锁状态，防止某些情况路段解锁了之后地块信息没正常更新
                {
                    // if (BelongRegion.BelongArea.AreaRegionDictionary.TryGetValue(HostRegionId, out var belongToAreaRegion) && belongToAreaRegion.IsUnlock())
                    // {
                    //     //读取初始配置信息，如果初始配置时，该地块需要一定数量的净化值，则代表其初始状态是未净化状态
                    //     CellStatus = RequiredPurifiedNum > 0 ? ECellStatus.UnPurified : ECellStatus.Mergeable;
                    // }
                }
            }
        }

        private void InitPlacedItem(bool isNeverStorageBefore)
        {
            var placedItemId = 0;
        }

        public void PlaceItem(OnCellObject item, bool invokeByElf, bool showMovement = true, float movementTimeLength = 0.15f, Action onPlacedAction = null,
                              bool skipMergeProgress = false, Action onMergedAction = null)
        {
            if (item == null)
            {
                SetPlacedItem(null, invokeByElf, false);
                // todo 数据先行
                //TripleMergeSystem.Instance.Model.SetCellPlaceItem(_storageData, 0);
                return;
            }

            // todo 并且先清除其原本的地块信息


            // todo 首先将合成物放到自身地块的位置中心
        }

        private void SetPlacedItem(OnCellObject item, bool isInvokeByElf, bool showMovement)
        {
            var tobeReplaceItems = ListPool<OnCellObject>.Get();
        }


        public bool TryToMerge(OnCellObject item = null, bool isInvokeByElf = false, bool skipMergeProgress = false, Action<int> onFinish = null)
        {
            var isCombo = false;
            if (item == null)
            {
                item = PlacedItem;
                isCombo = true;
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
                var callerItemId = PlacedItem.CfgData?.Id ?? 0;

                Debug.Log($"三合开始");
                // 能够进行合成的情况，先进行合成计算，合成完毕后，再对合成完毕后的合成物进行新的位置分配
                DoMerge(ref continuousCell, isInvokeByElf, (finalMergeItemId, mergeResult) => { Debug.Log($"三合完成"); }, skipMergeProgress);

                return true;
            }

            return false;
        }

        private void DoMerge(ref IReadOnlyList<MergeableObject> continuousItems, bool isInvokeByElf, Action<int, Dictionary<int, int>> onMergeCompleted = null,
                             bool skipMergeProgress = false)
        {
            try
            {
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = true;
                Debug.Log($"set mark is merging true");
                var finalMergeItemId = 0;
                var afterMergeItems = ListPool<MergeableObject>.Get();
                var tobeMergeItems = ListPool<MergeableObject>.Get();
                var unlockNewItems = HashSetPool<int>.Get();
                var mergeResults = DictionaryPool<int, int>.Get();

                var cellPosition = transform.position;

                const float beforeMergePerformDuration = 0.15f;
                const float afterMergePerformDuration = 0.15f;
                // 因为合成后会重新分配合成物的位置，所以预先将这些格子本身的放置物信息清除.并且将合成物提取到参与合成计算的mergeableObjects列表中
                for (var i = 0; i < continuousItems.Count; i++)
                {
                    var item = continuousItems[i];
                    item.IsMerging = true;
                    tobeMergeItems.Add(item);
                }

                if (skipMergeProgress)
                {
                    CalculateMerge();
                }

                void CalculateMerge()
                {
                    // todo 开始计算合成
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"三合合成时发生异常:{e}");
                TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging = false;
            }
        }

        /// <summary>
        /// 以指定三合地块为中心锚点，查询与其连续的并且放置了指定合成物的地块列表
        /// </summary>
        public IReadOnlyList<MergeableObject> GetContinuousSameItems(MergeableObject referenceItem)
        {
            return null;
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