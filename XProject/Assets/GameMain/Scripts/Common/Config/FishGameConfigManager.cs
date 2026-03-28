using System;
using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using UnityEngine;

namespace Config.FishGame
{
    public enum FishBaitType
    {
        Consumable = 1,
        Lure = 2
    }

    public static class FishBaitTypeMask
    {
        public const int Consumable = 1 << 0;
        public const int Lure = 1 << 1;

        public static bool Supports(int mask, FishBaitType baitType)
        {
            var bit = baitType == FishBaitType.Lure ? Lure : Consumable;
            return (mask & bit) != 0;
        }
    }

    public class FishGlobalConfig
    {
        public int Id { get; set; }
        public float MinCastDistance { get; set; }
        public float NoBiteDistance { get; set; }
        public float MaxCastDistance { get; set; }
        public float FightFailDistance { get; set; }
        public float FightLineClampDistance { get; set; }
        public float AutoRetrieveSpeed { get; set; }
        public float WaitTimeMin { get; set; }
        public float WaitTimeMax { get; set; }
        public float EmptyHookCooldown { get; set; }
        public float CatchLineDistance { get; set; }
        public int StarterRodLevel { get; set; }
        public int StarterBaitId { get; set; }
        public int StarterBaitCount { get; set; }
        public int BaitPackCount { get; set; }
        public float OverLevelWeightScale { get; set; }
        public float ReelDrainBonus { get; set; }
        public float LockDrainBonus { get; set; }
        public float StruggleReleaseDrainScale { get; set; }
        public float ControlSuppressionDivisor { get; set; }
        public float SprintForceMul { get; set; }
        public float RestForceMul { get; set; }
    }

