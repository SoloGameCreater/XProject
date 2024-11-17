
using System.Threading.Tasks;
using GameFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [AssetAddress("UIForms/DialogForm")]
    public class DialogForm : UIPopup
    {
        public class Param : UIViewParam
        {
            public DialogParams DialogParam;
        }
        [SerializeField] private TextMeshProUGUI m_TitleText = null;

        [SerializeField] private TextMeshProUGUI m_MessageText = null;

        [SerializeField] private GameObject[] m_ModeObjects = null;

        [SerializeField] private TextMeshProUGUI[] m_ConfirmTexts = null;

        [SerializeField] private TextMeshProUGUI[] m_CancelTexts = null;

        [SerializeField] private TextMeshProUGUI[] m_OtherTexts = null;
        
        [ComponentBinder("Confirm")] private Button _btnConfirm;
        [ComponentBinder("Cancel")] private Button _btnCancel;
        [ComponentBinder("Other")] private Button _btnOther;
        
        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private object m_UserData = null;
        private GameFrameworkAction<object> m_OnClickConfirm = null;
        private GameFrameworkAction<object> m_OnClickCancel = null;
        private GameFrameworkAction<object> m_OnClickOther = null;

        public int DialogMode
        {
            get { return m_DialogMode; }
        }

        public bool PauseGame
        {
            get { return m_PauseGame; }
        }

        public object UserData
        {
            get { return m_UserData; }
        }

        private void OnConfirmButtonClick()
        {
            OnViewDestroy();

            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(m_UserData);
            }
        }

        private void OnCancelButtonClick()
        {
            OnViewDestroy();

            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(m_UserData);
            }
        }

        private void OnOtherButtonClick()
        {
            OnViewDestroy();

            if (m_OnClickOther != null)
            {
                m_OnClickOther(m_UserData);
            }
        }
        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);
            var param2 = param as Param;
            if (param2 == null)
            {
                Debug.LogError("DialogForm::OnViewOpen param is invalid.");
                return;
            }
            Init(param2.DialogParam);
            
            _btnConfirm.onClick.AddListener(OnConfirmButtonClick);
            _btnCancel.onClick.AddListener(OnCancelButtonClick);
            _btnOther.onClick.AddListener(OnOtherButtonClick);
        }

        public override Task OnViewClose()
        {
            _btnConfirm.onClick.RemoveListener(OnConfirmButtonClick);
            _btnCancel.onClick.RemoveListener(OnCancelButtonClick);
            _btnOther.onClick.RemoveListener(OnOtherButtonClick);
            if (m_PauseGame)
            {
                GameModule.Base.ResumeGame();
            }

            m_DialogMode = 1;
            m_TitleText.text = string.Empty;
            m_MessageText.text = string.Empty;
            m_PauseGame = false;
            m_UserData = null;

            RefreshConfirmText(string.Empty);
            m_OnClickConfirm = null;

            RefreshCancelText(string.Empty);
            m_OnClickCancel = null;

            RefreshOtherText(string.Empty);
            m_OnClickOther = null;
            
            return base.OnViewClose();
        }

        private void Init(DialogParams dialogParams)
        {
            m_DialogMode = dialogParams.Mode;
            RefreshDialogMode();

            m_TitleText.text = dialogParams.Title;
            m_MessageText.text = dialogParams.Message;

            m_PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();

            m_UserData = dialogParams.UserData;

            RefreshConfirmText(dialogParams.ConfirmText);
            m_OnClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            m_OnClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            m_OnClickOther = dialogParams.OnClickOther;
        }
        private void RefreshDialogMode()
        {
            for (int i = 1; i <= m_ModeObjects.Length; i++)
            {
                m_ModeObjects[i - 1].SetActive(i == m_DialogMode);
            }
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameModule.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = GameModule.Localization.GetString("Dialog.ConfirmButton");
            }

            for (int i = 0; i < m_ConfirmTexts.Length; i++)
            {
                m_ConfirmTexts[i].text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = GameModule.Localization.GetString("Dialog.CancelButton");
            }

            for (int i = 0; i < m_CancelTexts.Length; i++)
            {
                m_CancelTexts[i].text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = GameModule.Localization.GetString("Dialog.OtherButton");
            }

            for (int i = 0; i < m_OtherTexts.Length; i++)
            {
                m_OtherTexts[i].text = otherText;
            }
        }
    }
}