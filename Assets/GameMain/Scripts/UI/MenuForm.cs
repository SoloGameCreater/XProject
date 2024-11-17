
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [AssetAddress("UIForms/MenuForm")]
    public class MenuForm : UIPopup
    {
        /* 关闭按钮 */
        [ComponentBinder("About")] private Button _btnAbout;
        [ComponentBinder("Quit")] private Button _btnQuit;
        [ComponentBinder("Start")] private Button _btnStart;
        [ComponentBinder("Setting")] private Button _btnSetting;

        private void OnStartButtonClick()
        {
           
        }

        private void OnSettingButtonClick()
        {
            UIViewSystem.Instance.Open<SettingForm>();
        }

        private void OnAboutButtonClick()
        {
            UIViewSystem.Instance.Open<AboutForm>();
        }

        private void OnQuitButtonClick()
        {
            var dialogInfo = new DialogForm.Param()
            {
                DialogParam = new DialogParams()
                {
                    Mode = 2,
                    Title = GameModule.Localization.GetString("AskQuitGame.Title"),
                    Message = GameModule.Localization.GetString("AskQuitGame.Message"),
                    OnClickConfirm = delegate(object userData) { GameSystem.Shutdown(ShutdownType.Quit); },
                }
            };
            UIViewSystem.Instance.Open<DialogForm>(dialogInfo);
        }

        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);
            _btnAbout.onClick.AddListener(OnAboutButtonClick);
            _btnStart.onClick.AddListener(OnStartButtonClick);
            _btnSetting.onClick.AddListener(OnStartButtonClick);
            _btnQuit.onClick.AddListener(OnQuitButtonClick);
        }

        public override Task OnViewClose()
        {
            _btnAbout.onClick.RemoveListener(OnAboutButtonClick);
            _btnStart.onClick.RemoveListener(OnSettingButtonClick);
            _btnSetting.onClick.RemoveListener(OnStartButtonClick);
            _btnQuit.onClick.RemoveListener(OnQuitButtonClick);
            
            return base.OnViewClose();
        }
    }
}