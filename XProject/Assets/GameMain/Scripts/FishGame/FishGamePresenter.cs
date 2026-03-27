using System.Collections.Generic;
using System.Text;
using GameplayRuntime;
using Framework;
using UnityEngine;

namespace FishGameRuntime
{
    public sealed class FishGamePresenter
    {
        private enum FishingState
        {
            Idle,
            Casting,
            Waiting,
            Fighting
        }

        private sealed class RodConfig
        {
            public string Name;
            public float TensionMax;
            public float FatigueMultiplier;
            public float ReelSpeed;
        }

        private sealed class LureConfig
        {
            public string Name;
            public string TargetTag;
            public bool Unlocked;
        }

        private sealed class FishDefinition
        {
            public string Species;
            public string Rarity;
            public float Stamina;
            public float FightPower;
            public float SprintInterval;
            public float SprintDuration;
            public Vector2 WeightRange;
            public string LureTag;
            public int SellPricePerKg;
        }

        private sealed class FightData
        {
            public FishDefinition Fish;
            public float Weight;
            public float Stamina;
            public float StaminaMax;
            public float Resistance;
            public float Tension;
            public float LineLength;
            public float LineLengthMax = 30f;
            public float LineLengthMin = 1.5f;
            public bool Reeling;
            public bool Releasing;
            public bool IsSprinting;
            public int SprintDirection;
            public float SprintTimer;
            public float SprintCooldown;
        }

        private sealed class InventoryEntry
        {
            public string Species;
            public float Weight;
            public int Price;
        }

        private const float DefaultCastLineLength = 20f;
        private const float MinCastLineLength = 5f;
        private const float MaxCastLineLength = 40f;
        private const float MaxCastChargeDuration = 1.5f;

        private readonly FishGameMainUI _view;
        private readonly RodConfig _rod = new RodConfig
        {
            Name = "基础鱼竿",
            TensionMax = 60f,
            FatigueMultiplier = 1f,
            ReelSpeed = 1.5f
        };

        private readonly LureConfig[] _lures =
        {
            new LureConfig { Name = "基础飞蝇", TargetTag = "any", Unlocked = true },
            new LureConfig { Name = "鲈鱼波趴", TargetTag = "bass", Unlocked = false },
            new LureConfig { Name = "鲟鱼复合饵", TargetTag = "sturgeon", Unlocked = false },
            new LureConfig { Name = "鲑鱼卵", TargetTag = "salmon", Unlocked = false },
            new LureConfig { Name = "鳟鱼毛钩", TargetTag = "trout", Unlocked = false }
        };

        private readonly FishDefinition[] _fishDefinitions =
        {
            new FishDefinition { Species = "岩鲈", Rarity = "common", Stamina = 40f, FightPower = 15f, SprintInterval = 6f, SprintDuration = 1.5f, WeightRange = new Vector2(0.3f, 1.2f), LureTag = "bass", SellPricePerKg = 5 },
            new FishDefinition { Species = "大口黑鲈", Rarity = "common", Stamina = 60f, FightPower = 20f, SprintInterval = 5f, SprintDuration = 2f, WeightRange = new Vector2(0.8f, 3f), LureTag = "bass", SellPricePerKg = 8 },
            new FishDefinition { Species = "小口黑鲈", Rarity = "common", Stamina = 55f, FightPower = 18f, SprintInterval = 5f, SprintDuration = 1.8f, WeightRange = new Vector2(0.5f, 2.5f), LureTag = "bass", SellPricePerKg = 7 },
            new FishDefinition { Species = "虹鳟", Rarity = "common", Stamina = 50f, FightPower = 22f, SprintInterval = 4f, SprintDuration = 2f, WeightRange = new Vector2(0.4f, 2f), LureTag = "trout", SellPricePerKg = 9 },
            new FishDefinition { Species = "公牛鳟", Rarity = "uncommon", Stamina = 80f, FightPower = 28f, SprintInterval = 4f, SprintDuration = 2.5f, WeightRange = new Vector2(1f, 5f), LureTag = "trout", SellPricePerKg = 12 },
            new FishDefinition { Species = "湖鳟", Rarity = "uncommon", Stamina = 85f, FightPower = 30f, SprintInterval = 4f, SprintDuration = 2.5f, WeightRange = new Vector2(1.5f, 6f), LureTag = "trout", SellPricePerKg = 13 },
            new FishDefinition { Species = "金鳟", Rarity = "rare", Stamina = 100f, FightPower = 35f, SprintInterval = 3f, SprintDuration = 3f, WeightRange = new Vector2(0.5f, 2.5f), LureTag = "trout", SellPricePerKg = 25 },
            new FishDefinition { Species = "红鲑", Rarity = "uncommon", Stamina = 75f, FightPower = 26f, SprintInterval = 4f, SprintDuration = 2f, WeightRange = new Vector2(0.8f, 3.5f), LureTag = "salmon", SellPricePerKg = 11 },
            new FishDefinition { Species = "帝王鲑", Rarity = "rare", Stamina = 120f, FightPower = 40f, SprintInterval = 3f, SprintDuration = 3.5f, WeightRange = new Vector2(3f, 12f), LureTag = "salmon", SellPricePerKg = 20 },
            new FishDefinition { Species = "北极茴鱼", Rarity = "uncommon", Stamina = 65f, FightPower = 24f, SprintInterval = 5f, SprintDuration = 2f, WeightRange = new Vector2(0.5f, 2f), LureTag = "salmon", SellPricePerKg = 10 },
            new FishDefinition { Species = "匙吻鲟", Rarity = "rare", Stamina = 150f, FightPower = 45f, SprintInterval = 4f, SprintDuration = 4f, WeightRange = new Vector2(5f, 20f), LureTag = "sturgeon", SellPricePerKg = 22 },
            new FishDefinition { Species = "淡白鲟", Rarity = "legendary", Stamina = 200f, FightPower = 55f, SprintInterval = 3f, SprintDuration = 5f, WeightRange = new Vector2(8f, 30f), LureTag = "sturgeon", SellPricePerKg = 35 }
        };

