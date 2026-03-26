using System.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace FishGameRuntime
{
    [AssetAddress("UIFishGame/FishGameMainUI")]
    public class FishGameMainUI : UIView
    {
        [ComponentBinder("StateText")] private Text _stateText;
        [ComponentBinder("EquipText")] private Text _equipText;
        [ComponentBinder("ScoreText")] private Text _scoreText;
        [ComponentBinder("LineText")] private Text _lineText;
        [ComponentBinder("HintText")] private Text _hintText;
        [ComponentBinder("NotifyPanel/NotifyText")] private Text _notifyText;
        [ComponentBinder("InventoryText")] private Text _inventoryText;

        [ComponentBinder("CastButton")] private Button _castButton;
        [ComponentBinder("PrevLureButton")] private Button _prevLureButton;
        [ComponentBinder("NextLureButton")] private Button _nextLureButton;
        [ComponentBinder("SwitchButton")] private Button _switchButton;
        [ComponentBinder("ReelButton")] private Button _reelButton;
        [ComponentBinder("ReleaseButton")] private Button _releaseButton;

        [ComponentBinder("HudPanel/StaminaBar/Fill")] private Image _staminaFill;
        [ComponentBinder("HudPanel/TensionBar/Fill")] private Image _tensionFill;
        [ComponentBinder("HudPanel/LineBar/Fill")] private Image _lineFill;

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

            _presenter = new FishGamePresenter(this);

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
            ((RectTransform)fill.transform).sizeDelta = new Vector2(320f * ratio, 0f);
        }
    }
}
