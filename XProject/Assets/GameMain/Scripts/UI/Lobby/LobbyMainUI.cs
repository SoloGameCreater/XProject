using System;
using System.Threading.Tasks;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
using DebugTools;
using Framework;

#endif
using SaveFile;
using SaveFile.TripleMerge;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TripleMerge
{
    [AssetAddress("UILobby/LobbyUI")]
    public class LobbyMainUI : UIView
    {
        [ComponentBinder("StartButton")] private Button _startButton;
        [ComponentBinder("DebugBtn")] private Button _debugButton;
        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);
            _debugButton.gameObject.SetActive(false);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _debugButton.gameObject.SetActive(true);
#endif
            _startButton.onClick.AddListener(OnStartClick);
            _debugButton.onClick.AddListener(OnDebugClick);
        }

        public override async Task OnViewClose()
        {
            _startButton.onClick.RemoveListener(OnStartClick);
            _debugButton.onClick.RemoveListener(OnDebugClick);
            await base.OnViewClose();
        }


        private void OnDebugClick()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            //todo debug工具弹出框
            UIViewSystem.Instance.Open<DebugUI>();
            //SaveFileManager.Instance.GetSaveFile<SaveFileTripleMerge>().Clear();
            //DebugUtil.LogWarning("清除三消数据");
            //QuitApp();
#endif
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