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

        // 地图上的地块字典
        [JsonProperty] SaveFileDictionary<string, SaveFileTripleMergeCellData> cells = new SaveFileDictionary<string, SaveFileTripleMergeCellData>();

        [JsonIgnore]
        public SaveFileDictionary<string, SaveFileTripleMergeCellData> Cells
        {
            get { return cells; }
        }
    }
}