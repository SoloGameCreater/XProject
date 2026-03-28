using System.Collections.Generic;
using System.Text;
using Config.FishGame;
using GameplayRuntime;
using Framework;
using SaveFile;
using UnityEngine;

namespace FishGameRuntime
{
    public sealed partial class FishGamePresenterV2
    {
        private enum FishingState
        {
            Idle,
            Casting,
            Waiting,
            Fighting,
            AutoRetrieve
        }

        private sealed class FightData
        {
            public FishSpeciesConfig Fish;
            public float Weight;
            public float Stamina;
            public float StaminaMax;
            public float Resistance;
            public float EscapeSpeed;
            public float Tension;
            public float LineLength;
            public float LineLengthMax;
            public float LineLengthMin;
            public bool Reeling;
            public bool Locking;
            public bool IsSprinting;
            public float SprintTimer;
            public float SprintCooldown;
        }

        private const float MaxCastChargeDuration = 1.5f;
        private const float TensionRelaxDuration = 0.5f;

        private readonly FishGameMainUI _view;
        private readonly FishGameConfigManager _configManager;
        private readonly SaveFileFishGame _save;
        private readonly FightData _fight = new();
        private readonly List<FishBaitConfig> _selectableBaitsBuffer = new();

        private FishingState _state = FishingState.Idle;
        private float _castingTimer;
        private float _waitTimer;
        private bool _waitWillHook;
        private float _notificationTimer;
        private string _notificationMessage = string.Empty;
        private Color _notificationColor = Color.white;
        private bool _isSwitching;

        public FishGamePresenterV2(FishGameMainUI view)
        {
            _view = view;
            _configManager = FishGameConfigManager.Instance;
            _save = SaveFileManager.Instance.GetSaveFile<SaveFileFishGame>();

            EnsurePersistentState();
            ResetFightDataForCast();
            ShowNotification("按住屏幕中央的抛竿区蓄力，松开后开始钓鱼。", 3f, Color.white);
            RefreshUi();
        }

        public void Tick(float deltaTime)
        {
            UpdateNotification(deltaTime);
            UpdateInput();
            UpdateStateTimers(deltaTime);
            UpdateFight(deltaTime);
            RefreshUi();
        }

        public void OnCastReleased(float holdDuration)
        {
            if (_state != FishingState.Idle)
            {
                return;
            }

            if (!ConsumeCurrentBaitForCast())
            {
                ShowNotification("当前鱼饵库存不足，无法抛竿。", 2.2f, new Color(1f, 0.45f, 0.45f));
                return;
            }

            ResetFightDataForCast();
            _fight.LineLength = GetCastLineLength(holdDuration);
            _state = FishingState.Casting;
            _castingTimer = 0.8f;
            ShowNotification($"抛竿中... 本次抛投 {_fight.LineLength:F1} m", 1f, Color.white);
        }

        public void OnPrevLureClicked()
        {
            ChangeBait(-1);
        }

        public void OnNextLureClicked()
        {
            ChangeBait(1);
        }

        public async void OnSwitchClicked()
        {
            if (_state != FishingState.Idle || _isSwitching)
            {
                return;
            }

            _isSwitching = true;
            ShowNotification("正在切换到三消玩法...", 2f, new Color(1f, 0.84f, 0.35f));
            DebugUtil.Log("FishGamePresenterV2: switching to TripleMerge.");
            try
            {
                await GameplayDirector.Instance.EnterAsync(GameplayIds.TripleMerge);
            }
            finally
            {
                _isSwitching = false;
            }
        }

        public void OnSellAllClicked()
        {
            if (_state != FishingState.Idle)
            {
                return;
            }

            if (_save == null || _save.FishBag.Count == 0)
            {
                ShowNotification("鱼篓为空，没有可出售的鱼。", 1.8f, Color.white);
                return;
            }

            var totalPrice = 0;
            var soldCount = _save.FishBag.Count;
            for (var i = 0; i < _save.FishBag.Count; i++)
            {
                totalPrice += _save.FishBag[i].SellPrice;
            }

            _save.FishBag.Clear();
            _save.TotalSoldCount += soldCount;
            CurrencyModel.Instance.AddCurrency(CurrencyType.Coin, totalPrice);
            SaveFileManager.Instance.TryAutoSave("FishGame.SellAll", true);
            ShowNotification($"一键出售完成，获得 {totalPrice} Coin。", 2.2f, new Color(0.45f, 1f, 0.55f));
        }

