using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaveFile.TripleMerge
{
    [System.Serializable]
    public class SaveFileTripleMerge : SaveFileBase
    {
        // 当前开启的区域id列表
        [JsonProperty] List<int> openRegionIds = new List<int>();

        [JsonIgnore]
        public List<int> OpenRegionIds
        {
            get { return openRegionIds; }
            set
            {
                if (openRegionIds != value)
                {
                    openRegionIds = value;
                }
            }
        }
    }
}