        private readonly Dictionary<string, int> _rarityWeight = new Dictionary<string, int>
        {
            { "common", 55 },
            { "uncommon", 30 },
            { "rare", 12 },
            { "legendary", 3 }
        };

        private readonly FightData _fight = new FightData();
        private readonly List<InventoryEntry> _inventory = new List<InventoryEntry>();

        private FishingState _state = FishingState.Idle;
        private int _activeLureIndex;
        private int _totalScore;
        private float _castingTimer;
        private float _waitTimer;
        private float _notificationTimer;
        private string _notificationMessage = string.Empty;
        private Color _notificationColor = Color.white;
        private bool _isSwitching;

        public FishGamePresenter(FishGameMainUI view)
        {
            _view = view;
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

            ResetFightDataForCast();
            _fight.LineLength = GetCastLineLength(holdDuration);
            _state = FishingState.Casting;
            _castingTimer = 0.8f;
            ShowNotification($"抛竿中... 本次抛投 {_fight.LineLength:F1} m", 1f, Color.white);
        }

        public void OnPrevLureClicked()
        {
            ChangeLure(-1);
        }

        public void OnNextLureClicked()
        {
            ChangeLure(1);
        }

        public async void OnSwitchClicked()
        {
            if (_state != FishingState.Idle || _isSwitching)
            {
                return;
            }

            _isSwitching = true;
            ShowNotification("正在切换到三消玩法...", 2f, new Color(1f, 0.84f, 0.35f));
            DebugUtil.Log("FishGamePresenter: switching to TripleMerge.");
            try
            {
                await GameplayDirector.Instance.EnterAsync(GameplayIds.TripleMerge);
            }
            finally
            {
                _isSwitching = false;
            }
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
                    _state = FishingState.Waiting;
                    var lureFactor = _lures[_activeLureIndex].TargetTag == "any" ? 1f : 0.7f;
                    _waitTimer = UnityEngine.Random.Range(2f, 8f) * lureFactor;
                    ShowNotification("鱼线已入水，按住鼠标左键可提前收线。", 1.6f, new Color(0.75f, 0.9f, 1f));
                }
            }