        public void OnBuyBaitClicked()
        {
            if (_state != FishingState.Idle)
            {
                return;
            }

            var bait = GetCurrentBait();
            var rod = GetCurrentRod();
            var global = _configManager?.GlobalConfig;
            if (bait == null || rod == null || global == null)
            {
                ShowNotification("鱼饵配置未准备好。", 2f, new Color(1f, 0.45f, 0.45f));
                return;
            }

            if (bait.BaitType != FishBaitType.Consumable || !bait.CanPurchase)
            {
                ShowNotification("当前鱼饵暂不支持购买。", 2f, Color.white);
                return;
            }

            if (!SupportsBait(rod, bait))
            {
                ShowNotification("当前鱼竿等级不足，无法使用或购买该鱼饵。", 2.2f, new Color(1f, 0.65f, 0.3f));
                return;
            }

            if (!CurrencyModel.Instance.IsCurrencyEnough(CurrencyType.Coin, bait.BuyPriceCoin))
            {
                ShowNotification("金币不足，无法购买鱼饵。", 2f, new Color(1f, 0.45f, 0.45f));
                return;
            }

            CurrencyModel.Instance.CostCurrency(CurrencyType.Coin, bait.BuyPriceCoin);
            AddBaitCount(bait.BaitId, global.BaitPackCount);
            ShowNotification($"购买 {bait.Name} 成功，获得 {global.BaitPackCount} 个。", 2.2f, new Color(0.45f, 1f, 0.55f));
        }

        public void OnUpgradeRodClicked()
        {
            if (_state != FishingState.Idle)
            {
                return;
            }

            var currentRod = GetCurrentRod();
            var nextRod = _configManager?.GetNextRodLevelConfig(_save != null ? _save.RodLevel : 0);
            if (currentRod == null)
            {
                ShowNotification("鱼竿配置未准备好。", 2f, new Color(1f, 0.45f, 0.45f));
                return;
            }

            if (nextRod == null)
            {
                ShowNotification("鱼竿已达到最高等级。", 1.8f, Color.white);
                return;
            }

            if (!CurrencyModel.Instance.IsCurrencyEnough(CurrencyType.Coin, nextRod.UpgradeCostCoin))
            {
                ShowNotification("金币不足，无法升级鱼竿。", 2f, new Color(1f, 0.45f, 0.45f));
                return;
            }

            CurrencyModel.Instance.CostCurrency(CurrencyType.Coin, nextRod.UpgradeCostCoin);
            _save.RodLevel = nextRod.Level;
            EnsureSelectedBaitValid();
            SaveFileManager.Instance.TryAutoSave("FishGame.UpgradeRod", true);
            ShowNotification($"鱼竿升级成功：{currentRod.Name} -> {nextRod.Name}", 2.2f, new Color(0.45f, 1f, 0.55f));
        }

        private void UpdateInput()
        {
            if (_view == null)
            {
                return;
            }

            if (_state == FishingState.Idle && _view.CastZoneClickedThisFrame)
            {
                OnCastReleased(_view.CastZoneHoldDuration);
            }
        }

        private void UpdateNotification(float deltaTime)
        {
            if (_notificationTimer <= 0f)
            {
                return;
            }

            _notificationTimer -= deltaTime;
            if (_notificationTimer <= 0f)
            {
                _notificationMessage = string.Empty;
                if (_view != null)
                {
                    _view.NotifyText.text = string.Empty;
                }
            }
        }

        private void UpdateStateTimers(float deltaTime)
        {
            if (_state == FishingState.Casting)
            {
                _castingTimer -= deltaTime;
                if (_castingTimer <= 0f)
                {
                    BeginWaiting();
                }
            }

            if (_state == FishingState.Waiting)
            {
                _waitTimer -= deltaTime;
                if (_waitTimer <= 0f)
                {
                    if (_waitWillHook)
                    {
                        StartFight(PickFish());
                    }
                    else
                    {
                        ShowNotification("本次抛投没有鱼咬钩。", 1.4f, Color.white);
                        ResetFight();
                    }
                }
            }
        }

