#if false
using System;
using System.Collections.Generic;
using System.Text;
using GameplayRuntime;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FishGameRuntime
{
    public class LegacyFishGameController : MonoBehaviour
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
        private bool _isSwitching;
        private Font _font;

        private Text _stateText;
        private Text _equipText;
        private Text _scoreText;
        private Text _lineText;
        private Text _hintText;
        private Text _notifyText;
        private Text _inventoryText;
        private Image _staminaFill;
        private Image _tensionFill;
        private Image _lineFill;
        private Button _castButton;
        private Button _prevLureButton;
        private Button _nextLureButton;
        private Button _switchButton;
        private FishGameHoldButton _reelButton;
        private FishGameHoldButton _releaseButton;

        public void Initialize()
        {
            _font = LoadRuntimeFont();
            BuildUi();
            Notify("钓鱼系统已加载，点击按钮开始。", 3f, Color.white);
            DebugUtil.Log("FishGameController: 钓鱼玩法 UI 创建完成。");
        }

        private void Update()
        {
            UpdateStateTimers(Time.deltaTime);
            UpdateFight(Time.deltaTime);
            RefreshUi();
        }

        private void OnDestroy()
        {
            if (_castButton != null)
            {
                _castButton.onClick.RemoveListener(OnCastClicked);
            }

            if (_prevLureButton != null)
            {
                _prevLureButton.onClick.RemoveListener(OnPrevLureClicked);
            }

            if (_nextLureButton != null)
            {
                _nextLureButton.onClick.RemoveListener(OnNextLureClicked);
            }

            if (_switchButton != null)
            {
                _switchButton.onClick.RemoveListener(OnSwitchClicked);
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
                    Notify("鱼线已入水，等待咬钩...", 1.4f, new Color(0.75f, 0.9f, 1f));
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

            _fight.Reeling = _reelButton != null && _reelButton.IsPressed;
            _fight.Releasing = _releaseButton != null && _releaseButton.IsPressed;

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

        private void BuildUi()
        {
            var rootRect = gameObject.AddComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            var background = gameObject.AddComponent<Image>();
            background.color = new Color(0.05f, 0.12f, 0.18f, 0.9f);

            var title = CreateText("Title", transform, "Fish Game", 42, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetRect((RectTransform)title.transform, new Vector2(24f, -24f), new Vector2(360f, 54f), new Vector2(0f, 1f));

            var hudPanel = CreatePanel("HudPanel", transform, new Color(0f, 0f, 0f, 0.28f));
            Stretch(hudPanel, new Vector2(24f, 180f), new Vector2(408f, 24f));

            _stateText = CreateText("StateText", hudPanel, string.Empty, 28, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetRect((RectTransform)_stateText.transform, new Vector2(20f, -20f), new Vector2(340f, 32f), new Vector2(0f, 1f));

            _equipText = CreateText("EquipText", hudPanel, string.Empty, 22, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetRect((RectTransform)_equipText.transform, new Vector2(20f, -60f), new Vector2(580f, 28f), new Vector2(0f, 1f));

            _scoreText = CreateText("ScoreText", hudPanel, string.Empty, 22, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetRect((RectTransform)_scoreText.transform, new Vector2(20f, -96f), new Vector2(360f, 28f), new Vector2(0f, 1f));

            var staminaLabel = CreateText("StaminaLabel", hudPanel, "鱼体力", 20, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetRect((RectTransform)staminaLabel.transform, new Vector2(20f, -146f), new Vector2(120f, 24f), new Vector2(0f, 1f));
            _staminaFill = CreateBar("StaminaBar", hudPanel, new Vector2(20f, -174f), new Color(0.31f, 0.78f, 0.31f));

            var tensionLabel = CreateText("TensionLabel", hudPanel, "线张力", 20, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetRect((RectTransform)tensionLabel.transform, new Vector2(20f, -222f), new Vector2(120f, 24f), new Vector2(0f, 1f));
            _tensionFill = CreateBar("TensionBar", hudPanel, new Vector2(20f, -250f), new Color(0.86f, 0.26f, 0.26f));

            _lineText = CreateText("LineText", hudPanel, string.Empty, 20, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetRect((RectTransform)_lineText.transform, new Vector2(20f, -298f), new Vector2(360f, 24f), new Vector2(0f, 1f));
            _lineFill = CreateBar("LineBar", hudPanel, new Vector2(20f, -326f), new Color(0.31f, 0.63f, 1f));

            _hintText = CreateText("HintText", hudPanel, string.Empty, 22, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetRect((RectTransform)_hintText.transform, new Vector2(20f, -378f), new Vector2(720f, 52f), new Vector2(0f, 1f));

            var inventoryPanel = CreatePanel("InventoryPanel", transform, new Color(0f, 0f, 0f, 0.32f));
            Stretch(inventoryPanel, new Vector2(1536f, 180f), new Vector2(24f, 24f));
            var inventoryTitle = CreateText("InventoryTitle", inventoryPanel, "背包", 28, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetRect((RectTransform)inventoryTitle.transform, new Vector2(20f, -20f), new Vector2(120f, 32f), new Vector2(0f, 1f));
            _inventoryText = CreateText("InventoryText", inventoryPanel, "背包为空", 20, TextAnchor.UpperLeft, FontStyle.Normal);
            Stretch((RectTransform)_inventoryText.transform, new Vector2(20f, 20f), new Vector2(20f, 64f));

            var notifyPanel = CreatePanel("NotifyPanel", transform, new Color(0f, 0f, 0f, 0.45f));
            SetRect(notifyPanel, new Vector2(0f, -120f), new Vector2(640f, 60f), new Vector2(0.5f, 1f));
            _notifyText = CreateText("NotifyText", notifyPanel, string.Empty, 24, TextAnchor.MiddleCenter, FontStyle.Bold);
            Stretch((RectTransform)_notifyText.transform, new Vector2(12f, 8f), new Vector2(12f, 8f));

            var controlPanel = CreatePanel("ControlPanel", transform, new Color(0f, 0f, 0f, 0.32f));
            SetRect(controlPanel, new Vector2(24f, 24f), new Vector2(1180f, 120f), new Vector2(0f, 0f));

            _castButton = CreateButton("CastButton", controlPanel, "抛竿 / 收竿", new Vector2(24f, 20f), new Vector2(170f, 40f));
            _prevLureButton = CreateButton("PrevLureButton", controlPanel, "上一种鱼饵", new Vector2(214f, 20f), new Vector2(150f, 40f));
            _nextLureButton = CreateButton("NextLureButton", controlPanel, "下一种鱼饵", new Vector2(384f, 20f), new Vector2(150f, 40f));
            _switchButton = CreateButton("SwitchButton", controlPanel, "进入三消", new Vector2(554f, 20f), new Vector2(180f, 40f), new Color(0.85f, 0.49f, 0.13f));
            _reelButton = CreateHoldButton("ReelButton", controlPanel, "按住收线", new Vector2(24f, 70f), new Vector2(260f, 34f), new Color(0.14f, 0.49f, 0.21f));
            _releaseButton = CreateHoldButton("ReleaseButton", controlPanel, "按住放线", new Vector2(304f, 70f), new Vector2(260f, 34f), new Color(0.16f, 0.32f, 0.63f));

            _castButton.onClick.AddListener(OnCastClicked);
            _prevLureButton.onClick.AddListener(OnPrevLureClicked);
            _nextLureButton.onClick.AddListener(OnNextLureClicked);
            _switchButton.onClick.AddListener(OnSwitchClicked);
        }

        private void OnCastClicked()
        {
            if (_state == FishingState.Idle)
            {
                _state = FishingState.Casting;
                _castingTimer = 0.8f;
                Notify("抛竿中...", 1f, Color.white);
                return;
            }

            if (_state == FishingState.Waiting)
            {
                _state = FishingState.Idle;
                Notify("已收竿。", 1.2f, Color.white);
            }
        }

        private void OnPrevLureClicked()
        {
            ChangeLure(-1);
        }

        private void OnNextLureClicked()
        {
            ChangeLure(1);
        }

        private async void OnSwitchClicked()
        {
            if (_isSwitching)
            {
                return;
            }

            _isSwitching = true;
            Notify("正在切换到 TripleMerge...", 2f, new Color(1f, 0.84f, 0.35f));
            DebugUtil.Log("FishGameController: 切换到 TripleMerge。");
            await GameplayDirector.Instance.EnterAsync(GameplayIds.TripleMerge);
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
            Notify($"鱼饵切换为：{lure.Name}{suffix}", 1.8f, Color.white);
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
            Notify($"咬钩！{fish.Species}（{fish.Rarity}，{_fight.Weight:F1} kg）", 3f, GetRarityColor(fish.Rarity));
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
            Notify($"上鱼成功！{_fight.Fish.Species} {_fight.Weight:F1} kg，得分 +{price}", 3f, new Color(0.45f, 1f, 0.55f));
            DebugUtil.Log($"FishGameController: 成功钓到 {_fight.Fish.Species}, weight={_fight.Weight:F1}, price={price}");
            ResetFight();
        }

        private void Fail(string reason)
        {
            Notify(reason, 2.5f, new Color(1f, 0.45f, 0.45f));
            DebugUtil.LogWarning($"FishGameController: 钓鱼失败, reason={reason}");
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
            if (_reelButton != null) _reelButton.SetPressed(false);
            if (_releaseButton != null) _releaseButton.SetPressed(false);
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
            _stateText.text = _state switch
            {
                FishingState.Idle => "状态：空闲",
                FishingState.Casting => "状态：抛竿中",
                FishingState.Waiting => "状态：等待咬钩",
                FishingState.Fighting => "状态：遛鱼中",
                _ => "状态：未知"
            };

            var lure = _lures[_activeLureIndex];
            _equipText.text = $"鱼竿：{_rod.Name}    鱼饵：{lure.Name}";
            _scoreText.text = $"得分：{_totalScore}";

            if (_state == FishingState.Fighting && _fight.Fish != null)
            {
                SetBar(_staminaFill, _fight.StaminaMax <= 0f ? 0f : _fight.Stamina / _fight.StaminaMax);
                var tensionRatio = _fight.Tension / _rod.TensionMax;
                SetBar(_tensionFill, tensionRatio);
                _tensionFill.color = tensionRatio switch
                {
                    < 0.6f => new Color(0.31f, 0.78f, 0.31f),
                    < 0.85f => new Color(0.95f, 0.72f, 0.12f),
                    _ => new Color(0.86f, 0.26f, 0.26f)
                };
                SetBar(_lineFill, 1f - Mathf.Clamp01((_fight.LineLength - _fight.LineLengthMin) / (_fight.LineLengthMax - _fight.LineLengthMin)));
                _lineFill.color = _fight.Stamina <= 0f ? new Color(1f, 0.82f, 0.15f) : new Color(0.31f, 0.63f, 1f);
                _lineText.text = $"收线距离：{_fight.LineLength:F1} m    目标：{_fight.LineLengthMin:F1} m";
                _hintText.text = _fight.IsSprinting
                    ? "鱼在冲刺，按住“放线”降低张力。"
                    : _fight.Stamina <= 0f
                        ? "鱼已疲劳，继续按住“收线”即可上鱼。"
                        : "按住“收线”消耗体力，按住“放线”降低张力。";
            }
            else
            {
                SetBar(_staminaFill, 0f);
                SetBar(_tensionFill, 0f);
                SetBar(_lineFill, 0f);
                _lineText.text = "收线距离：--";
                _hintText.text = "点击“抛竿 / 收竿”开始，所有交互都通过 UI 完成。";
            }

            _castButton.interactable = _state == FishingState.Idle || _state == FishingState.Waiting;
            var canChangeLure = _state != FishingState.Fighting;
            _prevLureButton.interactable = canChangeLure;
            _nextLureButton.interactable = canChangeLure;
            _reelButton.gameObject.SetActive(_state == FishingState.Fighting);
            _releaseButton.gameObject.SetActive(_state == FishingState.Fighting);
            RefreshInventoryText();
        }

        private void RefreshInventoryText()
        {
            if (_inventory.Count == 0)
            {
                _inventoryText.text = "背包为空\n\n钓到的鱼会显示在这里。";
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
            _inventoryText.text = builder.ToString();
        }

        private void Notify(string message, float duration, Color color)
        {
            _notifyText.text = message;
            _notifyText.color = color;
            CancelInvoke(nameof(ClearNotify));
            Invoke(nameof(ClearNotify), duration);
        }

        private void ClearNotify()
        {
            if (_notifyText != null)
            {
                _notifyText.text = string.Empty;
            }
        }

        private static Font LoadRuntimeFont()
        {
            try
            {
                var builtinFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                if (builtinFont != null)
                {
                    return builtinFont;
                }
            }
            catch (Exception e)
            {
                DebugUtil.LogWarning($"FishGameController: 加载内置字体失败，error={e.Message}");
            }

            return Font.CreateDynamicFontFromOSFont(new[] { "Arial", "Microsoft YaHei", "Segoe UI" }, 18);
        }

        private Text CreateText(string name, Transform parent, string content, int fontSize, TextAnchor alignment, FontStyle fontStyle)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textObject.transform.SetParent(parent, false);
            var text = textObject.GetComponent<Text>();
            text.font = _font;
            text.text = content;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.fontStyle = fontStyle;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private RectTransform CreatePanel(string name, Transform parent, Color color)
        {
            var panelObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panelObject.transform.SetParent(parent, false);
            panelObject.GetComponent<Image>().color = color;
            return panelObject.GetComponent<RectTransform>();
        }

        private Image CreateBar(string name, Transform parent, Vector2 anchoredPosition, Color color)
        {
            var background = CreatePanel(name, parent, new Color(1f, 1f, 1f, 0.12f));
            SetRect(background, anchoredPosition, new Vector2(320f, 18f), new Vector2(0f, 1f));

            var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            fillObject.transform.SetParent(background, false);
            var fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(0f, 1f);
            fillRect.pivot = new Vector2(0f, 0.5f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fillRect.sizeDelta = new Vector2(0f, 0f);
            var fill = fillObject.GetComponent<Image>();
            fill.color = color;
            return fill;
        }

        private Button CreateButton(string name, Transform parent, string text, Vector2 anchoredPosition, Vector2 size, Color? color = null)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            SetRect(rect, anchoredPosition, size, new Vector2(0f, 0f));
            var image = buttonObject.GetComponent<Image>();
            image.color = color ?? new Color(0.18f, 0.47f, 0.72f);
            var button = buttonObject.GetComponent<Button>();
            var label = CreateText("Label", buttonObject.transform, text, 18, TextAnchor.MiddleCenter, FontStyle.Bold);
            Stretch((RectTransform)label.transform, new Vector2(8f, 4f), new Vector2(8f, 4f));
            var colors = button.colors;
            colors.normalColor = image.color;
            colors.highlightedColor = image.color * 1.08f;
            colors.pressedColor = image.color * 0.9f;
            colors.selectedColor = image.color;
            colors.disabledColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            button.colors = colors;
            return button;
        }

        private FishGameHoldButton CreateHoldButton(string name, Transform parent, string text, Vector2 anchoredPosition, Vector2 size, Color color)
        {
            var button = CreateButton(name, parent, text, anchoredPosition, size, color);
            return button.gameObject.AddComponent<FishGameHoldButton>();
        }

        private void SetRect(RectTransform rect, Vector2 anchoredPosition, Vector2 size, Vector2 anchor)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
        }

        private void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = offsetMin;
            rect.offsetMax = -offsetMax;
        }

        private void SetBar(Image fill, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            ((RectTransform)fill.transform).sizeDelta = new Vector2(320f * ratio, 0f);
        }

        private Color GetRarityColor(string rarity)
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

    internal sealed class FishGameHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public bool IsPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPressed = false;
        }

        public void SetPressed(bool pressed)
        {
            IsPressed = pressed;
        }
    }
}
#endif