            if (_state == FishingState.Waiting)
            {
                _waitTimer -= deltaTime;
                if (_waitTimer <= 0f)
                {
                    StartFight(PickFish());
                }
            }
        }

        private void UpdateFight(float deltaTime)
        {
            var isPrimaryHeld = _view != null && _view.IsPrimaryHeld;

            if (_state == FishingState.Waiting)
            {
                _fight.Reeling = isPrimaryHeld;
                if (_fight.Reeling)
                {
                    _fight.LineLength = Mathf.Max(_fight.LineLengthMin, _fight.LineLength - _rod.ReelSpeed * 6f * deltaTime);
                    if (_fight.LineLength <= _fight.LineLengthMin + 0.05f)
                    {
                        ShowNotification("鱼竿已收回，返回待命。", 1.2f, Color.white);
                        ResetFight();
                    }
                }

                return;
            }

            if (_state != FishingState.Fighting || _fight.Fish == null)
            {
                return;
            }

            _fight.Reeling = isPrimaryHeld;

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
                    _fight.SprintCooldown = _fight.Fish.SprintInterval + UnityEngine.Random.Range(-1f, 2f);
                }
            }

            var basePullSpeed = Mathf.Lerp(0.45f, 1.75f, _fight.Resistance);
            var sprintPullBonus = _fight.IsSprinting ? Mathf.Lerp(1.5f, 3.5f, _fight.Resistance) : 0f;
            var fishPullSpeed = basePullSpeed + sprintPullBonus;

            if (_fight.Reeling)
            {
                var tiredBonus = _fight.Stamina <= 0f ? 1.8f : 1f;
                var reelSpeed = _rod.ReelSpeed * Mathf.Lerp(1.15f, 0.42f, _fight.Resistance) * tiredBonus;
                var netReelSpeed = Mathf.Max(0.08f, reelSpeed - fishPullSpeed * 0.35f);
                _fight.Stamina = Mathf.Max(0f, _fight.Stamina - _rod.ReelSpeed * _rod.FatigueMultiplier * deltaTime * Mathf.Lerp(8f, 4.5f, _fight.Resistance));
                _fight.LineLength = Mathf.Max(_fight.LineLengthMin, _fight.LineLength - netReelSpeed * deltaTime);
                _fight.Tension += (_fight.Fish.FightPower * Mathf.Lerp(0.045f, 0.115f, _fight.Resistance) + fishPullSpeed * 3.2f) * deltaTime;
            }
            else
            {
                _fight.LineLength = Mathf.Min(_fight.LineLengthMax, _fight.LineLength + fishPullSpeed * deltaTime);
                _fight.Tension = Mathf.Max(0f, _fight.Tension - Mathf.Lerp(20f, 12f, _fight.Resistance) * deltaTime);
                _fight.Stamina = Mathf.Min(_fight.StaminaMax, _fight.Stamina + Mathf.Lerp(1.2f, 2.8f, _fight.Resistance) * deltaTime);
            }

            if (_fight.Tension >= _rod.TensionMax)
            {
                Fail("鱼线断裂：张力超过上限。");
                return;
            }

            if (_fight.Stamina <= 0f && _fight.LineLength <= _fight.LineLengthMin + 0.5f)
            {
                CatchFish();
            }
        }

        private void ChangeLure(int direction)
        {
            _activeLureIndex += direction;
            if (_activeLureIndex < 0)
            {
                _activeLureIndex = _lures.Length - 1;
            }
            else if (_activeLureIndex >= _lures.Length)
            {
                _activeLureIndex = 0;
            }

            var lure = _lures[_activeLureIndex];
            var suffix = lure.Unlocked ? string.Empty : "（暂未解锁，仅演示）";
            ShowNotification($"已切换鱼饵：{lure.Name}{suffix}", 1.8f, Color.white);
        }

        private void StartFight(FishDefinition fish)
        {
            if (fish == null)
            {
                Fail("未找到可用的鱼配置。");
                return;
            }

            _fight.Fish = fish;
            _fight.Weight = Mathf.Round(UnityEngine.Random.Range(fish.WeightRange.x, fish.WeightRange.y) * 10f) / 10f;
            _fight.StaminaMax = fish.Stamina * (_fight.Weight / fish.WeightRange.y);
            _fight.Stamina = _fight.StaminaMax;
            _fight.Resistance = Mathf.Clamp01(_fight.Weight / 12f);
            _fight.Tension = 0f;
            _fight.LineLength = Mathf.Clamp(_fight.LineLength, _fight.LineLengthMin, _fight.LineLengthMax);
            _fight.IsSprinting = false;
            _fight.SprintDirection = 0;
            _fight.SprintTimer = 0f;
            _fight.SprintCooldown = fish.SprintInterval + UnityEngine.Random.Range(-1f, 2f);
            _fight.Reeling = false;
            _fight.Releasing = false;
            _state = FishingState.Fighting;
            _view?.ResetPointerState();
            ShowNotification($"鱼上钩了：{fish.Species} {_fight.Weight:F1} kg，注意控制张力。", 3f, GetRarityColor(fish.Rarity));
        }

        private void CatchFish()
        {
            var price = Mathf.FloorToInt(_fight.Weight * _fight.Fish.SellPricePerKg);
            _inventory.Add(new InventoryEntry
            {
                Species = _fight.Fish.Species,
                Weight = _fight.Weight,
                Price = price
            });
            _totalScore += price;
            ShowNotification($"成功钓起：{_fight.Fish.Species} {_fight.Weight:F1} kg，积分 +{price}", 3f, new Color(0.45f, 1f, 0.55f));
            DebugUtil.Log($"FishGamePresenter: caught {_fight.Fish.Species}, weight={_fight.Weight:F1}, price={price}");
            ResetFight();
        }

        private void Fail(string reason)
        {
            ShowNotification(reason, 2.5f, new Color(1f, 0.45f, 0.45f));
            DebugUtil.LogWarning($"FishGamePresenter: fishing failed, reason={reason}");
            ResetFight();
        }

        private void ResetFight()
        {
            ResetFightDataForCast();
            _state = FishingState.Idle;
        }

        private void ResetFightDataForCast()
        {
            _fight.Fish = null;
            _fight.Weight = 0f;
            _fight.Stamina = 0f;
            _fight.StaminaMax = 0f;
            _fight.Resistance = 0f;
            _fight.Tension = 0f;
            _fight.LineLengthMax = MaxCastLineLength;
            _fight.LineLength = DefaultCastLineLength;
            _fight.Reeling = false;
            _fight.Releasing = false;
            _fight.IsSprinting = false;
            _fight.SprintDirection = 0;
            _fight.SprintTimer = 0f;
            _fight.SprintCooldown = 0f;
            _view?.ResetPointerState();
        }

        private FishDefinition PickFish()
        {
            var rarity = PickRarity();
            var lure = _lures[_activeLureIndex];
            var pool = new List<FishDefinition>();
            for (var i = 0; i < _fishDefinitions.Length; i++)
            {
                if (_fishDefinitions[i].Rarity == rarity)
                {
                    pool.Add(_fishDefinitions[i]);
                }
            }

            if (pool.Count == 0)
            {
                pool.AddRange(_fishDefinitions);
            }

            var weightedPool = new List<FishDefinition>();
            for (var i = 0; i < pool.Count; i++)
            {
                var weight = lure.TargetTag == "any" || pool[i].LureTag == lure.TargetTag ? 3 : 1;
                for (var j = 0; j < weight; j++)
                {
                    weightedPool.Add(pool[i]);
                }
            }

            return weightedPool[UnityEngine.Random.Range(0, weightedPool.Count)];
        }

        private string PickRarity()
        {
            var totalWeight = 0;
            foreach (var item in _rarityWeight)
            {
                totalWeight += item.Value;
            }

            var randomValue = UnityEngine.Random.Range(0, totalWeight);
            var currentWeight = 0;
            foreach (var item in _rarityWeight)
            {
                currentWeight += item.Value;
                if (randomValue < currentWeight)
                {
                    return item.Key;
                }
            }

            return "common";
        }

        private void RefreshUi()
        {
            if (_view == null)
            {
                return;
            }

            _view.StateText.text = _state switch
            {
                FishingState.Idle => "待命",
                FishingState.Casting => "抛竿",
                FishingState.Waiting => "等待",
                FishingState.Fighting => "遛鱼",
                _ => "未知"
            };
            _view.StateText.color = _state switch
            {
                FishingState.Casting => new Color(1f, 0.84f, 0.35f),
                FishingState.Waiting => new Color(0.72f, 0.9f, 1f),
                FishingState.Fighting => new Color(0.45f, 1f, 0.55f),
                _ => Color.white
            };

            var lure = _lures[_activeLureIndex];
            _view.EquipText.text = $"装备配置 / {_rod.Name} / {lure.Name}";
            _view.ScoreText.text = $"积分\n{_totalScore}";

            if (_state == FishingState.Fighting && _fight.Fish != null)
            {
                _view.SetBar(_view.StaminaFill, _fight.StaminaMax <= 0f ? 0f : _fight.Stamina / _fight.StaminaMax);
                var tensionRatio = _fight.Tension / _rod.TensionMax;
                _view.SetBar(_view.TensionFill, tensionRatio);
                _view.TensionFill.color = tensionRatio switch
                {
                    < 0.6f => new Color(0.31f, 0.78f, 0.31f),
                    < 0.85f => new Color(0.95f, 0.72f, 0.12f),
                    _ => new Color(0.86f, 0.26f, 0.26f)
                };
                _view.SetBar(_view.LineFill, 1f - Mathf.Clamp01((_fight.LineLength - _fight.LineLengthMin) / (_fight.LineLengthMax - _fight.LineLengthMin)));
                _view.LineFill.color = _fight.Stamina <= 0f ? new Color(1f, 0.82f, 0.15f) : new Color(0.31f, 0.63f, 1f);
                _view.StaminaValueText.text = $"{_fight.Stamina:F0} / {_fight.StaminaMax:F0}";
                _view.TensionValueText.text = $"{_fight.Tension:F0} / {_rod.TensionMax:F0}";
                _view.LineValueText.text = $"{_fight.LineLength:F1} m";
                _view.LineText.text = $"目标鱼 / {_fight.Fish.Species} / {_fight.Weight:F1} kg / 阻力 {_fight.Resistance * 100f:F0}% / 收线目标 {_fight.LineLengthMin:F1} m";
                _view.HintText.text = _fight.IsSprinting
                    ? "鱼正在发力外冲，松开鼠标左键，让鱼把线带出去。"
                    : _fight.Stamina <= 0f
                        ? "鱼已经乏力，按住鼠标左键把它拉回来。"
                        : "按住鼠标左键收线；松开后不会主动放线，只有鱼发力时才会被带线。";
            }
            else if (_state == FishingState.Casting || _state == FishingState.Waiting)
            {
                _view.SetBar(_view.StaminaFill, 0f);
                _view.SetBar(_view.TensionFill, 0f);
                _view.SetBar(_view.LineFill, 1f - Mathf.Clamp01((_fight.LineLength - _fight.LineLengthMin) / (_fight.LineLengthMax - _fight.LineLengthMin)));
                _view.LineFill.color = new Color(0.31f, 0.63f, 1f);
                _view.StaminaValueText.text = "--";
                _view.TensionValueText.text = $"0 / {_rod.TensionMax:F0}";
                _view.LineValueText.text = $"{_fight.LineLength:F1} m";
                _view.LineText.text = _state == FishingState.Casting
                    ? "抛竿动作进行中。"
                    : $"等待咬钩 / 当前线长 {_fight.LineLength:F1} m / 按住鼠标左键可提前收线。";
                _view.HintText.text = _state == FishingState.Casting
                    ? "鱼线落水后会自动进入等待阶段。"
                    : "等待阶段按住鼠标左键会持续收线，松开不会主动放线。";
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
                    : "待命 / 按住中央抛竿区蓄力，松开鼠标左键抛竿。";
                _view.HintText.text = isChargingCast
                    ? "按住越久抛得越远，松开后立即抛竿。"
                    : "待命时按住中央抛竿区蓄力抛竿；最短 5 m，最长 40 m。";
            }

            _view.ApplyInputMode(_state == FishingState.Idle);
            _view.UpdateCastZoneChargeDisplay(currentDistance: GetCastLineLength(_view.CastZoneHoldDuration), chargeRatio: GetCastChargeRatio(_view.CastZoneHoldDuration), isCharging: _state == FishingState.Idle && _view.IsCastZonePressActive);
            RefreshInventoryText();
        }

        private void RefreshInventoryText()
        {
            if (_inventory.Count == 0)
            {
                _view.InventoryText.text = "鱼篓为空\n\n钓到的鱼会显示在这里。";
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("鱼篓");
            builder.AppendLine();
            var totalPrice = 0;
            for (var i = 0; i < _inventory.Count; i++)
            {
                totalPrice += _inventory[i].Price;
                builder.AppendLine($"{i + 1}. {_inventory[i].Species}  {_inventory[i].Weight:F1} kg  ${_inventory[i].Price}");
            }

            builder.AppendLine();
            builder.AppendLine($"总价值：${totalPrice}");
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

        private void ResetHoldButtons()
        {
            _view?.ResetPointerState();
        }

        private static float GetCastChargeRatio(float holdDuration)
        {
            return Mathf.Clamp01(holdDuration / MaxCastChargeDuration);
        }

        private static float GetCastLineLength(float holdDuration)
        {
            return Mathf.Lerp(MinCastLineLength, MaxCastLineLength, GetCastChargeRatio(holdDuration));
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