        private void UpdateFight(float deltaTime)
        {
            var rod = GetCurrentRod();
            var global = _configManager?.GlobalConfig;
            if (rod == null || global == null)
            {
                return;
            }

            var isPrimaryHeld = _view != null && _view.IsPrimaryHeld;
            var isSecondaryHeld = _view != null && _view.IsSecondaryHeld;

            if (_state == FishingState.Waiting)
            {
                if (isPrimaryHeld)
                {
                    _fight.LineLength = Mathf.Max(_fight.LineLengthMin, _fight.LineLength - rod.ReelSpeed * 6f * deltaTime);
                    if (_fight.LineLength <= _fight.LineLengthMin + 0.05f)
                    {
                        ShowNotification("鱼线已收回，返回待命。", 1.2f, Color.white);
                        ResetFight();
                    }
                }

                return;
            }

            if (_state == FishingState.AutoRetrieve)
            {
                _fight.LineLength = Mathf.Max(_fight.LineLengthMin, _fight.LineLength - global.AutoRetrieveSpeed * deltaTime);
                if (_fight.LineLength <= _fight.LineLengthMin + 0.05f)
                {
                    ResetFight();
                }

                return;
            }

            if (_state != FishingState.Fighting || _fight.Fish == null)
            {
                return;
            }

            _fight.Reeling = isPrimaryHeld;
            _fight.Locking = !isPrimaryHeld && isSecondaryHeld;

            UpdateSprint(deltaTime);

            var staminaRatio = _fight.StaminaMax <= 0f ? 0f : _fight.Stamina / _fight.StaminaMax;
            var fishPullSpeed = GetFishPullSpeed(rod, staminaRatio);

            if (_fight.Reeling)
            {
                var reelSpeed = rod.ReelSpeed * Mathf.Lerp(1.05f, 0.62f, Mathf.Clamp01(_fight.Resistance / 2.4f));
                var minimumReelSpeed = Mathf.Max(0.35f, rod.ReelSpeed * 0.4f);
                var netReelSpeed = Mathf.Max(minimumReelSpeed, reelSpeed - fishPullSpeed * 0.18f);
                _fight.LineLength = Mathf.Max(_fight.LineLengthMin, _fight.LineLength - netReelSpeed * deltaTime);
                _fight.Stamina = Mathf.Max(0f, _fight.Stamina - (rod.ReelSpeed * (1.35f + _fight.Resistance * 0.65f)) * deltaTime);
                _fight.Tension += (fishPullSpeed * 2.3f + _fight.Resistance * (5.2f + staminaRatio * 1.6f)) * deltaTime;
            }
            else if (_fight.Locking)
            {
                var sprintBonus = _fight.IsSprinting ? 1.25f : 1f;
                _fight.Stamina = Mathf.Max(0f, _fight.Stamina - rod.LockLineStaminaDamagePerSec * sprintBonus * deltaTime);
            }
            else
            {
                var tensionRecoveryPerSecond = rod.LineStrength / TensionRelaxDuration;
                _fight.LineLength = Mathf.Min(_fight.LineLengthMax, _fight.LineLength + fishPullSpeed * deltaTime);
                _fight.Tension = Mathf.MoveTowards(_fight.Tension, 0f, tensionRecoveryPerSecond * deltaTime);
                _fight.Stamina = Mathf.Min(_fight.StaminaMax, _fight.Stamina + _fight.Fish.StaminaRecoveryPerSec * (_fight.IsSprinting ? 0.4f : 1f) * deltaTime);
            }

            if (_fight.LineLength > global.FightFailDistance)
            {
                BeginAutoRetrieveFail("鱼跑远了，正在快速收线。");
                return;
            }

            if (_fight.Tension >= rod.LineStrength)
            {
                FailAndReset("鱼线断裂：张力超过鱼竿承载。");
                return;
            }

            if (_fight.Stamina <= 0f && _fight.LineLength <= _fight.LineLengthMin + 0.2f)
            {
                CatchFish();
            }
        }

        private void BeginWaiting()
        {
            var global = _configManager?.GlobalConfig;
            var bait = GetCurrentBait();
            var tier = _configManager?.GetDistanceTier(_fight.LineLength);
            if (global == null || bait == null)
            {
                ResetFight();
                return;
            }

            _state = FishingState.Waiting;
            _fight.LineLengthMax = global.FightLineClampDistance;

            if (_fight.LineLength <= global.NoBiteDistance || tier == null || !tier.CanBite)
            {
                _waitWillHook = false;
                _waitTimer = global.EmptyHookCooldown;
                ShowNotification("抛投距离过近，本次不会有鱼咬钩。", 1.8f, new Color(1f, 0.82f, 0.3f));
                return;
            }

            _waitWillHook = true;
            var baseWait = Random.Range(global.WaitTimeMin, global.WaitTimeMax);
            _waitTimer = baseWait * Mathf.Max(0.45f, bait.WaitTimeMultiplier) * Mathf.Max(0.45f, tier.WaitTimeMultiplier);
            ShowNotification("鱼线已入水，等待咬钩。按住鼠标左键可提前收线。", 1.8f, new Color(0.75f, 0.9f, 1f));
        }

        private void ChangeBait(int direction)
        {
            var selectableBaits = GetSelectableBaits();
            if (selectableBaits == null || selectableBaits.Count == 0)
            {
                ShowNotification("当前没有可用鱼饵。", 1.8f, new Color(1f, 0.45f, 0.45f));
                return;
            }

            var currentId = _save != null ? _save.EquippedBaitId : 0;
            var currentIndex = selectableBaits.FindIndex(item => item.BaitId == currentId);
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }

            currentIndex += direction;
            if (currentIndex < 0)
            {
                currentIndex = selectableBaits.Count - 1;
            }
            else if (currentIndex >= selectableBaits.Count)
            {
                currentIndex = 0;
            }

