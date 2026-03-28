using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace FishGameRuntime
{
    [AssetAddress("UIFishGame/FishGameMainUI")]
    public class FishGameMainUI : UIView
    {
        private const float CastZoneWidthRatio = 1280f / 1920f;
        private const float CastZoneHeightRatio = 720f / 1080f;

        [ComponentBinder("HeaderPanel/StateBadge/StateText")] private Text _stateText;
        [ComponentBinder("HeaderPanel/TitleGroup/EquipText")] private Text _equipText;
        [ComponentBinder("HeaderPanel/ScoreCard/ScoreText")] private Text _scoreText;
        [ComponentBinder("StatusPanel/WaterCard/LineText")] private Text _lineText;
        [ComponentBinder("StatusPanel/WaterCard/HintText")] private Text _hintText;
        [ComponentBinder("StatusPanel/WaterCard/NotifyPanel/NotifyText")] private Text _notifyText;
        [ComponentBinder("InventoryPanel/Body/InventoryText")] private Text _inventoryText;
        [ComponentBinder("HudPanel/StaminaRow/ValueText")] private Text _staminaValueText;
        [ComponentBinder("HudPanel/TensionRow/ValueText")] private Text _tensionValueText;
        [ComponentBinder("HudPanel/LineRow/ValueText")] private Text _lineValueText;
        [ComponentBinder("ControlsPanel/ActionHint")] private Text _actionHintText;

        [ComponentBinder("ControlsPanel/PrimaryActions/CastButton")] private Button _sellAllButton;
        [ComponentBinder("ControlsPanel/LureActions/PrevLureButton")] private Button _prevLureButton;
        [ComponentBinder("ControlsPanel/LureActions/NextLureButton")] private Button _nextLureButton;
        [ComponentBinder("ControlsPanel/PrimaryActions/SwitchButton")] private Button _switchButton;
        [ComponentBinder("ControlsPanel/FightActions/ReelButton")] private Button _buyBaitButton;
        [ComponentBinder("ControlsPanel/FightActions/ReleaseButton")] private Button _upgradeRodButton;
        [ComponentBinder("ControlsPanel/PrimaryActions")] private Transform _primaryActionsRoot;
        [ComponentBinder("ControlsPanel/LureActions")] private Transform _lureActionsRoot;
        [ComponentBinder("ControlsPanel/FightActions")] private Transform _shopActionsRoot;

        [ComponentBinder("HudPanel/StaminaRow/StaminaBar/Fill")] private Image _staminaFill;
        [ComponentBinder("HudPanel/TensionRow/TensionBar/Fill")] private Image _tensionFill;
        [ComponentBinder("HudPanel/LineRow/LineBar/Fill")] private Image _lineFill;

        private FishGamePresenterV2 _presenter;
        private readonly List<Button> _idleOnlyButtons = new List<Button>();
        private readonly List<Button> _alwaysHiddenButtons = new List<Button>();
        private RectTransform _castZoneVisual;
        private Text _castZoneLabel;
        private bool _isIdleInputMode = true;
        private bool _primaryHeld;
        private bool _secondaryHeld;
        private bool _primaryPressedThisFrame;
        private bool _primaryReleasedThisFrame;
        private bool _castZoneClickedThisFrame;
        private float _castZoneHoldDuration;
        private bool _pendingCastPress;

        internal Text StateText => _stateText;
        internal Text EquipText => _equipText;
        internal Text ScoreText => _scoreText;
        internal Text LineText => _lineText;
        internal Text HintText => _hintText;
        internal Text NotifyText => _notifyText;
        internal Text InventoryText => _inventoryText;
        internal Text StaminaValueText => _staminaValueText;
        internal Text TensionValueText => _tensionValueText;
        internal Text LineValueText => _lineValueText;
        internal Text ActionHintText => _actionHintText;
        internal Button SellAllButton => _sellAllButton;
        internal Button PrevLureButton => _prevLureButton;
        internal Button NextLureButton => _nextLureButton;
        internal Button SwitchButton => _switchButton;
        internal Button BuyBaitButton => _buyBaitButton;
        internal Button UpgradeRodButton => _upgradeRodButton;
        internal Image StaminaFill => _staminaFill;
        internal Image TensionFill => _tensionFill;
        internal Image LineFill => _lineFill;
        internal bool IsPrimaryHeld => _primaryHeld;
        internal bool IsSecondaryHeld => _secondaryHeld;
        internal bool PrimaryPressedThisFrame => _primaryPressedThisFrame;
        internal bool PrimaryReleasedThisFrame => _primaryReleasedThisFrame;
        internal bool CastZoneClickedThisFrame => _castZoneClickedThisFrame;
        internal bool IsCastZonePressActive => _isIdleInputMode && _pendingCastPress && _primaryHeld;
        internal float CastZoneHoldDuration => _castZoneHoldDuration;

        public override UIViewLayer ViewLayer => UIViewLayer.Normal;

        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);

            BuildCastZoneVisual();
            RegisterButtons();
            ApplyInputMode(true);

            _presenter = new FishGamePresenterV2(this);

            _prevLureButton.onClick.AddListener(_presenter.OnPrevLureClicked);
            _nextLureButton.onClick.AddListener(_presenter.OnNextLureClicked);
            _switchButton.onClick.AddListener(_presenter.OnSwitchClicked);
            _sellAllButton.onClick.AddListener(_presenter.OnSellAllClicked);
            _buyBaitButton.onClick.AddListener(_presenter.OnBuyBaitClicked);
            _upgradeRodButton.onClick.AddListener(_presenter.OnUpgradeRodClicked);
        }

        public override async Task OnViewClose()
        {
            if (_presenter != null)
            {
                _prevLureButton.onClick.RemoveListener(_presenter.OnPrevLureClicked);
                _nextLureButton.onClick.RemoveListener(_presenter.OnNextLureClicked);
                _switchButton.onClick.RemoveListener(_presenter.OnSwitchClicked);
                _sellAllButton.onClick.RemoveListener(_presenter.OnSellAllClicked);
                _buyBaitButton.onClick.RemoveListener(_presenter.OnBuyBaitClicked);
                _upgradeRodButton.onClick.RemoveListener(_presenter.OnUpgradeRodClicked);
            }

            ResetPointerState();
            _presenter = null;
            await base.OnViewClose();
        }

        public override void OnViewUpdate(float deltaTime)
        {
            base.OnViewUpdate(deltaTime);
            PollPointerInput(deltaTime);
            _presenter?.Tick(deltaTime);
            ClearFrameInput();
        }

        internal void SetBar(Image fill, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            var fillRect = (RectTransform)fill.transform;
            var backgroundRect = fillRect.parent as RectTransform;
            var width = backgroundRect != null && backgroundRect.rect.width > 1f ? backgroundRect.rect.width : 320f;
            fillRect.sizeDelta = new Vector2(width * ratio, 0f);
        }

        internal void SetButtonInteractable(Button button, bool interactable)
        {
            if (button == null)
            {
                return;
            }

            button.interactable = interactable;

            var graphic = button.targetGraphic;
            if (graphic != null)
            {
                var color = graphic.color;
                color.a = interactable ? 1f : 0.45f;
                graphic.color = color;
            }
        }

        internal void ApplyInputMode(bool isIdle)
        {
            _isIdleInputMode = isIdle;
            UpdateCastZoneLayout();
            if (_castZoneVisual != null)
            {
                _castZoneVisual.gameObject.SetActive(isIdle);
            }

            SetRootActive(_primaryActionsRoot, isIdle);
            SetRootActive(_lureActionsRoot, isIdle);
            SetRootActive(_shopActionsRoot, isIdle);

            for (var i = 0; i < _idleOnlyButtons.Count; i++)
            {
                var button = _idleOnlyButtons[i];
                if (button == null)
                {
                    continue;
                }

                button.gameObject.SetActive(isIdle);
                button.interactable = isIdle;
            }

            for (var i = 0; i < _alwaysHiddenButtons.Count; i++)
            {
                var button = _alwaysHiddenButtons[i];
                if (button == null)
                {
                    continue;
                }

                button.gameObject.SetActive(false);
                button.interactable = false;
            }

            if (_actionHintText != null)
            {
                _actionHintText.text = isIdle
                    ? "待命阶段可出售、购买和升级；按住中央抛竿区蓄力，松开后抛竿"
                    : "钓鱼进行中，左键持续收线并涨张力；松开左右键会快速卸力，右键锁线并保持张力";
            }
        }

        internal void ResetPointerState()
        {
            _primaryHeld = false;
            _secondaryHeld = false;
            _primaryPressedThisFrame = false;
            _primaryReleasedThisFrame = false;
            _castZoneClickedThisFrame = false;
            _castZoneHoldDuration = 0f;
            _pendingCastPress = false;
        }

        private void RegisterButtons()
        {
            _idleOnlyButtons.Clear();
            _alwaysHiddenButtons.Clear();

            _idleOnlyButtons.Add(_switchButton);
            _idleOnlyButtons.Add(_prevLureButton);
            _idleOnlyButtons.Add(_nextLureButton);
            _idleOnlyButtons.Add(_sellAllButton);
            _idleOnlyButtons.Add(_buyBaitButton);
            _idleOnlyButtons.Add(_upgradeRodButton);
        }

        private void BuildCastZoneVisual()
        {
            if (_castZoneVisual != null)
            {
                return;
            }

            var castZoneObject = new GameObject("CastZoneVisual", typeof(RectTransform), typeof(Image));
            castZoneObject.transform.SetParent(transform, false);
            castZoneObject.layer = gameObject.layer;

            _castZoneVisual = castZoneObject.GetComponent<RectTransform>();
            var castZoneImage = castZoneObject.GetComponent<Image>();
            castZoneImage.color = new Color(0.20f, 0.72f, 0.86f, 0.05f);
            castZoneImage.raycastTarget = false;

            var outline = castZoneObject.AddComponent<Outline>();
            outline.effectColor = new Color(0.42f, 0.84f, 0.98f, 0.22f);
            outline.effectDistance = new Vector2(2f, -2f);

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(castZoneObject.transform, false);
            labelObject.layer = gameObject.layer;

            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(32f, 24f);
            labelRect.offsetMax = new Vector2(-32f, -24f);

            _castZoneLabel = labelObject.GetComponent<Text>();
            _castZoneLabel.font = _stateText != null ? _stateText.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _castZoneLabel.fontSize = 26;
            _castZoneLabel.alignment = TextAnchor.MiddleCenter;
            _castZoneLabel.fontStyle = FontStyle.Bold;
            _castZoneLabel.color = new Color(0.88f, 0.95f, 0.98f, 1f);
            _castZoneLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
            _castZoneLabel.verticalOverflow = VerticalWrapMode.Overflow;
            _castZoneLabel.raycastTarget = false;
            _castZoneLabel.text = "按住蓄力抛竿";

            UpdateCastZoneLayout();
            _castZoneVisual.SetSiblingIndex(Mathf.Min(1, transform.childCount - 1));
        }

        private void UpdateCastZoneLayout()
        {
            if (_castZoneVisual == null)
            {
                return;
            }

            var rootRect = transform as RectTransform;
            var width = 1280f;
            var height = 720f;
            if (rootRect != null && rootRect.rect.width > 1f && rootRect.rect.height > 1f)
            {
                width = rootRect.rect.width * CastZoneWidthRatio;
                height = rootRect.rect.height * CastZoneHeightRatio;
            }

            _castZoneVisual.anchorMin = new Vector2(0.5f, 0.5f);
            _castZoneVisual.anchorMax = new Vector2(0.5f, 0.5f);
            _castZoneVisual.pivot = new Vector2(0.5f, 0.5f);
            _castZoneVisual.anchoredPosition = Vector2.zero;
            _castZoneVisual.sizeDelta = new Vector2(width, height);
        }

        internal void UpdateCastZoneChargeDisplay(float currentDistance, float chargeRatio, bool isCharging)
        {
            if (_castZoneLabel == null)
            {
                return;
            }

            if (isCharging)
            {
                _castZoneLabel.text = $"松开抛竿\n{currentDistance:F1} m";
                _castZoneLabel.color = Color.Lerp(new Color(0.88f, 0.95f, 0.98f, 1f), new Color(1f, 0.92f, 0.45f, 1f), Mathf.Clamp01(chargeRatio));
                return;
            }

            _castZoneLabel.text = "按住蓄力抛竿\n5m - 40m";
            _castZoneLabel.color = new Color(0.88f, 0.95f, 0.98f, 1f);
        }

        private void PollPointerInput(float deltaTime)
        {
            UpdateCastZoneLayout();

            _primaryPressedThisFrame = Input.GetMouseButtonDown(0);
            _primaryReleasedThisFrame = Input.GetMouseButtonUp(0);
            _primaryHeld = Input.GetMouseButton(0);
            _secondaryHeld = !_isIdleInputMode && !Input.GetMouseButton(0) && Input.GetMouseButton(1);
            _castZoneClickedThisFrame = false;

            if (_primaryPressedThisFrame)
            {
                if (_isIdleInputMode)
                {
                    var pointerPosition = (Vector2)Input.mousePosition;
                    _pendingCastPress = IsPointerInsideCastZone(pointerPosition) && !IsPointerOverIdleOnlyButton(pointerPosition);
                    if (_pendingCastPress)
                    {
                        _castZoneHoldDuration = 0f;
                    }
                }
                else
                {
                    _pendingCastPress = false;
                }
            }

            if (_isIdleInputMode && _pendingCastPress && _primaryHeld)
            {
                _castZoneHoldDuration += deltaTime;
            }

            if (_primaryReleasedThisFrame)
            {
                if (_isIdleInputMode)
                {
                    _castZoneClickedThisFrame = _pendingCastPress;
                }

                _pendingCastPress = false;
            }
        }

        internal void SetButtonText(Button button, string value)
        {
            if (button == null)
            {
                return;
            }

            var label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                label.text = value;
            }
        }

        private void ClearFrameInput()
        {
            _primaryPressedThisFrame = false;
            _primaryReleasedThisFrame = false;
            _castZoneClickedThisFrame = false;
        }

        private static void SetRootActive(Transform root, bool active)
        {
            if (root != null)
            {
                root.gameObject.SetActive(active);
            }
        }

        private bool IsPointerInsideCastZone(Vector2 screenPoint)
        {
            if (_castZoneVisual == null)
            {
                return false;
            }

            var canvas = transform.GetComponentInParent<Canvas>();
            var eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
            return RectTransformUtility.RectangleContainsScreenPoint(_castZoneVisual, screenPoint, eventCamera);
        }

        private bool IsPointerOverIdleOnlyButton(Vector2 screenPoint)
        {
            var canvas = transform.GetComponentInParent<Canvas>();
            var eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
            for (var i = 0; i < _idleOnlyButtons.Count; i++)
            {
                var button = _idleOnlyButtons[i];
                if (button == null || !button.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(button.transform as RectTransform, screenPoint, eventCamera))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                ResetPointerState();
            }
        }
    }
}
