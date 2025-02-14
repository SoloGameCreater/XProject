using System;
using Newtonsoft.Json;

namespace SaveFile
{
    [Serializable]
    public class SaveFileCurrency : SaveFileBase
    {
        // 玩家拥有的货币集合
        [JsonProperty] SaveFileDictionary<int, SaveFileSafeCount> userAllCurrencyDic = new(true);

        [JsonIgnore]
        public SaveFileDictionary<int, SaveFileSafeCount> UserAllCurrencyDic
        {
            get { return userAllCurrencyDic; }
        }
    }
}