            _save.EquippedBaitId = selectableBaits[currentIndex].BaitId;
            SaveFileManager.Instance.TryAutoSave("FishGame.ChangeBait");
            ShowNotification($"已切换鱼饵：{selectableBaits[currentIndex].Name}", 1.8f, Color.white);
        }
        private void StartFight(FishSpeciesConfig fish)
        {
            var global = _configManager?.GlobalConfig;
            if (fish == null || global == null)
            {
                FailAndReset("Missing fish config.");
                return;
            }

            _fight.Fish = fish;
            _fight.Weight = Mathf.Round(Random.Range(fish.WeightMin, fish.WeightMax) * 10f) / 10f;
            var weightRatio = fish.WeightMax <= fish.WeightMin ? 1f : Mathf.InverseLerp(fish.WeightMin, fish.WeightMax, _fight.Weight);
            _fight.StaminaMax = fish.BaseStamina * Mathf.Lerp(0.88f, 1.18f, weightRatio);
            _fight.Stamina = _fight.StaminaMax;
            _fight.Resistance = fish.Resistance * Mathf.Lerp(0.9f, 1.25f, weightRatio);
            _fight.EscapeSpeed = fish.EscapeSpeed * Mathf.Lerp(0.92f, 1.18f, weightRatio);
            _fight.Tension = 0f;
            _fight.LineLength = Mathf.Clamp(_fight.LineLength, _fight.LineLengthMin, global.FightLineClampDistance);
            _fight.LineLengthMax = global.FightLineClampDistance;
            _fight.IsSprinting = false;
            _fight.SprintTimer = 0f;
            _fight.SprintCooldown = fish.SprintInterval + Random.Range(-0.8f, 1.2f);
            _fight.Reeling = false;
            _fight.Locking = false;
            _state = FishingState.Fighting;
            _view?.ResetPointerState();
            ShowNotification($"Hooked: {fish.Species} {_fight.Weight:F1} kg", 3f, GetRarityColor(fish.Rarity));
        }

        private void CatchFish()
        {
            if (_save == null || _fight.Fish == null)
            {
                ResetFight();
                return;
            }

            var price = Mathf.Max(1, Mathf.RoundToInt(_fight.Weight * _fight.Fish.BasePricePerKg));
            _save.FishBag.Add(new SaveFileFishBagEntry
            {
                FishId = _fight.Fish.FishId,
                Species = _fight.Fish.Species,
                Rarity = _fight.Fish.Rarity,
                Weight = _fight.Weight,
                SellPrice = price
            });
            _save.TotalCaughtCount += 1;
            SaveFileManager.Instance.TryAutoSave("FishGame.CatchFish", true);
            ShowNotification($"Caught: {_fight.Fish.Species} {_fight.Weight:F1} kg", 2.8f, new Color(0.45f, 1f, 0.55f));
            DebugUtil.Log($"FishGamePresenterV2: caught {_fight.Fish.Species}, weight={_fight.Weight:F1}, price={price}");
            ResetFight();
        }

        private void BeginAutoRetrieveFail(string reason)
        {
            ShowNotification(reason, 2.4f, new Color(1f, 0.45f, 0.45f));
            DebugUtil.LogWarning($"FishGamePresenterV2: auto retrieve fail, reason={reason}");
            _fight.Reeling = false;
            _fight.Locking = false;
            _state = FishingState.AutoRetrieve;
        }

        private void FailAndReset(string reason)
        {
            ShowNotification(reason, 2.4f, new Color(1f, 0.45f, 0.45f));
            DebugUtil.LogWarning($"FishGamePresenterV2: fishing failed, reason={reason}");
            ResetFight();
        }

        private void ResetFight()
        {
            ResetFightDataForCast();
            _state = FishingState.Idle;
        }

        private void ResetFightDataForCast()
        {
            var global = _configManager?.GlobalConfig;
            _fight.Fish = null;
            _fight.Weight = 0f;
            _fight.Stamina = 0f;
            _fight.StaminaMax = 0f;
            _fight.Resistance = 0f;
            _fight.EscapeSpeed = 0f;
            _fight.Tension = 0f;
            _fight.LineLengthMin = global != null ? global.CatchLineDistance : 1.5f;
            _fight.LineLengthMax = global != null ? global.MaxCastDistance : 40f;
            _fight.LineLength = global != null ? global.MaxCastDistance * 0.5f : 20f;
            _fight.Reeling = false;
            _fight.Locking = false;
            _fight.IsSprinting = false;
            _fight.SprintTimer = 0f;
            _fight.SprintCooldown = 0f;
            _waitWillHook = false;
            _view?.ResetPointerState();
        }

