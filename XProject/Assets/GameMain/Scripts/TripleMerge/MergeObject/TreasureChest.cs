using UnityEngine;

namespace TripleMerge
{
    public class TreasureChest : OnCellObject
    {
        public const int TreasureChestID = 99999; 
        protected override void OnInitialize()
        {
            throw new System.NotImplementedException();
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

        protected override void OnCfgDataUpdated()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnClicked()
        {
            OpenChest();
        }

        protected override void OnDragBegin()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDragEnd()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnLongPressTrigger()
        {
            throw new System.NotImplementedException();
        }

        public void OpenChest()
        {
            Debug.Log("OpenChest!!");
        }
    }
}