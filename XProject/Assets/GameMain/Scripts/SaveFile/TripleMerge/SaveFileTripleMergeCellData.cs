using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaveFile.TripleMerge
{
    public class SaveFileTripleMergeCellData : SaveFileBase
    {
        // 地块当前的状态
        [JsonProperty] int state;

        [JsonIgnore]
        public int State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }

        // 地块当前放置的合成物品ID
        [JsonProperty] int placedItemId;

        [JsonIgnore]
        public int PlacedItemId
        {
            get => placedItemId;
            set
            {
                if (placedItemId != value)
                {
                    placedItemId = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }
        // 地块当前放置的三合物品
        [JsonProperty]
        SaveFileTripleMergeItemData placedItem = new SaveFileTripleMergeItemData();
        [JsonIgnore]
        public SaveFileTripleMergeItemData PlacedItem
        {
            get
            {
                return placedItem;
            }
        }
        // 地块当前的净化值
        [JsonProperty]
        int purificationValue;
        [JsonIgnore]
        public int PurificationValue
        {
            get
            {
                return purificationValue;
            }
            set
            {
                if(purificationValue != value)
                {
                    purificationValue = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }
    }
}