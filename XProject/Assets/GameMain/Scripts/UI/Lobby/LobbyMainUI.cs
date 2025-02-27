using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TripleMerge
{
    [AssetAddress("UILobby/LobbyUI")]
    public class LobbyMainUI : UIView
    {
        [ComponentBinder("StartButton")] private Button _startButton;
        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);

            _startButton.onClick.AddListener(OnStartClick);
        }

        public override async Task OnViewClose()
        {
            _startButton.onClick.RemoveListener(OnStartClick);
            base.OnViewClose();
        }

        private void OnStartClick()
        {
            var generationViewportPos = UIRoot.Instance.mUICamera.ScreenToViewportPoint(((RectTransform) transform).anchoredPosition);
            TripleMergeSystem.Instance.Gameplay.TreasureManager.GenerateTreasure(generationViewportPos);
        }

        public void Show()
        {
            Debug.LogWarning("Shoooooooow");
        }
    }
}