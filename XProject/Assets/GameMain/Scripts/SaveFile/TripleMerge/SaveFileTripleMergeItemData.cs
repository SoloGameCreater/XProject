using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaveFile.TripleMerge
{
    public class SaveFileTripleMergeItemData : SaveFileBase
    {
        [JsonProperty]
        int itemId;
        [JsonIgnore]
        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                if(itemId != value)
                {
                    itemId = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }
    }
}