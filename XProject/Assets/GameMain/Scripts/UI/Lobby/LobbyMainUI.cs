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
            Debug.LogWarning("Starrrrrrrt");
        }

        public void Show()
        {
            Debug.LogWarning("Shoooooooow");
        }
    }
}