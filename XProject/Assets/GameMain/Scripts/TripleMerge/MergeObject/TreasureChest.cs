using UnityEngine;

namespace TripleMerge
{
    public class TreasureChest : OnCellObject
    {
        public const int TreasureChestID = 99999; 
        protected SpriteRenderer IconSp { get; set; }
        protected override void OnInitialize()
        {
            IconSp = transform.Find("Icon").GetComponent<SpriteRenderer>();
        }

        protected override void OnRecycle()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSelect()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDeselect()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
        protected override void OnBelongCellUpdated()
        {
            base.OnBelongCellUpdated();

            if (BelongCell != null)
            {
                IconSp.sortingOrder = -BelongCell.MapCoordinate.y;
            }
        }
        protected override void OnLongPressTrigger()
        {
            throw new System.NotImplementedException();
        }

        public void OpenChest()
        {
            Debug.Log("OpenChest!!");
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