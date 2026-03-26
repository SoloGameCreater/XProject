using System.Threading.Tasks;
using Framework;
using Modules.FishGame;
using UnityEngine;
using UnityEngine.UI;

namespace FishGameRuntime
{
    [AssetAddress("UIFishGame/FishGameMainUI")]
    public class FishGameMainUI : UIView
    {
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

        [ComponentBinder("ControlsPanel/PrimaryActions/CastButton")] private Button _castButton;
        [ComponentBinder("ControlsPanel/LureActions/PrevLureButton")] private Button _prevLureButton;
        [ComponentBinder("ControlsPanel/LureActions/NextLureButton")] private Button _nextLureButton;
        [ComponentBinder("ControlsPanel/PrimaryActions/SwitchButton")] private Button _switchButton;
        [ComponentBinder("ControlsPanel/FightActions/ReelButton")] private Button _reelButton;
        [ComponentBinder("ControlsPanel/FightActions/ReleaseButton")] private Button _releaseButton;

        [ComponentBinder("HudPanel/StaminaRow/StaminaBar/Fill")] private Image _staminaFill;
        [ComponentBinder("HudPanel/TensionRow/TensionBar/Fill")] private Image _tensionFill;
        [ComponentBinder("HudPanel/LineRow/LineBar/Fill")] private Image _lineFill;

        private FishGamePresenter _presenter;
        private FishGameHoldButton _reelHoldButton;
        private FishGameHoldButton _releaseHoldButton;

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
        internal Button CastButton => _castButton;
        internal Button PrevLureButton => _prevLureButton;
        internal Button NextLureButton => _nextLureButton;
        internal Button ReelButton => _reelButton;
        internal Button ReleaseButton => _releaseButton;
        internal Image StaminaFill => _staminaFill;
        internal Image TensionFill => _tensionFill;
        internal Image LineFill => _lineFill;
        internal FishGameHoldButton ReelHoldButton => _reelHoldButton;
        internal FishGameHoldButton ReleaseHoldButton => _releaseHoldButton;

        public override UIViewLayer ViewLayer => UIViewLayer.Normal;

        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);

            _reelHoldButton = CommonUtils.GetOrCreateComponent<FishGameHoldButton>(_reelButton.gameObject);
            _releaseHoldButton = CommonUtils.GetOrCreateComponent<FishGameHoldButton>(_releaseButton.gameObject);

            _presenter = FishGameSession.GetOrCreatePresenter(this);
            _presenter.BindView(this);

            _castButton.onClick.AddListener(_presenter.OnCastClicked);
            _prevLureButton.onClick.AddListener(_presenter.OnPrevLureClicked);
            _nextLureButton.onClick.AddListener(_presenter.OnNextLureClicked);
            _switchButton.onClick.AddListener(_presenter.OnSwitchClicked);
        }

        public override async Task OnViewClose()
        {
            _castButton.onClick.RemoveListener(_presenter.OnCastClicked);
            _prevLureButton.onClick.RemoveListener(_presenter.OnPrevLureClicked);
            _nextLureButton.onClick.RemoveListener(_presenter.OnNextLureClicked);
            _switchButton.onClick.RemoveListener(_presenter.OnSwitchClicked);
            _presenter.UnbindView(this);
            _presenter = null;
            await base.OnViewClose();
        }

        public override void OnViewUpdate(float deltaTime)
        {
            base.OnViewUpdate(deltaTime);
            _presenter?.Tick(deltaTime);
        }

        internal void SetBar(Image fill, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            var fillRect = (RectTransform)fill.transform;
            var backgroundRect = fillRect.parent as RectTransform;
            var width = backgroundRect != null && backgroundRect.rect.width > 1f ? backgroundRect.rect.width : 320f;
            fillRect.sizeDelta = new Vector2(width * ratio, 0f);
        }
    }
}
