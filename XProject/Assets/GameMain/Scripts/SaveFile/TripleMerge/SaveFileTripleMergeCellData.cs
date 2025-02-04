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
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                }
            }
        }

        // 地块当前放置的合成物品ID
        [JsonProperty] int placedItemId;

        [JsonIgnore]
        public int PlacedItemId
        {
            get { return placedItemId; }
            set
            {
                if (placedItemId != value)
                {
                    placedItemId = value;
                }
            }
        }
    }
}