    public class FishRodLevelConfig
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public int UpgradeCostCoin { get; set; }
        public float LineStrength { get; set; }
        public float ReelSpeed { get; set; }
        public float ControlPower { get; set; }
        public float EscapeMitigation { get; set; }
        public float LockLineStaminaDamagePerSec { get; set; }
        public float LockLineTensionGainPerSec { get; set; }
        public int SupportedBaitTypeMask { get; set; }
        public int SupportedBaitQualityMax { get; set; }
        public int RecommendFishLevelMax { get; set; }
    }

    public class FishBaitConfig
    {
        public int BaitId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Quality { get; set; }
        public int BuyPriceCoin { get; set; }
        public int ConsumePerCast { get; set; }
        public string[] TargetTags { get; set; }
        public float HookWeightBonus { get; set; }
        public float HighLevelWeightBonus { get; set; }
        public float WaitTimeMultiplier { get; set; }
        public int UnlockRodLevel { get; set; }
        public int EnabledPhase { get; set; }
        public bool CanPurchase { get; set; }

        [JsonIgnore]
        public FishBaitType BaitType => (FishBaitType)Type;
    }

    public class FishSpeciesConfig
    {
        public int FishId { get; set; }
        public string Species { get; set; }
        public int Level { get; set; }
        public string Rarity { get; set; }
        public float WeightMin { get; set; }
        public float WeightMax { get; set; }
        public float BaseStamina { get; set; }
        public float Resistance { get; set; }
        public float EscapeSpeed { get; set; }
        public int BasePricePerKg { get; set; }
        public float StruggleDurationMin { get; set; }
        public float StruggleDurationMax { get; set; }
        public float RestDurationMin { get; set; }
        public float RestDurationMax { get; set; }
        public float SprintChance { get; set; }
        public float BaseDrainPerSec { get; set; }
        public float RecoveryPerSec { get; set; }
        public string[] PreferredBaitTags { get; set; }
        public float CatchWeight { get; set; }
        public int RecommendRodLevel { get; set; }
    }

    public class FishDistanceTierConfig
    {
        public int TierId { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
        public bool CanBite { get; set; }
        public float FishLevelWeightBonus { get; set; }
        public float RarityWeightCommon { get; set; }
        public float RarityWeightUncommon { get; set; }
        public float RarityWeightRare { get; set; }
        public float RarityWeightLegendary { get; set; }
        public float WaitTimeMultiplier { get; set; }
    }

    public sealed class FishGameConfigManager : GlobalSystem<FishGameConfigManager>
    {
        public const int CurrentEnabledPhase = 1;

        private readonly Dictionary<Type, string> _typeToSubModule = new()
        {
            [typeof(FishGlobalConfig)] = "fishglobal",
            [typeof(FishRodLevelConfig)] = "fishrodlevel",
            [typeof(FishBaitConfig)] = "fishbait",
            [typeof(FishSpeciesConfig)] = "fishspecies",
            [typeof(FishDistanceTierConfig)] = "fishdistancetier"
        };

        private List<FishGlobalConfig> _fishGlobalList;
        private List<FishRodLevelConfig> _fishRodLevelList;
        private List<FishBaitConfig> _fishBaitList;
        private List<FishSpeciesConfig> _fishSpeciesList;
        private List<FishDistanceTierConfig> _fishDistanceTierList;

        private readonly Dictionary<int, FishRodLevelConfig> _rodCache = new();
        private readonly Dictionary<int, FishBaitConfig> _baitCache = new();
        private readonly Dictionary<int, FishSpeciesConfig> _speciesCache = new();
        private readonly List<FishBaitConfig> _enabledBaitsCache = new();
        private bool _enabledBaitsBuilt;

        public FishGlobalConfig GlobalConfig => GetSingleConfig<FishGlobalConfig>();
        public List<FishRodLevelConfig> RodLevelConfigs => GetConfig<FishRodLevelConfig>();
        public List<FishBaitConfig> BaitConfigs => GetConfig<FishBaitConfig>();
        public List<FishSpeciesConfig> SpeciesConfigs => GetConfig<FishSpeciesConfig>();
        public List<FishDistanceTierConfig> DistanceTierConfigs => GetConfig<FishDistanceTierConfig>();

        public FishRodLevelConfig GetRodLevelConfig(int level)
        {
            if (TryGetRodLevelConfig(level, out var config))
            {
                return config;
            }

            FishRodLevelConfig fallback = null;
            var configs = RodLevelConfigs;
            for (var i = 0; i < configs.Count; i++)
            {
                if (configs[i].Level <= level && (fallback == null || configs[i].Level > fallback.Level))
                {
                    fallback = configs[i];
                }
            }

            return fallback ?? (configs.Count > 0 ? configs[0] : null);
        }

        public bool TryGetRodLevelConfig(int level, out FishRodLevelConfig config)
        {
            if (_rodCache.TryGetValue(level, out config))
            {
                return config != null;
            }

            config = RodLevelConfigs.Find(item => item.Level == level);
            _rodCache[level] = config;
            return config != null;
        }

        public FishRodLevelConfig GetNextRodLevelConfig(int currentLevel)
        {
            FishRodLevelConfig candidate = null;
            var configs = RodLevelConfigs;
            for (var i = 0; i < configs.Count; i++)
            {
                if (configs[i].Level > currentLevel && (candidate == null || configs[i].Level < candidate.Level))
                {
                    candidate = configs[i];
                }
            }

            return candidate;
        }

        public bool TryGetBaitConfig(int baitId, out FishBaitConfig config)
        {
            if (_baitCache.TryGetValue(baitId, out config))
            {
                return config != null;
            }

            config = BaitConfigs.Find(item => item.BaitId == baitId);
            _baitCache[baitId] = config;
            return config != null;
        }

        public bool TryGetSpeciesConfig(int fishId, out FishSpeciesConfig config)
        {
            if (_speciesCache.TryGetValue(fishId, out config))
            {
                return config != null;
            }

            config = SpeciesConfigs.Find(item => item.FishId == fishId);
            _speciesCache[fishId] = config;
            return config != null;
        }

        public FishDistanceTierConfig GetDistanceTier(float distance)
        {
            var configs = DistanceTierConfigs;
            for (var i = 0; i < configs.Count; i++)
            {
                if (distance >= configs[i].MinDistance && distance <= configs[i].MaxDistance)
                {
                    return configs[i];
                }
            }

            if (configs.Count == 0)
            {
                return null;
            }

            if (distance < configs[0].MinDistance)
            {
                return configs[0];
            }

            return configs[configs.Count - 1];
        }

        public List<FishBaitConfig> GetEnabledBaits()
        {
            if (_enabledBaitsBuilt)
            {
                return _enabledBaitsCache;
            }

            _enabledBaitsCache.Clear();
            var configs = BaitConfigs;
            for (var i = 0; i < configs.Count; i++)
            {
                if (configs[i].EnabledPhase <= CurrentEnabledPhase)
                {
                    _enabledBaitsCache.Add(configs[i]);
                }
            }

            _enabledBaitsBuilt = true;
            return _enabledBaitsCache;
        }

        public float GetRarityWeight(FishDistanceTierConfig tier, string rarity)
        {
            if (tier == null)
            {
                return 1f;
            }

            return rarity switch
            {
                "legendary" => tier.RarityWeightLegendary,
                "rare" => tier.RarityWeightRare,
                "uncommon" => tier.RarityWeightUncommon,
                _ => tier.RarityWeightCommon
            };
        }

        private T GetSingleConfig<T>() where T : class
        {
            var list = GetConfig<T>();
            return list.Count > 0 ? list[0] : null;
        }

        private List<T> GetConfig<T>() where T : class
        {
            var subModule = _typeToSubModule[typeof(T)];
            TryLoad(subModule);

            return subModule switch
            {
                "fishglobal" => _fishGlobalList as List<T>,
                "fishrodlevel" => _fishRodLevelList as List<T>,
                "fishbait" => _fishBaitList as List<T>,
                "fishspecies" => _fishSpeciesList as List<T>,
                "fishdistancetier" => _fishDistanceTierList as List<T>,
                _ => throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null)
            };
        }

        private void TryLoad(string subModule)
        {
            switch (subModule)
            {
                case "fishglobal":
                    if (_fishGlobalList != null)
                    {
                        return;
                    }
                    break;
                case "fishrodlevel":
                    if (_fishRodLevelList != null)
                    {
                        return;
                    }
                    break;
                case "fishbait":
                    if (_fishBaitList != null)
                    {
                        return;
                    }
                    break;
                case "fishspecies":
                    if (_fishSpeciesList != null)
                    {
                        return;
                    }
                    break;
                case "fishdistancetier":
                    if (_fishDistanceTierList != null)
                    {
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }

            var path = $"Configs/DataJson/FishGame/{subModule}";
            var textAsset = ResourcesManager.Instance.LoadResource<TextAsset>(path);
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                DebugUtil.LogError($"Load {path} error!");
                return;
            }

            switch (subModule)
            {
                case "fishglobal":
                    _fishGlobalList = JsonConvert.DeserializeObject<List<FishGlobalConfig>>(textAsset.text);
                    break;
                case "fishrodlevel":
                    _fishRodLevelList = JsonConvert.DeserializeObject<List<FishRodLevelConfig>>(textAsset.text);
                    break;
                case "fishbait":
                    _fishBaitList = JsonConvert.DeserializeObject<List<FishBaitConfig>>(textAsset.text);
                    break;
                case "fishspecies":
                    _fishSpeciesList = JsonConvert.DeserializeObject<List<FishSpeciesConfig>>(textAsset.text);
                    break;
                case "fishdistancetier":
                    _fishDistanceTierList = JsonConvert.DeserializeObject<List<FishDistanceTierConfig>>(textAsset.text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
        }
    }
}
