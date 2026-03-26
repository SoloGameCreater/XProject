using System.Threading.Tasks;
using GameplayRuntime;
using Framework;
using TripleMerge;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.TripleMerge
{
    public class TripleMergeSession : GameplayRuntime.IGameplaySession
    {
        private GameObject _switchButtonRoot;

        public Task<bool> EnterAsync(object param = null)
        {
            TripleMergeSystem.Instance.OnEnterTripleMerge();
            return Task.FromResult(true);
        }

        public void EnterFinish()
        {
            UILoadingController.Hide(false);
            if (UIViewSystem.Instance.Get<LobbyMainUI>() != null)
            {
                UIViewSystem.Instance.Get<LobbyMainUI>().Show();
            }
            else
            {
                UIViewSystem.Instance.Open<LobbyMainUI>();
            }

            CreateSwitchButton();
        }

        public void Update(float deltaTime)
        {
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void Exit()
        {
            if (_switchButtonRoot != null)
            {
                Object.Destroy(_switchButtonRoot);
                _switchButtonRoot = null;
            }

            TripleMergeSystem.Instance.Exit();
        }

        private void CreateSwitchButton()
        {
            if (UIRoot.Instance == null || UIRoot.Instance.mRootCanvas == null)
            {
                return;
            }

            _switchButtonRoot = new GameObject("TripleMergeToFishButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            _switchButtonRoot.transform.SetParent(UIRoot.Instance.mRootCanvas.transform, false);

            var rect = _switchButtonRoot.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = new Vector2(-24f, 24f);
            rect.sizeDelta = new Vector2(180f, 44f);

            var image = _switchButtonRoot.GetComponent<Image>();
            image.color = new Color(0.15f, 0.42f, 0.75f, 0.95f);

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            labelObject.transform.SetParent(_switchButtonRoot.transform, false);
            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(8f, 4f);
            labelRect.offsetMax = new Vector2(-8f, -4f);

            var label = labelObject.GetComponent<Text>();
            label.font = LoadRuntimeFont();
            label.text = "返回钓鱼玩法";
            label.fontSize = 18;
            label.alignment = TextAnchor.MiddleCenter;
            label.fontStyle = FontStyle.Bold;
            label.color = Color.white;

            _switchButtonRoot.GetComponent<Button>().onClick.AddListener(async () =>
            {
                DebugUtil.Log("TripleMergeSession: 切换回 FishGame。");
                await GameplayDirector.Instance.EnterAsync(GameplayIds.FishGame);
            });
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
            catch
            {
            }

            return Font.CreateDynamicFontFromOSFont(new[] { "Arial", "Microsoft YaHei", "Segoe UI" }, 18);
        }
    }
}
