using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using TripleMerge;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace TripleMerge
{
    // 合成地块
    [RequireComponent(typeof(PolygonCollider2D))]
    [Obfuscation]
    public partial class MergeableCell : ItemBase
    {
        public enum ECellStatus
        {
            Unset,//未设置
            Mergeable,//可合成
            UnPurified,//未净化
            Locked,//未解锁
        }

        public enum EVertexType
        {
            LeftTop,
            LeftBottom,
            RightBottom,
            RightTop,
        }
        //地块状态
        public ECellStatus CellStatus;

        /// <summary>
        /// 所属三合区域
        /// </summary>
        //public MergeableRegion HostRegion { private set; get; }

        private OnCellObject _placeableItem;

        /// <summary>
        /// 当前地块上放置的三合物品
        /// </summary>
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
        
        public void Initialize(/*MergeableRegion hostRegion*/)
        {
            //HostRegion = hostRegion;

            _cellCollider = transform.GetComponent<PolygonCollider2D>();

            InteractiveTrigger = transform.Find("Trigger").GetComponent<BoxCollider2D>();

            PlaceItemRoot = new GameObject("PlaceItemRoot").transform;
            PlaceItemRoot.SetParent(transform);
            PlaceItemRoot.localPosition = Vector3.zero;
        }

        public void LoadData()
        {
            _isInitialized = false;
            
            //var isNeverStorageBefore = _storageData.State == (int)ECellStatus.Unset;
            var isNeverStorageBefore = true;
            InitCellStatus(isNeverStorageBefore);
            InitPlacedItem(isNeverStorageBefore);

            _isInitialized = true;
        }

        /// <summary>
        /// 初始化地块
        /// </summary>
        /// <param name="isNeverStorageBefore"></param>
        private void InitCellStatus(bool isNeverStorageBefore)
        {
            
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
                DoMerge(ref continuousCell, isInvokeByElf, (finalMergeItemId, mergeResult) =>
                {
                    Debug.Log($"三合完成");
                }, skipMergeProgress);

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