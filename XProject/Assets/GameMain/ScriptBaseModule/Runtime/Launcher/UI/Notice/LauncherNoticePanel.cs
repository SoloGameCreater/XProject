using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseModule
{
    public class LauncherNoticePanel : MonoBehaviour
    {
        public class Param
        {
            public string DescStr                     = "";   //描述文字
            public string TitleStr                    = "";   //标题,不传用默认值(id:297)
            public string ConfirmStr                  = "";   //确认文字，不传用默认值(id:213)
            public string CancelStr                   = "";   //关闭文字 不传使用默认值(id:216)
            public bool   IsShowCancelBtn             = true; //是否显示取消按钮,默认不显示 通知界面只会有一个确认按钮
            public bool   IsShowCloseBtn              = true; //是否显示关闭按钮
            public Action ConfirmCallBack             = null; //确认回调
            public Action CancelCallBack              = null; //取消回调
            public Action CloseCallBack               = null; //关闭回调
        }
        
        private static LauncherNoticePanel Instance;
    
        public static void Show(Param param)
        {
            GameObject prefab = Resources.Load<GameObject>("Launcher/Prefab/LauncherNotice");
            GameObject obj = Instantiate(prefab, GameObject.Find("UIRoot/Canvas").transform);
            Instance = obj.AddComponent<LauncherNoticePanel>();
            Instance.Init(param);
        }

        public static void Hide()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }
        }

        private Param            m_Param;
        
        private Text             m_TitleText;
        private Button           m_CloseBtn;
        private Text             m_DescText;
        private Button           m_CancelBtn;
        private Button           m_ConfirmBtn;
        private Text             m_CancelBtnText;
        private Text             m_ConfirmBtnText;
        
        public void Init(Param param)
        {
            m_TitleText = gameObject.transform.Find("Root/TitleGroup/TitleText")?.GetComponent<Text>();
            LauncherLocalizationManager.Instance().SetMaterial(m_TitleText);
            m_CloseBtn  = gameObject.transform.Find("Root/CloseButton")?.GetComponent<Button>();
            m_CloseBtn.onClick.AddListener(OnCloseClick);
            m_DescText  = gameObject.transform.Find("Root/InsideGroup/Scroll View/Viewport/Content/TipsText")?.GetComponent<Text>();
            LauncherLocalizationManager.Instance().SetMaterial(m_DescText);
            m_CancelBtn = gameObject.transform.Find("Root/ButtonGroup/NoButton")?.GetComponent<Button>();
            m_CancelBtn.onClick.AddListener(OnCancelClick);
            m_ConfirmBtn = gameObject.transform.Find("Root/ButtonGroup/YesButton")?.GetComponent<Button>();
            m_ConfirmBtn.onClick.AddListener(OnConfirmClick);
            m_CancelBtnText        = gameObject.transform.Find("Root/ButtonGroup/NoButton/Text")?.GetComponent<Text>();
            LauncherLocalizationManager.Instance().SetMaterial(m_CancelBtnText);
            m_ConfirmBtnText       = gameObject.transform.Find("Root/ButtonGroup/YesButton/Text")?.GetComponent<Text>();
            LauncherLocalizationManager.Instance().SetMaterial(m_ConfirmBtnText);

            m_Param = param;
            
            m_CloseBtn.gameObject.SetActive(m_Param.IsShowCloseBtn);
            m_CancelBtn.gameObject.SetActive(m_Param.IsShowCancelBtn);
            m_ConfirmBtn.gameObject.SetActive(true);

            if (!string.IsNullOrEmpty(m_Param.TitleStr)) m_TitleText.text = m_Param.TitleStr;
            else m_TitleText.text = LauncherLocalizationManager.Instance().GetText("UI_Desc_Notice");
            
            if (!string.IsNullOrEmpty(m_Param.DescStr)) m_DescText.text = m_Param.DescStr;

            if (!string.IsNullOrEmpty(m_Param.CancelStr)) m_CancelBtnText.text = m_Param.CancelStr;
            else m_CancelBtnText.text = LauncherLocalizationManager.Instance().GetText("UI_Button_Cancel");

            if (!string.IsNullOrEmpty(m_Param.ConfirmStr)) m_ConfirmBtnText.text = m_Param.ConfirmStr;
            else m_ConfirmBtnText.text = LauncherLocalizationManager.Instance().GetText("UI_Button_Ok");
        }
        
        private void OnConfirmClick()
        {
            OnCloseClick();
            m_Param?.ConfirmCallBack?.Invoke();
        }

        private void OnCancelClick()
        {
            OnCloseClick();
            m_Param?.CancelCallBack?.Invoke();
        }

        private void OnCloseClick()
        {
            Hide();
            m_Param?.CloseCallBack?.Invoke();
        }
    }
}