        private FishSpeciesConfig PickFish()
        {
            var bait = GetCurrentBait();
            var tier = _configManager?.GetDistanceTier(_fight.LineLength);
            var rod = GetCurrentRod();
            var speciesConfigs = _configManager?.SpeciesConfigs;
            if (bait == null || tier == null || rod == null || speciesConfigs == null || speciesConfigs.Count == 0)
            {
                return null;
            }

            var totalWeight = 0f;
            var weightedCandidates = new List<(FishSpeciesConfig fish, float weight)>();
            for (var i = 0; i < speciesConfigs.Count; i++)
            {
                var fish = speciesConfigs[i];
                var weight = Mathf.Max(0.01f, fish.CatchWeight);
                weight *= Mathf.Max(0.01f, _configManager.GetRarityWeight(tier, fish.Rarity));
                weight *= 1f + Mathf.Max(0f, fish.Level - 1) * (tier.FishLevelWeightBonus + bait.HighLevelWeightBonus);

                if (HasTagOverlap(fish.PreferredBaitTags, bait.TargetTags))
                {
                    weight *= Mathf.Max(1f, bait.HookWeightBonus);
                }
                else if (bait.TargetTags != null && bait.TargetTags.Length > 0)
                {
                    weight *= 0.55f;
                }

                var levelGap = Mathf.Max(0f, fish.Level - rod.RecommendFishLevelMax);
                var rodGap = Mathf.Max(0f, fish.RecommendRodLevel - rod.Level);
                if (levelGap > 0f || rodGap > 0f)
                {
                    var recommendationPenalty = 1f - Mathf.Min(0.18f, levelGap * 0.04f + rodGap * 0.06f);
                    weight *= Mathf.Max(0.72f, recommendationPenalty);
                }

                totalWeight += weight;
                weightedCandidates.Add((fish, weight));
            }

            if (totalWeight <= 0f)
            {
                return speciesConfigs[Random.Range(0, speciesConfigs.Count)];
            }

            var randomValue = Random.Range(0f, totalWeight);
            for (var i = 0; i < weightedCandidates.Count; i++)
            {
                randomValue -= weightedCandidates[i].weight;
                if (randomValue <= 0f)
                {
                    return weightedCandidates[i].fish;
                }
            }

            return weightedCandidates[weightedCandidates.Count - 1].fish;
        }

