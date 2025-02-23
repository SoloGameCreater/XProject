
using Config.TripleMerge;
using UnityEngine;

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
        }

        protected override void OnCfgDataUpdated()
        {
            Debug.LogWarning("配置更新");
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
            //StopMergeTipping();

            _rigidbody.Sleep();

            _draggingTips.SetActive(false);

            ItemRenderer.sortingOrder = 0;
            
            OnBelongCellUpdated();
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
    }
}