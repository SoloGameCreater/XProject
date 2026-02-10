using System.Threading.Tasks;
using TripleMerge;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Maintenance
{
    /// <summary>
    /// 维护模式会话：显示玩法关闭提示，避免无默认玩法时黑屏卡死。
    /// </summary>
    public class MaintenanceSession : GameplayRuntime.IGameplaySession
    {
        private const string MaintenancePanelName = "MaintenancePanel";
        private const string BuiltinLegacyFontPath = "LegacyRuntime.ttf";
        private GameObject _maintenancePanel;

        public Task<bool> EnterAsync(object param = null)
        {
            return Task.FromResult(true);
        }

        public void EnterFinish()
        {
            UILoadingController.Hide(false);
            TryCloseLobby();
            ShowMaintenancePanel();
            Debug.LogWarning("当前默认玩法为维护模式，TripleMerge 已关闭。");
        }

        public void Update(float deltaTime)
        {
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void Exit()
        {
            if (_maintenancePanel != null)
            {
                Object.Destroy(_maintenancePanel);
                _maintenancePanel = null;
            }
        }

        private void TryCloseLobby()
        {
            if (UIViewSystem.Instance.Get<LobbyMainUI>() != null)
            {
                UIViewSystem.Instance.Close<LobbyMainUI>();
            }
        }

        private void ShowMaintenancePanel()
        {
            if (UIRoot.Instance == null || UIRoot.Instance.mRootCanvas == null)
            {
                Debug.LogError("MaintenanceSession: UIRoot 未就绪，无法显示维护提示。");
                return;
            }

            var parent = UIRoot.Instance.mRootCanvas.transform;
            var exists = parent.Find(MaintenancePanelName);
            if (exists != null)
            {
                _maintenancePanel = exists.gameObject;
                _maintenancePanel.SetActive(true);
                return;
            }

            _maintenancePanel = new GameObject(MaintenancePanelName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            _maintenancePanel.transform.SetParent(parent, false);

            var panelRect = (RectTransform)_maintenancePanel.transform;
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            var bg = _maintenancePanel.GetComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.85f);

            var textObject = new GameObject("Message", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textObject.transform.SetParent(_maintenancePanel.transform, false);

            var textRect = (RectTransform)textObject.transform;
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(900f, 260f);

            var messageText = textObject.GetComponent<Text>();
            messageText.alignment = TextAnchor.MiddleCenter;
            messageText.fontSize = 42;
            messageText.lineSpacing = 1.2f;
            messageText.color = Color.white;
            messageText.font = LoadRuntimeFont();
            messageText.text = "当前版本已关闭 TripleMerge 玩法\n请切换默认玩法模块或稍后重试";
        }

        private Font LoadRuntimeFont()
        {
            try
            {
                var builtinFont = Resources.GetBuiltinResource<Font>(BuiltinLegacyFontPath);
                if (builtinFont != null)
                {
                    return builtinFont;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"MaintenanceSession: 加载内置字体失败，path={BuiltinLegacyFontPath}, error={e.Message}");
            }

            // 兜底到系统字体，避免文本组件空字体导致异常
            return Font.CreateDynamicFontFromOSFont(new[] { "Arial", "Microsoft YaHei", "Segoe UI" }, 14);
        }
    }
}