        private void RefreshUi()
        {
            if (_view == null)
            {
                return;
            }

            var rod = GetCurrentRod();
            var bait = GetCurrentBait();
            var coins = CurrencyModel.Instance != null ? CurrencyModel.Instance.GetCurrencyAmount(CurrencyType.Coin) : 0;
            var bagValue = GetFishBagValue();

            _view.StateText.text = _state switch
            {
                FishingState.Idle => "待命",
                FishingState.Casting => "抛竿",
                FishingState.Waiting => "等待",
                FishingState.Fighting => "遛鱼",
                FishingState.AutoRetrieve => "回收",
                _ => "未知"
            };
            _view.StateText.color = _state switch
            {
                FishingState.Casting => new Color(1f, 0.84f, 0.35f),
                FishingState.Waiting => new Color(0.72f, 0.9f, 1f),
                FishingState.Fighting => new Color(0.45f, 1f, 0.55f),
                FishingState.AutoRetrieve => new Color(1f, 0.55f, 0.35f),
                _ => Color.white
            };

            var baitCount = bait != null ? GetBaitCount(bait.BaitId) : 0;
            _view.EquipText.text = rod == null || bait == null
                ? "装备配置 / 未初始化"
                : $"装备配置 / {rod.Name} Lv.{rod.Level} / {bait.Name} x{baitCount}";
            _view.ScoreText.text = $"金币\n{coins}";

            var nextRod = _configManager?.GetNextRodLevelConfig(_save != null ? _save.RodLevel : 0);
            var selectableBaits = GetSelectableBaits();
            var canCycleBait = _state == FishingState.Idle && selectableBaits.Count > 1;
            var canSellAll = _state == FishingState.Idle && _save != null && _save.FishBag.Count > 0;
            var canBuyBait = _state == FishingState.Idle && CanBuyCurrentBait(bait, rod, coins);
            var canUpgradeRod = _state == FishingState.Idle && nextRod != null && coins >= nextRod.UpgradeCostCoin;

            _view.SetButtonText(_view.CastButton, $"全部出售\n{bagValue} Coin");
            _view.SetButtonText(_view.ReelButton, bait == null ? "购买鱼饵" : $"购买鱼饵\n{bait.BuyPriceCoin} Coin");
            _view.SetButtonText(_view.ReleaseButton, nextRod == null ? "鱼竿满级" : $"升级鱼竿\n{nextRod.UpgradeCostCoin} Coin");
            _view.SetButtonInteractable(_view.CastButton, canSellAll);
            _view.SetButtonInteractable(_view.ReelButton, canBuyBait);
            _view.SetButtonInteractable(_view.ReleaseButton, canUpgradeRod);
            _view.SetButtonInteractable(_view.PrevLureButton, canCycleBait);
            _view.SetButtonInteractable(_view.NextLureButton, canCycleBait);
            _view.SetButtonInteractable(_view.SwitchButton, _state == FishingState.Idle);

            if (_state == FishingState.Fighting && _fight.Fish != null)
            {
                var rodLineStrength = rod != null ? rod.LineStrength : 1f;
                var tensionRatio = _fight.Tension / Mathf.Max(1f, rodLineStrength);
                _view.SetBar(_view.StaminaFill, _fight.StaminaMax <= 0f ? 0f : _fight.Stamina / _fight.StaminaMax);
                _view.SetBar(_view.TensionFill, tensionRatio);
                _view.SetBar(_view.LineFill, 1f - Mathf.Clamp01((_fight.LineLength - _fight.LineLengthMin) / Mathf.Max(0.1f, _fight.LineLengthMax - _fight.LineLengthMin)));
                _view.TensionFill.color = tensionRatio switch
                {
                    < 0.6f => new Color(0.31f, 0.78f, 0.31f),
                    < 0.85f => new Color(0.95f, 0.72f, 0.12f),
                    _ => new Color(0.86f, 0.26f, 0.26f)
                };
                _view.LineFill.color = _fight.Locking ? new Color(1f, 0.78f, 0.25f) : new Color(0.31f, 0.63f, 1f);
                _view.StaminaValueText.text = $"{_fight.Stamina:F0} / {_fight.StaminaMax:F0}";
                _view.TensionValueText.text = $"{_fight.Tension:F0} / {rodLineStrength:F0}";
                _view.LineValueText.text = $"{_fight.LineLength:F1} m";
                _view.LineText.text = $"目标鱼 / {_fight.Fish.Species} / {_fight.Weight:F1} kg / 等级 {_fight.Fish.Level} / 阻力 {_fight.Resistance:F2}";
                _view.HintText.text = _fight.Locking
                    ? "按住右键锁线：鱼的距离保持不变，体力持续下降，当前张力保持不变。"
                    : _fight.Reeling
                        ? "按住左键持续收线；鱼线会稳定回收，同时张力会持续上涨。"
                        : "松开左右键会卸力，张力会在 0.5 秒内回到 0；按住右键可锁线。";
            }
            else if (_state == FishingState.Waiting)
            {
                _view.SetBar(_view.StaminaFill, 0f);
                _view.SetBar(_view.TensionFill, 0f);
                _view.SetBar(_view.LineFill, 1f - Mathf.Clamp01((_fight.LineLength - _fight.LineLengthMin) / Mathf.Max(0.1f, _fight.LineLengthMax - _fight.LineLengthMin)));
                _view.LineFill.color = new Color(0.31f, 0.63f, 1f);
                _view.StaminaValueText.text = "--";
                _view.TensionValueText.text = rod != null ? $"0 / {rod.LineStrength:F0}" : "--";
                _view.LineValueText.text = $"{_fight.LineLength:F1} m";
                _view.LineText.text = _waitWillHook
                    ? $"等待咬钩 / 当前线长 {_fight.LineLength:F1} m"
                    : $"抛投过近 / 当前线长 {_fight.LineLength:F1} m / 本次不会咬钩";
                _view.HintText.text = "等待阶段按住左键可提前收线；未咬钩时会自动返回待命。";
            }
            else if (_state == FishingState.Casting || _state == FishingState.AutoRetrieve)
            {
                _view.SetBar(_view.StaminaFill, 0f);
                _view.SetBar(_view.TensionFill, 0f);
                _view.SetBar(_view.LineFill, 1f - Mathf.Clamp01((_fight.LineLength - _fight.LineLengthMin) / Mathf.Max(0.1f, _fight.LineLengthMax - _fight.LineLengthMin)));
                _view.LineFill.color = _state == FishingState.AutoRetrieve ? new Color(1f, 0.55f, 0.35f) : new Color(0.31f, 0.63f, 1f);
                _view.StaminaValueText.text = "--";
                _view.TensionValueText.text = rod != null ? $"0 / {rod.LineStrength:F0}" : "--";
                _view.LineValueText.text = $"{_fight.LineLength:F1} m";
                _view.LineText.text = _state == FishingState.Casting ? "抛竿动作进行中。" : "正在自动快速收线。";
                _view.HintText.text = _state == FishingState.Casting ? "鱼线落水后会进入等待阶段。" : "失败后会自动回收鱼线并返回待命。";
            }
            else
            {
                var currentCastLength = GetCastLineLength(_view.CastZoneHoldDuration);
                var isChargingCast = _view.IsCastZonePressActive;
                _view.SetBar(_view.StaminaFill, 0f);
                _view.SetBar(_view.TensionFill, 0f);
                _view.SetBar(_view.LineFill, 0f);
                _view.LineFill.color = new Color(0.31f, 0.63f, 1f);
                _view.StaminaValueText.text = "--";
                _view.TensionValueText.text = "--";
                _view.LineValueText.text = "--";
                _view.LineText.text = isChargingCast
                    ? $"蓄力抛竿 / 当前目标线长 {currentCastLength:F1} m"
                    : $"待命 / 鱼篓 {(_save != null ? _save.FishBag.Count : 0)} 条 / 当前总价值 {bagValue} Coin";
                _view.HintText.text = isChargingCast
                    ? "按住越久抛得越远，超过 5m 才有机会咬钩。"
                    : "待命时按住左键蓄力抛竿；战斗中左键收线，右键锁线。";
            }

            _view.ApplyInputMode(_state == FishingState.Idle);
            _view.UpdateCastZoneChargeDisplay(
                currentDistance: GetCastLineLength(_view.CastZoneHoldDuration),
                chargeRatio: GetCastChargeRatio(_view.CastZoneHoldDuration),
                isCharging: _state == FishingState.Idle && _view.IsCastZonePressActive);
            RefreshInventoryText();
        }

