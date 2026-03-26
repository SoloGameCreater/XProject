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

        private readonly FishGameMainUI _view;
        private readonly RodConfig _rod = new RodConfig
        {
            Name = "Basic Rod",
            TensionMax = 60f,
            FatigueMultiplier = 1f,
            ReelSpeed = 1.5f
        };

        private readonly LureConfig[] _lures =
        {
            new LureConfig { Name = "Basic Mayfly", TargetTag = "any", Unlocked = true },
            new LureConfig { Name = "Bass Popper", TargetTag = "bass", Unlocked = false },
            new LureConfig { Name = "Hybrid Sturgeon", TargetTag = "sturgeon", Unlocked = false },
            new LureConfig { Name = "Salmon Egg", TargetTag = "salmon", Unlocked = false },
            new LureConfig { Name = "Trout Bugger", TargetTag = "trout", Unlocked = false }
        };

        private readonly FishDefinition[] _fishDefinitions =
        {
            new FishDefinition { Species = "Rock Bass", Rarity = "common", Stamina = 40f, FightPower = 15f, SprintInterval = 6f, SprintDuration = 1.5f, WeightRange = new Vector2(0.3f, 1.2f), LureTag = "bass", SellPricePerKg = 5 },
            new FishDefinition { Species = "Largemouth Bass", Rarity = "common", Stamina = 60f, FightPower = 20f, SprintInterval = 5f, SprintDuration = 2f, WeightRange = new Vector2(0.8f, 3f), LureTag = "bass", SellPricePerKg = 8 },
            new FishDefinition { Species = "Smallmouth Bass", Rarity = "common", Stamina = 55f, FightPower = 18f, SprintInterval = 5f, SprintDuration = 1.8f, WeightRange = new Vector2(0.5f, 2.5f), LureTag = "bass", SellPricePerKg = 7 },
            new FishDefinition { Species = "Rainbow Trout", Rarity = "common", Stamina = 50f, FightPower = 22f, SprintInterval = 4f, SprintDuration = 2f, WeightRange = new Vector2(0.4f, 2f), LureTag = "trout", SellPricePerKg = 9 },
            new FishDefinition { Species = "Bull Trout", Rarity = "uncommon", Stamina = 80f, FightPower = 28f, SprintInterval = 4f, SprintDuration = 2.5f, WeightRange = new Vector2(1f, 5f), LureTag = "trout", SellPricePerKg = 12 },
            new FishDefinition { Species = "Lake Trout", Rarity = "uncommon", Stamina = 85f, FightPower = 30f, SprintInterval = 4f, SprintDuration = 2.5f, WeightRange = new Vector2(1.5f, 6f), LureTag = "trout", SellPricePerKg = 13 },
            new FishDefinition { Species = "Golden Trout", Rarity = "rare", Stamina = 100f, FightPower = 35f, SprintInterval = 3f, SprintDuration = 3f, WeightRange = new Vector2(0.5f, 2.5f), LureTag = "trout", SellPricePerKg = 25 },
            new FishDefinition { Species = "Kokanee Salmon", Rarity = "uncommon", Stamina = 75f, FightPower = 26f, SprintInterval = 4f, SprintDuration = 2f, WeightRange = new Vector2(0.8f, 3.5f), LureTag = "salmon", SellPricePerKg = 11 },
            new FishDefinition { Species = "Chinook Salmon", Rarity = "rare", Stamina = 120f, FightPower = 40f, SprintInterval = 3f, SprintDuration = 3.5f, WeightRange = new Vector2(3f, 12f), LureTag = "salmon", SellPricePerKg = 20 },
            new FishDefinition { Species = "Arctic Grayling", Rarity = "uncommon", Stamina = 65f, FightPower = 24f, SprintInterval = 5f, SprintDuration = 2f, WeightRange = new Vector2(0.5f, 2f), LureTag = "salmon", SellPricePerKg = 10 },
            new FishDefinition { Species = "Paddlefish Sturgeon", Rarity = "rare", Stamina = 150f, FightPower = 45f, SprintInterval = 4f, SprintDuration = 4f, WeightRange = new Vector2(5f, 20f), LureTag = "sturgeon", SellPricePerKg = 22 },
            new FishDefinition { Species = "Pallid Sturgeon", Rarity = "legendary", Stamina = 200f, FightPower = 55f, SprintInterval = 3f, SprintDuration = 5f, WeightRange = new Vector2(8f, 30f), LureTag = "sturgeon", SellPricePerKg = 35 }
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
            ShowNotification("钓鱼系统已加载，点击按钮开始。", 3f, Color.white);
            RefreshUi();
        }

        public void Tick(float deltaTime)
        {
            UpdateNotification(deltaTime);
            UpdateStateTimers(deltaTime);
            UpdateFight(deltaTime);
            RefreshUi();
        }

        public void OnCastClicked()
        {
            if (_state == FishingState.Idle)
            {
                _state = FishingState.Casting;
                _castingTimer = 0.8f;
                ShowNotification("抛竿中...", 1f, Color.white);
                return;
            }

            if (_state == FishingState.Waiting)
            {
                _state = FishingState.Idle;
                ShowNotification("已收竿。", 1.2f, Color.white);
            }
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
            if (_isSwitching)
            {
                return;
            }

            _isSwitching = true;
            ShowNotification("正在切换到 TripleMerge...", 2f, new Color(1f, 0.84f, 0.35f));
            DebugUtil.Log("FishGamePresenter: 切换到 TripleMerge。");
            try
            {
                await GameplayDirector.Instance.EnterAsync(GameplayIds.TripleMerge);
            }
            finally
            {
                _isSwitching = false;
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
                    ShowNotification("鱼线已入水，等待咬钩...", 1.4f, new Color(0.75f, 0.9f, 1f));
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
            if (_state != FishingState.Fighting || _fight.Fish == null)
            {
                return;
            }

            var isReelPressed = _view != null && _view.ReelHoldButton.IsPressed;
            var isReleasePressed = _view != null && _view.ReleaseHoldButton.IsPressed;
            _fight.Reeling = isReelPressed && !isReleasePressed;
            _fight.Releasing = isReleasePressed && !isReelPressed;

            if (!_fight.IsSprinting)
            {
                _fight.SprintCooldown -= deltaTime;
                if (_fight.SprintCooldown <= 0f)
                {
                    _fight.IsSprinting = true;
                    _fight.SprintTimer = _fight.Fish.SprintDuration;
                    _fight.SprintDirection = 1;
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

                if (_fight.SprintDirection == 1)
                {
                    _fight.LineLength = Mathf.Min(_fight.LineLengthMax, _fight.LineLength + deltaTime * 4f);
                }

                _fight.Tension += _fight.Reeling
                    ? _fight.Fish.FightPower * 0.25f * deltaTime
                    : _fight.Fish.FightPower * 0.08f * deltaTime;
            }

            if (_fight.Reeling && !_fight.IsSprinting)
            {
                _fight.Stamina = Mathf.Max(0f, _fight.Stamina - _rod.ReelSpeed * _rod.FatigueMultiplier * deltaTime * 8f);
                var reelSpeed = _fight.Stamina <= 0f ? _rod.ReelSpeed * 2.5f : _rod.ReelSpeed;
                _fight.LineLength = Mathf.Max(_fight.LineLengthMin, _fight.LineLength - reelSpeed * deltaTime);
                _fight.Tension += (_fight.Stamina > 0f ? _fight.Fish.FightPower * 0.04f : _fight.Fish.FightPower * 0.005f) * deltaTime;
            }

            if (_fight.Releasing)
            {
                _fight.Tension = Mathf.Max(0f, _fight.Tension - 30f * deltaTime);
                _fight.LineLength = Mathf.Min(_fight.LineLengthMax, _fight.LineLength + 2f * deltaTime);
                _fight.Stamina = Mathf.Min(_fight.StaminaMax, _fight.Stamina + 3f * deltaTime);
            }

            if (!_fight.Reeling && !_fight.Releasing && !_fight.IsSprinting)
            {
                _fight.Tension = Mathf.Max(0f, _fight.Tension - 15f * deltaTime);
            }

            if (_fight.Tension >= _rod.TensionMax)
            {
                Fail("线断了！张力超过了鱼竿上限。");
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
            var suffix = lure.Unlocked ? string.Empty : "（未解锁，当前只做演示）";
            ShowNotification($"鱼饵切换为：{lure.Name}{suffix}", 1.8f, Color.white);
        }

        private void StartFight(FishDefinition fish)
        {
            if (fish == null)
            {
                Fail("没有找到可用鱼种。");
                return;
            }

            _fight.Fish = fish;
            _fight.Weight = Mathf.Round(UnityEngine.Random.Range(fish.WeightRange.x, fish.WeightRange.y) * 10f) / 10f;
            _fight.StaminaMax = fish.Stamina * (_fight.Weight / fish.WeightRange.y);
            _fight.Stamina = _fight.StaminaMax;
            _fight.Tension = 0f;
            _fight.LineLength = 20f;
            _fight.IsSprinting = false;
            _fight.SprintCooldown = fish.SprintInterval + UnityEngine.Random.Range(-1f, 2f);
            _state = FishingState.Fighting;
            ShowNotification($"咬钩！{fish.Species}（{fish.Rarity}，{_fight.Weight:F1} kg）", 3f, GetRarityColor(fish.Rarity));
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
            ShowNotification($"上鱼成功！{_fight.Fish.Species} {_fight.Weight:F1} kg，得分 +{price}", 3f, new Color(0.45f, 1f, 0.55f));
            DebugUtil.Log($"FishGamePresenter: 成功钓到 {_fight.Fish.Species}, weight={_fight.Weight:F1}, price={price}");
            ResetFight();
        }

        private void Fail(string reason)
        {
            ShowNotification(reason, 2.5f, new Color(1f, 0.45f, 0.45f));
            DebugUtil.LogWarning($"FishGamePresenter: 钓鱼失败, reason={reason}");
            ResetFight();
        }

        private void ResetFight()
        {
            _fight.Fish = null;
            _fight.Weight = 0f;
            _fight.Stamina = 0f;
            _fight.StaminaMax = 0f;
            _fight.Tension = 0f;
            _fight.LineLength = 20f;
            _fight.Reeling = false;
            _fight.Releasing = false;
            _fight.IsSprinting = false;
            _fight.SprintTimer = 0f;
            _fight.SprintCooldown = 0f;
            ResetHoldButtons();
            _state = FishingState.Idle;
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
                FishingState.Idle => "空闲",
                FishingState.Casting => "抛竿",
                FishingState.Waiting => "待鱼",
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
            _view.ScoreText.text = $"累计得分\n{_totalScore}";

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
                _view.LineText.text = $"当前目标 / {_fight.Fish.Species} / {_fight.Weight:F1} kg / 目标线距 {_fight.LineLengthMin:F1} m";
                _view.HintText.text = _fight.IsSprinting
                    ? "鱼正在冲刺，优先按住“放线”泄压，别急着硬收。"
                    : _fight.Stamina <= 0f
                        ? "鱼已经疲劳，稳住张力后持续“收线”即可完成上鱼。"
                        : "按住“收线”压低鱼体力，张力过高时改按“放线”缓冲。";
            }
            else
            {
                _view.SetBar(_view.StaminaFill, 0f);
                _view.SetBar(_view.TensionFill, 0f);
                _view.SetBar(_view.LineFill, 0f);
                _view.StaminaValueText.text = "--";
                _view.TensionValueText.text = "--";
                _view.LineValueText.text = "--";
                _view.LineText.text = "当前水域 / 河岸试钓区 / 抛竿后将自动进入等待咬钩阶段";
                _view.HintText.text = "点击“抛竿 / 收竿”开始，所有操作都通过底部控制台完成。";
            }

            _view.CastButton.interactable = _state == FishingState.Idle || _state == FishingState.Waiting;
            var canChangeLure = _state != FishingState.Fighting;
            _view.PrevLureButton.interactable = canChangeLure;
            _view.NextLureButton.interactable = canChangeLure;
            _view.ReelButton.gameObject.SetActive(_state == FishingState.Fighting);
            _view.ReleaseButton.gameObject.SetActive(_state == FishingState.Fighting);
            RefreshInventoryText();
        }

        private void RefreshInventoryText()
        {
            if (_inventory.Count == 0)
            {
                _view.InventoryText.text = "背包为空\n\n钓到的鱼会显示在这里。";
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("背包");
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
            if (_view == null)
            {
                return;
            }

            _view.ReelHoldButton.ResetState();
            _view.ReleaseHoldButton.ResetState();
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
