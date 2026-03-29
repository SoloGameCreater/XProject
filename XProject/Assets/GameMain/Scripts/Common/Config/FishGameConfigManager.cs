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
        public int StarterRodId { get; set; }
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

        // Cast
        public float MaxCastChargeDuration { get; set; } = 1.5f;
        public float CastingDelay { get; set; } = 0.8f;

        // Fight - Tension & Reel
        public float TensionRelaxDuration { get; set; } = 0.5f;
        public float ReelTensionSpeedMul { get; set; } = 0.75f;
        public float MinimumReelSpeed { get; set; } = 0.2f;
        public float NormalCatchWindowOffset { get; set; } = 1.5f;
        public float FatigueCatchWindowOffset { get; set; } = 1.85f;

        // Fight - Burst System
        public float BurstReadyStaminaRatio { get; set; } = 0.7f;
        public float BurstReadyDelayMin { get; set; } = 0.4f;
        public float BurstReadyDelayMax { get; set; } = 1.1f;
        public float BurstReelDangerWindow { get; set; } = 2f;
        public float BurstReelDangerTensionPressureMin { get; set; } = 0.18f;
        public float BurstReelDangerTensionPressureMax { get; set; } = 0.42f;
        public float BurstReelProgressCapRatio { get; set; } = 0.22f;
        public float BurstReelResistancePenaltyMin { get; set; } = 0.9f;
        public float BurstReelResistancePenaltyMax { get; set; } = 1.6f;

        // Fight - Weight Variation
        public float WeightStaminaLerpMin { get; set; } = 0.88f;
        public float WeightStaminaLerpMax { get; set; } = 1.18f;
        public float WeightResistanceLerpMin { get; set; } = 0.9f;
        public float WeightResistanceLerpMax { get; set; } = 1.25f;
        public float WeightEscapeLerpMin { get; set; } = 0.92f;
        public float WeightEscapeLerpMax { get; set; } = 1.18f;

        // Fight - Phase Multipliers
        public float SteadyForceLerpWeight { get; set; } = 0.35f;
        public float BurstTensionDrainScale { get; set; } = 2.1f;
        public float FatigueTensionLerpWeight { get; set; } = 0.25f;
        public float SteadyTensionDrainScale { get; set; } = 0.1f;
        public float ReelResistanceBase { get; set; } = 0.2f;
        public float ReelResistanceDrainScale { get; set; } = 0.24f;
        public float BurstReelStruggleScale { get; set; } = 0.3f;
        public float FatigueReelResistanceFloor { get; set; } = 0.08f;
        public float FatigueReelResistanceScale { get; set; } = 0.6f;

        // Fight - Misc
        public float PullForceStaminaLerpMin { get; set; } = 0.55f;
        public float FatigueControlBonus { get; set; } = 0.18f;
        public float RelaxBoostScale { get; set; } = 0.5f;
        public float EarlyReelSpeedMul { get; set; } = 6f;
        public float WaitTimeMultiplierFloor { get; set; } = 0.45f;
        public float ControlSuppressionMax { get; set; } = 0.65f;
        public float SteadyDurationScale { get; set; } = 0.7f;
        public float BaitMismatchWeightScale { get; set; } = 0.55f;
    }

    public class FishRodConfig
    {
        public int RodId { get; set; }
        public string Name { get; set; }
        public int BuyCostCoin { get; set; }
        public float LineStrength { get; set; }
        public float ReelSpeed { get; set; }
        public float ControlPower { get; set; }
        public float SuppressPower { get; set; }
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
            [typeof(FishRodConfig)] = "fishrod",
            [typeof(FishBaitConfig)] = "fishbait",
            [typeof(FishSpeciesConfig)] = "fishspecies",
            [typeof(FishDistanceTierConfig)] = "fishdistancetier"
        };

        private List<FishGlobalConfig> _fishGlobalList;
        private List<FishRodConfig> _fishRodList;
        private List<FishBaitConfig> _fishBaitList;
        private List<FishSpeciesConfig> _fishSpeciesList;
        private List<FishDistanceTierConfig> _fishDistanceTierList;

        private readonly Dictionary<int, FishRodConfig> _rodCache = new();
        private readonly Dictionary<int, FishBaitConfig> _baitCache = new();
        private readonly Dictionary<int, FishSpeciesConfig> _speciesCache = new();
        private readonly List<FishBaitConfig> _enabledBaitsCache = new();
        private bool _enabledBaitsBuilt;

        public FishGlobalConfig GlobalConfig => GetSingleConfig<FishGlobalConfig>();
        public List<FishRodConfig> RodConfigs => GetConfig<FishRodConfig>();
        public List<FishBaitConfig> BaitConfigs => GetConfig<FishBaitConfig>();
        public List<FishSpeciesConfig> SpeciesConfigs => GetConfig<FishSpeciesConfig>();
        public List<FishDistanceTierConfig> DistanceTierConfigs => GetConfig<FishDistanceTierConfig>();

        public FishRodConfig GetRodConfig(int rodId)
        {
            if (_rodCache.TryGetValue(rodId, out var config))
            {
                return config;
            }

            config = RodConfigs.Find(item => item.RodId == rodId);
            if (config == null && RodConfigs.Count > 0)
            {
                config = RodConfigs[0];
            }

            _rodCache[rodId] = config;
            return config;
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
                "fishrod" => _fishRodList as List<T>,
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
                case "fishrod":
                    if (_fishRodList != null)
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
                case "fishrod":
                    _fishRodList = JsonConvert.DeserializeObject<List<FishRodConfig>>(textAsset.text);
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