        private void RefreshInventoryText()
        {
            if (_view == null)
            {
                return;
            }

            if (_save == null || _save.FishBag.Count == 0)
            {
                _view.InventoryText.text = "鱼篓为空\n\n钓到的鱼会显示在这里。";
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("鱼篓");
            builder.AppendLine();
            var totalPrice = 0;
            for (var i = 0; i < _save.FishBag.Count; i++)
            {
                var entry = _save.FishBag[i];
                totalPrice += entry.SellPrice;
                builder.AppendLine($"{i + 1}. {entry.Species}  {entry.Weight:F1} kg  {entry.Rarity}  {entry.SellPrice} Coin");
            }

            builder.AppendLine();
            builder.AppendLine($"总价值：{totalPrice} Coin");
            builder.AppendLine($"累计钓获：{_save.TotalCaughtCount}  累计售出：{_save.TotalSoldCount}");
            _view.InventoryText.text = builder.ToString();
        }

        private void ShowNotification(string message, float duration, Color color)
        {
            _notificationMessage = message;
            _notificationColor = color;
            _notificationTimer = duration;
            ApplyNotification();
        }

        private void ApplyNotification()
        {
            if (_view == null)
            {
                return;
            }

            _view.NotifyText.text = _notificationMessage;
            _view.NotifyText.color = _notificationColor;
        }

        private void EnsurePersistentState()
        {
            var global = _configManager?.GlobalConfig;
            if (_save == null || global == null)
            {
                return;
            }

            if (!_save.IsInitialized)
            {
                _save.RodLevel = Mathf.Max(1, global.StarterRodLevel);
                _save.EquippedBaitId = global.StarterBaitId;
                AddBaitCount(global.StarterBaitId, global.StarterBaitCount, false);
                _save.IsInitialized = true;
                SaveFileManager.Instance.TryAutoSave("FishGame.Init", true);
            }

            EnsureSelectedBaitValid();
        }

        private void EnsureSelectedBaitValid()
        {
            if (_save == null)
            {
                return;
            }

            var currentBait = GetCurrentBait();
            if (currentBait != null)
            {
                return;
            }

            var selectableBaits = GetSelectableBaits();
            if (selectableBaits != null && selectableBaits.Count > 0)
            {
                _save.EquippedBaitId = selectableBaits[0].BaitId;
                SaveFileManager.Instance.TryAutoSave("FishGame.EnsureSelectedBait");
            }
        }

        private FishRodLevelConfig GetCurrentRod()
        {
            return _configManager?.GetRodLevelConfig(_save != null ? _save.RodLevel : 0);
        }

        private FishBaitConfig GetCurrentBait()
        {
            if (_save == null || _configManager == null)
            {
                return null;
            }

            _configManager.TryGetBaitConfig(_save.EquippedBaitId, out var bait);
            return bait != null && bait.EnabledPhase <= FishGameConfigManager.CurrentEnabledPhase ? bait : null;
        }

        private List<FishBaitConfig> GetSelectableBaits()
        {
            _selectableBaitsBuffer.Clear();
            var rod = GetCurrentRod();
            var enabledBaits = _configManager?.GetEnabledBaits();
            if (rod == null || enabledBaits == null)
            {
                return _selectableBaitsBuffer;
            }

            for (var i = 0; i < enabledBaits.Count; i++)
            {
                if (SupportsBait(rod, enabledBaits[i]))
                {
                    _selectableBaitsBuffer.Add(enabledBaits[i]);
                }
            }

            return _selectableBaitsBuffer;
        }

        private bool ConsumeCurrentBaitForCast()
        {
            var bait = GetCurrentBait();
            if (bait == null)
            {
                return false;
            }

            if (bait.BaitType != FishBaitType.Consumable || bait.ConsumePerCast <= 0)
            {
                return true;
            }

            var count = GetBaitCount(bait.BaitId);
            if (count < bait.ConsumePerCast)
            {
                return false;
            }

            SetBaitCount(bait.BaitId, count - bait.ConsumePerCast);
            SaveFileManager.Instance.TryAutoSave("FishGame.ConsumeBait", true);
            return true;
        }

        private void AddBaitCount(int baitId, int amount, bool saveImmediately = true)
        {
            if (_save == null || amount <= 0)
            {
                return;
            }

            SetBaitCount(baitId, GetBaitCount(baitId) + amount);
            if (saveImmediately)
            {
                SaveFileManager.Instance.TryAutoSave("FishGame.AddBait", true);
            }
        }

        private void SetBaitCount(int baitId, int amount)
        {
            if (_save == null)
            {
                return;
            }

            if (_save.OwnedBaits.TryGetValue(baitId, out var safeCount))
            {
                safeCount.SetValue(amount);
                return;
            }

            var newSafeCount = new SaveFileSafeCount();
            newSafeCount.SetValue(amount);
            _save.OwnedBaits.Add(baitId, newSafeCount);
        }

        private int GetBaitCount(int baitId)
        {
            if (_save == null || !_save.OwnedBaits.TryGetValue(baitId, out var safeCount))
            {
                return 0;
            }

            return safeCount.GetValue();
        }

        private int GetFishBagValue()
        {
            if (_save == null)
            {
                return 0;
            }

            var total = 0;
            for (var i = 0; i < _save.FishBag.Count; i++)
            {
                total += _save.FishBag[i].SellPrice;
            }

            return total;
        }

        private static bool CanBuyCurrentBait(FishBaitConfig bait, FishRodLevelConfig rod, int coins)
        {
            if (bait == null || rod == null)
            {
                return false;
            }

            if (bait.BaitType != FishBaitType.Consumable || !bait.CanPurchase)
            {
                return false;
            }

            if (!SupportsBait(rod, bait))
            {
                return false;
            }

            return coins >= bait.BuyPriceCoin;
        }

        private void UpdateSprint(float deltaTime)
        {
            if (_fight.Fish == null)
            {
                return;
            }

            if (!_fight.IsSprinting)
            {
                _fight.SprintCooldown -= deltaTime;
                if (_fight.SprintCooldown <= 0f)
                {
                    _fight.IsSprinting = true;
                    _fight.SprintTimer = _fight.Fish.SprintDuration;
                }
            }
            else
            {
                _fight.SprintTimer -= deltaTime;
                if (_fight.SprintTimer <= 0f)
                {
                    _fight.IsSprinting = false;
                    _fight.SprintCooldown = _fight.Fish.SprintInterval + Random.Range(-1f, 1.5f);
                }
            }
        }

        private float GetFishPullSpeed(FishRodLevelConfig rod, float staminaRatio)
        {
            var levelGap = Mathf.Max(0f, _fight.Fish.Level - rod.RecommendFishLevelMax);
            var recommendRodGap = Mathf.Max(0f, _fight.Fish.RecommendRodLevel - rod.Level);
            var rodGap = Mathf.Max(levelGap, recommendRodGap);
            var controlPenalty = Mathf.Max(0.55f, (_fight.Resistance / Mathf.Max(0.35f, rod.ControlPower)) - rod.EscapeMitigation + 0.55f);
            var speed = _fight.EscapeSpeed * controlPenalty * (0.65f + staminaRatio * 0.75f);
            speed *= 1f + rodGap * 0.22f;
            if (_fight.IsSprinting)
            {
                speed *= 1.35f;
            }

            return speed;
        }

        private static bool SupportsBait(FishRodLevelConfig rod, FishBaitConfig bait)
        {
            if (rod == null || bait == null)
            {
                return false;
            }

            return FishBaitTypeMask.Supports(rod.SupportedBaitTypeMask, bait.BaitType)
                && bait.Quality <= rod.SupportedBaitQualityMax
                && rod.Level >= bait.UnlockRodLevel;
        }

        private static bool HasTagOverlap(string[] left, string[] right)
        {
            if (left == null || right == null || left.Length == 0 || right.Length == 0)
            {
                return false;
            }

            for (var i = 0; i < left.Length; i++)
            {
                for (var j = 0; j < right.Length; j++)
                {
                    if (!string.IsNullOrEmpty(left[i]) && left[i] == right[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static float GetCastChargeRatio(float holdDuration)
        {
            return Mathf.Clamp01(holdDuration / MaxCastChargeDuration);
        }

        private float GetCastLineLength(float holdDuration)
        {
            var global = _configManager?.GlobalConfig;
            var min = global != null ? global.MinCastDistance : 5f;
            var max = global != null ? global.MaxCastDistance : 40f;
            return Mathf.Lerp(min, max, GetCastChargeRatio(holdDuration));
        }

        private static Color GetRarityColor(string rarity)
        {
            return rarity switch
            {
                "legendary" => new Color(0.76f, 0.47f, 1f),
                "rare" => new Color(1f, 0.8f, 0.25f),
                "uncommon" => new Color(0.53f, 0.92f, 0.63f),
                _ => Color.white
            };
        }
    }
}
