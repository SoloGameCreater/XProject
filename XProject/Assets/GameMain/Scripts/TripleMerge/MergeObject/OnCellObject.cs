using Config.TripleMerge;
using SaveFile.TripleMerge;
using Sirenix.OdinInspector;
using TripleMerge;

namespace TripleMerge
{
    public abstract class OnCellObject : ItemBase
    {
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
                // todo 
                //OnBelongCellUpdated();
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
        public void Active()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Recycle()
        {
            throw new System.NotImplementedException();
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

        protected abstract void OnCfgDataUpdated();
    }
}