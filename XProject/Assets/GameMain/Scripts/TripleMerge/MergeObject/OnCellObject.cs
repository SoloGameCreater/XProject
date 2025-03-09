using System;
using System.Collections.Generic;
using Config.TripleMerge;
using SaveFile.TripleMerge;
using Sirenix.OdinInspector;
using TripleMerge;
using UnityEngine;
using UnityEngine.Pool;

namespace TripleMerge
{
    public abstract partial class OnCellObject : ItemBase
    {
        #if UNITY_EDITOR
        public uint GUID { get; set; }
        #endif
        private MergeableCell _belongCell;
        /// <summary>
        /// 当前所属的地块
        /// </summary>
        [ShowInInspector]
        public MergeableCell BelongCell
        {
            set
            {
                _belongCell = value;
                OnBelongCellUpdated();
            }

            get => _belongCell;
        }
        /// <summary>
        /// 绑定的配置数据
        /// </summary>
        public MergeableItemCfg CfgData { private set; get; }
        /// <summary>
        /// 存储model
        /// </summary>
        public SaveFileTripleMergeItemData ItemSaveData { private set; get; }
        /// <summary>
        /// 触发器
        /// </summary>
        protected PolygonCollider2D TargetCollider;
        protected Transform TriggerPivot;
        /// <summary>
        /// 是否处于选中状态
        /// </summary>
        public bool IsSelecting { get; set; }
        /// <summary>
        /// 是否是万能卡
        /// </summary>
        public virtual bool IsUniversalCard => false;
        
        [ShowInInspector] protected MergeableCell TouchedCell;
        [ShowInInspector] private readonly Dictionary<Collider2D, MergeableCell> _triggeredCells = new();

        #region Abstract Function
        protected abstract void OnInitialize();
        protected abstract void OnRecycle();
        protected abstract void OnSelect();
        protected abstract void OnDeselect();
        protected abstract void OnCfgDataUpdated();
        protected abstract void OnClicked();
        protected abstract void OnDragBegin();
        protected abstract void OnDragEnd();
        protected abstract void OnLongPressTrigger();
        #endregion
        protected virtual void OnBelongCellUpdated()
        {
            
        }

        public virtual void PlayMaxTipAnim()
        {
        }

        public void Active()
        {
            _currentInputStage = EInputStage.None;
        }

        public void Initialize()
        {
            TargetCollider = transform.Find("Trigger").GetComponent<PolygonCollider2D>();
            var localPos = TargetCollider.transform.localPosition;
            localPos.z = -50f;
            TargetCollider.transform.localPosition = localPos;
            TriggerPivot = transform.Find("Trigger/Pivot").transform;
            
            OnInitialize();
        }
        public void Recycle()
        {
            OnRecycle();
        }

        public void BindItemSaveData(SaveFileTripleMergeItemData setCellPlaceItem)
        {
            ItemSaveData = setCellPlaceItem;
        }
        public void SetCfgData(MergeableItemCfg cfgData)
        {
            CfgData = cfgData;

            OnCfgDataUpdated();
        }

        protected void Select()
        {
            if (IsSelecting)
            {
                return;
            }
            if(TripleMergeSystem.Instance.Gameplay.MapManager.SelectedCellItem != null)
                TripleMergeSystem.Instance.Gameplay.MapManager.SelectedCellItem.Deselect();

            OnSelect();

            TripleMergeSystem.Instance.Gameplay.MapManager.SelectCellItemChanged(this);

            IsSelecting = true;
        }

        protected void Deselect()
        {
            if (!IsSelecting)
            {
                return;
            }

            //TryCloseLongPressedPopup();

            OnDeselect();
            TripleMergeSystem.Instance.Gameplay.MapManager.SelectCellItemChanged(null);

            //IsLongPressedTriggered = false;

            IsSelecting = false;
        }
        
        public static List<MergeableCell> GetMergeableEmptyCellsByDistance(int requiredNum, Vector3 cellPosition)
        {
            var mergeableCells = ListPool<MergeableCell>.Get();
            foreach (var mergeableCell in TripleMergeSystem.Instance.Gameplay.MapManager.MapArea.MergeableCellsDictionary.Values)
            {
                if (mergeableCell.CellStatus != MergeableCell.ECellStatus.Mergeable || mergeableCell.PlacedItem != null)
                {
                    continue;
                }

                mergeableCells.Add(mergeableCell);
            }

            mergeableCells.Sort((left, right) =>
            {
                var leftPointDistance = Vector2.Distance(cellPosition, left.transform.position);
                var rightPointDistance = Vector2.Distance(cellPosition, right.transform.position);

                if (leftPointDistance < rightPointDistance)
                {
                    return -1;
                }

                return leftPointDistance > rightPointDistance ? 1 : 0;
            });

            if (mergeableCells.Count > requiredNum)
            {
                for (var i = mergeableCells.Count - 1; i >= requiredNum; i--)
                {
                    mergeableCells.Remove(mergeableCells[i]);
                }
            }

            return mergeableCells;
        }
    }
}