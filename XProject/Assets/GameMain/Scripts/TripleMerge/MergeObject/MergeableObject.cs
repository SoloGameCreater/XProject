
using System.Collections.Generic;
using Config.TripleMerge;
using DG.Tweening;
using UnityEngine;
using Framework;

namespace TripleMerge
{
    public class MergeableObject : OnCellObject
    {
        private Rigidbody2D _rigidbody;
        private GameObject _draggingTips;
        public SpriteRenderer GrayItemRenderer { get; protected set; }

        public SpriteRenderer ItemRenderer { get; protected set; }

        public bool IsMerging { get; set; }

        public bool IsGray { get; set; }
        public override bool IsUniversalCard
        {
            get
            {
                if (CfgData != null)
                {
                    if (Utils.ParseTripleMergeItemType(CfgData.ItemType) == TripleMergeItemType.UniversalCard)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        private bool _isMergeTipping;
        private IReadOnlyList<MergeableObject> _mergeTippingItems;
        protected override void OnSelect()
        {
            _draggingTips.SetActive(true);
        }

        protected override void OnDeselect()
        {
            _draggingTips.SetActive(false);
        }

        protected override void OnRecycle()
        {
            transform.localScale = Vector3.one;

            _mergeTippingItems = null;
            _isMergeTipping = false;
        }

        protected override void OnCfgDataUpdated()
        {
            if (CfgData == null)
            {
                return;
            }

            ItemRenderer.sprite = GrayItemRenderer.sprite = ResourcesManager.Instance.GetSpriteVariant(Const.AtlasName, CfgData.Icon);

            if (!string.IsNullOrEmpty(CfgData.SpinePrefab))
            {
                ItemRenderer.gameObject.SetActive(false);
                GrayItemRenderer.gameObject.SetActive(false);
            }
        }

        protected override void OnClicked()
        {
            Select();
        }

        protected override void OnDragBegin()
        {
            TargetCollider.gameObject.SetActive(false);
            TargetCollider.gameObject.SetActive(true);

            _rigidbody.WakeUp();

            _draggingTips.SetActive(true);

            ItemRenderer.sortingOrder = 10;
        }

        protected override void OnDragEnd()
        {
            StopMergeTipping();

            _rigidbody.Sleep();

            _draggingTips.SetActive(false);

            ItemRenderer.sortingOrder = 0;
            
            OnBelongCellUpdated();
        }
        protected override void OnTouchCellUpdateBefore()
        {
            StopMergeTipping();
        }

        protected override void OnTouchCellUpdated()
        {
            TryShowMergeTip();
        }
        protected override void OnInitialize()
        {
            _rigidbody = transform.GetComponent<Rigidbody2D>();

            ItemRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();

            GrayItemRenderer = transform.Find("IconGray").GetComponent<SpriteRenderer>();
            GrayItemRenderer.sortingOrder = ItemRenderer.sortingOrder;
            var itemPosition = ItemRenderer.transform.position;
            itemPosition.z -= 0.1f;
            GrayItemRenderer.transform.position = itemPosition;
            
            _draggingTips = transform.Find("DraggingTips").gameObject;
            if (_draggingTips != null)
            {
                _draggingTips.SetActive(false);
            }

        }

        protected override void OnLongPressTrigger()
        {
            // todo 长按后弹出提示
            // var popUp = ThreeMergeSystem.Instance.Gameplay.MapManager.CellLongPressedPopup;
            // popUp.ShowWithTargetCell(HostCell);
        }
        // 到达合成链最高等级播放动画
        public override void PlayMaxTipAnim()
        {
            Debug.Log("已达到最高等级");
        }
        private void StopMergeTipping()
        {
            if (!_isMergeTipping)
            {
                return;
            }

            _isMergeTipping = false;

            if (_mergeTippingItems == null)
            {
                return;
            }

            foreach (var mergeTippingItem in _mergeTippingItems)
            {
                if (mergeTippingItem == this || mergeTippingItem == null || mergeTippingItem.gameObject == null || !mergeTippingItem.gameObject.activeSelf)
                {
                    continue;
                }

                mergeTippingItem.transform.DOKill();
                mergeTippingItem.transform.DOLocalMove(new Vector3(0, 0, mergeTippingItem.transform.localPosition.z), 0.125f);
            }

            _mergeTippingItems = null;
        }
        private void TryShowMergeTip()
        {
            if (!IsDragging)
            {
                return;
            }

            if (_isMergeTipping)
            {
                return;
            }

            var chainCfg = TripleMergeConfigManager.Instance.GetChainConfig(CfgData.ChainId);
            if (chainCfg != null && chainCfg.Chain.IndexOf(CfgData.Id) == chainCfg.Chain.Count - 1 && !IsUniversalCard)
            {
                Debug.Log($"最高级别物品，不能再合成了!");
                return;
            }

            if (TouchedCell == null)
            {
                return;
            }

            _isMergeTipping = true;

            if (TouchedCell.CellStatus != MergeableCell.ECellStatus.Mergeable
             || TouchedCell.PlacedItem == null 
             || (TouchedCell.PlacedItem.CfgData.Id != CfgData.Id && !IsUniversalCard))
            {
                return;
            }

            _mergeTippingItems = TouchedCell.GetContinuousSameItems(this);

            if (_mergeTippingItems.Count < 3)
            {
                _mergeTippingItems = null;
                return;
            }

            Vector2 cellPos = TouchedCell.transform.position;
            foreach (var mergeableObject in _mergeTippingItems)
            {
                if (mergeableObject == this)
                {
                    continue;
                }

                if (mergeableObject.BelongCell == TouchedCell)
                {
                    mergeableObject.transform.DOLocalMove(new Vector3(0, 0, mergeableObject.transform.localPosition.z), 0.2f);
                    continue;
                }

                var itemPos = mergeableObject.transform.position;
                var dir = cellPos - (Vector2) itemPos;

                mergeableObject.transform.DOMove(itemPos + (Vector3) dir.normalized * 0.4f, 0.35f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}