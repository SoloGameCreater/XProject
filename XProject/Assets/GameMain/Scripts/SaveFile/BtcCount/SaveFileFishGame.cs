using System;
using Newtonsoft.Json;

namespace SaveFile
{
    [Serializable]
    public class SaveFileFishBagEntry
    {
        public int FishId { get; set; }
        public string Species { get; set; }
        public string Rarity { get; set; }
        public float Weight { get; set; }
        public int SellPrice { get; set; }
    }

    [Serializable]
    public class SaveFileFishGame : SaveFileBase
    {
        [JsonProperty] private bool _isInitialized;
        [JsonProperty] private int _rodId;
        [JsonProperty] private int _equippedBaitId;
        [JsonProperty] private SaveFileDictionary<int, SaveFileSafeCount> _ownedBaits = new(true);
        [JsonProperty] private SaveFileDictionary<int, int> _unlockedLures = new();
        [JsonProperty] private SaveFileDictionary<int, int> _ownedRods = new();
        [JsonProperty] private SaveFileList<SaveFileFishBagEntry> _fishBag = new();
        [JsonProperty] private int _totalCaughtCount;
        [JsonProperty] private int _totalSoldCount;

        [JsonIgnore]
        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                if (_isInitialized != value)
                {
                    _isInitialized = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }

        [JsonIgnore]
        public int RodId
        {
            get => _rodId;
            set
            {
                if (_rodId != value)
                {
                    _rodId = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }

        [JsonIgnore]
        public int EquippedBaitId
        {
            get => _equippedBaitId;
            set
            {
                if (_equippedBaitId != value)
                {
                    _equippedBaitId = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }

        [JsonIgnore]
        public SaveFileDictionary<int, SaveFileSafeCount> OwnedBaits => _ownedBaits;

        [JsonIgnore]
        public SaveFileDictionary<int, int> UnlockedLures => _unlockedLures;

        [JsonIgnore]
        public SaveFileDictionary<int, int> OwnedRods => _ownedRods;

        [JsonIgnore]
        public SaveFileList<SaveFileFishBagEntry> FishBag => _fishBag;

        [JsonIgnore]
        public int TotalCaughtCount
        {
            get => _totalCaughtCount;
            set
            {
                if (_totalCaughtCount != value)
                {
                    _totalCaughtCount = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }

        [JsonIgnore]
        public int TotalSoldCount
        {
            get => _totalSoldCount;
            set
            {
                if (_totalSoldCount != value)
                {
                    _totalSoldCount = value;
                    SaveFileManager.Instance.LocalVersion++;
                }
            }
        }
    }
}
