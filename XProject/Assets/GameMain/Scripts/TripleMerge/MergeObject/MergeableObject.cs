
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
            throw new System.NotImplementedException();
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
        }

        protected override void OnLongPressTrigger()
        {
            // todo 长按后弹出提示
            // var popUp = ThreeMergeSystem.Instance.Gameplay.MapManager.CellLongPressedPopup;
            // popUp.ShowWithTargetCell(HostCell);
        }
    }
}