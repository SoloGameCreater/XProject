using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaveFile.TripleMerge
{
    [System.Serializable]
    public class SaveFileTripleMerge : SaveFileBase
    {
        // 当前开启的区域id列表
        [JsonProperty] SaveFileList<int> openRegionIds = new SaveFileList<int>();

        [JsonIgnore]
        public SaveFileList<int> OpenRegionIds
        {
            get { return openRegionIds; }
            set
            {
                if (openRegionIds != value)
                {
                    openRegionIds = value;
                    SaveFileManager.Instance.LocalVersion++;
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
        // 当前打开宝箱次数
        [JsonProperty] int openChestTimes;

        [JsonIgnore]
        public int OpenChestTimes
        {
            get { return openChestTimes; }
            set
            {
                if (openChestTimes != value)
                {
                    openChestTimes = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }
        // 已经解锁的三合合成物品
        [JsonProperty]
        SaveFileDictionary<int,int> unlockedMergeableItems = new SaveFileDictionary<int,int>();
        [JsonIgnore]
        public SaveFileDictionary<int,int> UnlockedMergeableItems
        {
            get
            {
                return unlockedMergeableItems;
            }
        }
    }
}