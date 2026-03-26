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

        [ComponentBinder("ControlsPanel/PrimaryActions/CastButton")] private Button _castButton;
        [ComponentBinder("ControlsPanel/LureActions/PrevLureButton")] private Button _prevLureButton;
        [ComponentBinder("ControlsPanel/LureActions/NextLureButton")] private Button _nextLureButton;
        [ComponentBinder("ControlsPanel/PrimaryActions/SwitchButton")] private Button _switchButton;
        [ComponentBinder("ControlsPanel/FightActions/ReelButton")] private Button _reelButton;
        [ComponentBinder("ControlsPanel/FightActions/ReleaseButton")] private Button _releaseButton;
        [ComponentBinder("ControlsPanel/PrimaryActions")] private Transform _primaryActionsRoot;
        [ComponentBinder("ControlsPanel/LureActions")] private Transform _lureActionsRoot;
        [ComponentBinder("ControlsPanel/FightActions")] private Transform _fightActionsRoot;

        [ComponentBinder("HudPanel/StaminaRow/StaminaBar/Fill")] private Image _staminaFill;
        [ComponentBinder("HudPanel/TensionRow/TensionBar/Fill")] private Image _tensionFill;
        [ComponentBinder("HudPanel/LineRow/LineBar/Fill")] private Image _lineFill;

        private FishGamePresenter _presenter;
        private readonly List<Button> _idleOnlyButtons = new List<Button>();
        private readonly List<Button> _alwaysHiddenButtons = new List<Button>();
        private RectTransform _castZoneVisual;
        private Text _castZoneLabel;
        private bool _isIdleInputMode = true;
        private bool _primaryHeld;
        private bool _primaryPressedThisFrame;
        private bool _primaryReleasedThisFrame;
        private bool _castZoneClickedThisFrame;
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
        internal Button CastButton => _castButton;
        internal Button PrevLureButton => _prevLureButton;
        internal Button NextLureButton => _nextLureButton;
        internal Button ReelButton => _reelButton;
        internal Button ReleaseButton => _releaseButton;
        internal Image StaminaFill => _staminaFill;
        internal Image TensionFill => _tensionFill;
        internal Image LineFill => _lineFill;
        internal bool IsPrimaryHeld => _primaryHeld;
        internal bool PrimaryPressedThisFrame => _primaryPressedThisFrame;
        internal bool PrimaryReleasedThisFrame => _primaryReleasedThisFrame;
        internal bool CastZoneClickedThisFrame => _castZoneClickedThisFrame;

        public override UIViewLayer ViewLayer => UIViewLayer.Normal;

        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);

            BuildCastZoneVisual();
            RegisterButtons();
            ApplyInputMode(true);

            _presenter = new FishGamePresenter(this);

            _prevLureButton.onClick.AddListener(_presenter.OnPrevLureClicked);
            _nextLureButton.onClick.AddListener(_presenter.OnNextLureClicked);
            _switchButton.onClick.AddListener(_presenter.OnSwitchClicked);
        }

        public override async Task OnViewClose()
        {
            if (_presenter != null)
            {
                _prevLureButton.onClick.RemoveListener(_presenter.OnPrevLureClicked);
                _nextLureButton.onClick.RemoveListener(_presenter.OnNextLureClicked);
                _switchButton.onClick.RemoveListener(_presenter.OnSwitchClicked);
            }

            ResetPointerState();
            _presenter = null;
            await base.OnViewClose();
        }

        public override void OnViewUpdate(float deltaTime)
        {
            base.OnViewUpdate(deltaTime);
            PollPointerInput();
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
            SetRootActive(_fightActionsRoot, false);

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
                    ? "待命阶段可切换玩法与鱼饵，点击中央抛竿区开始钓鱼"
                    : "钓鱼进行中，按住鼠标左键收线，松开后仅在鱼发力时被带线";
            }
        }

        internal void ResetPointerState()
        {
            _primaryHeld = false;
            _primaryPressedThisFrame = false;
            _primaryReleasedThisFrame = false;
            _castZoneClickedThisFrame = false;
            _pendingCastPress = false;
        }

        private void RegisterButtons()
        {
            _idleOnlyButtons.Clear();
            _alwaysHiddenButtons.Clear();

            _idleOnlyButtons.Add(_switchButton);
            _idleOnlyButtons.Add(_prevLureButton);
            _idleOnlyButtons.Add(_nextLureButton);

            _alwaysHiddenButtons.Add(_castButton);
            _alwaysHiddenButtons.Add(_reelButton);
            _alwaysHiddenButtons.Add(_releaseButton);
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
            _castZoneLabel.text = "点击中央区域抛竿";

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

        private void PollPointerInput()
        {
            UpdateCastZoneLayout();

            _primaryPressedThisFrame = Input.GetMouseButtonDown(0);
            _primaryReleasedThisFrame = Input.GetMouseButtonUp(0);
            _primaryHeld = Input.GetMouseButton(0);
            _castZoneClickedThisFrame = false;

            if (_primaryPressedThisFrame)
            {
                if (_isIdleInputMode)
                {
                    var pointerPosition = (Vector2)Input.mousePosition;
                    _pendingCastPress = IsPointerInsideCastZone(pointerPosition) && !IsPointerOverIdleOnlyButton(pointerPosition);
                }
                else
                {
                    _pendingCastPress = false;
                }
            }

            if (_primaryReleasedThisFrame)
            {
                if (_isIdleInputMode)
                {
                    var pointerPosition = (Vector2)Input.mousePosition;
                    _castZoneClickedThisFrame = _pendingCastPress
                        && IsPointerInsideCastZone(pointerPosition)
                        && !IsPointerOverIdleOnlyButton(pointerPosition);
                }

                _pendingCastPress = false;
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
