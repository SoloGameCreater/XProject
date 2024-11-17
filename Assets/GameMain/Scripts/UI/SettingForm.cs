
using System.Threading.Tasks;
using DG.Tweening;
using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class SettingForm : UIPopup
    {
        [SerializeField] private Toggle m_MusicMuteToggle = null;

        [SerializeField] private Slider m_MusicVolumeSlider = null;

        [SerializeField] private Toggle m_SoundMuteToggle = null;

        [SerializeField] private Slider m_SoundVolumeSlider = null;

        [SerializeField] private Toggle m_UISoundMuteToggle = null;

        [SerializeField] private Slider m_UISoundVolumeSlider = null;
        [ComponentBinder("LanguageTips")]
        private CanvasGroup m_LanguageTipsCanvasGroup = null;

        [SerializeField] private Toggle m_EnglishToggle = null;

        [SerializeField] private Toggle m_ChineseSimplifiedToggle = null;

        [SerializeField] private Toggle m_ChineseTraditionalToggle = null;

        [SerializeField] private Toggle m_KoreanToggle = null;
        [ComponentBinder("Cancel")] private Button _btnClose;
        [ComponentBinder("Confirm")] private Button _btnComfirm;

        private Language m_SelectedLanguage = Language.Unspecified;

        public void OnMusicMuteChanged(bool isOn)
        {
            GameModule.Sound.Mute("Music", !isOn);
            m_MusicVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            GameModule.Sound.SetVolume("Music", volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            GameModule.Sound.Mute("Sound", !isOn);
            m_SoundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameModule.Sound.SetVolume("Sound", volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            GameModule.Sound.Mute("UISound", !isOn);
            m_UISoundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            GameModule.Sound.SetVolume("UISound", volume);
        }

        public void OnEnglishSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.English;
            RefreshLanguageTips();
        }

        public void OnChineseSimplifiedSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.ChineseSimplified;
            RefreshLanguageTips();
        }

        public void OnChineseTraditionalSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.ChineseTraditional;
            RefreshLanguageTips();
        }

        public void OnKoreanSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.Korean;
            RefreshLanguageTips();
        }

        private void OnSubmitButtonClick()
        {
            if (m_SelectedLanguage == GameModule.Localization.Language)
            {
                OnViewDestroy();
                return;
            }

            GameModule.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
            GameModule.Setting.Save();

            GameModule.Sound.StopMusic();
            GameSystem.Shutdown(ShutdownType.Restart);
        }

        private void CloseOnClick()
        {
            OnViewDestroy();
        }
        private void PlayLanguageTips()
        {
            m_LanguageTipsCanvasGroup.DOFade(1, 0.1f);
        }

        private void RefreshLanguageTips()
        {
            if (m_SelectedLanguage != GameModule.Localization.Language)
            {
                PlayLanguageTips();
            }
            else
            {
                m_LanguageTipsCanvasGroup.alpha = 0;
            }
        }
        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);
            
            Init();
            _btnClose.onClick.AddListener(CloseOnClick);
            _btnComfirm.onClick.AddListener(OnSubmitButtonClick);
        }

        public override Task OnViewClose()
        {
            _btnClose.onClick.RemoveListener(CloseOnClick);
            _btnComfirm.onClick.RemoveListener(OnSubmitButtonClick);
            
            return base.OnViewClose();
        }

        private void Init()
        {
            m_MusicMuteToggle.isOn = !GameModule.Sound.IsMuted("Music");
            m_MusicVolumeSlider.value = GameModule.Sound.GetVolume("Music");

            m_SoundMuteToggle.isOn = !GameModule.Sound.IsMuted("Sound");
            m_SoundVolumeSlider.value = GameModule.Sound.GetVolume("Sound");

            m_UISoundMuteToggle.isOn = !GameModule.Sound.IsMuted("UISound");
            m_UISoundVolumeSlider.value = GameModule.Sound.GetVolume("UISound");

            m_SelectedLanguage = GameModule.Localization.Language;
            switch (m_SelectedLanguage)
            {
                case Language.English:
                    m_EnglishToggle.isOn = true;
                    break;

                case Language.ChineseSimplified:
                    m_ChineseSimplifiedToggle.isOn = true;
                    break;

                case Language.ChineseTraditional:
                    m_ChineseTraditionalToggle.isOn = true;
                    break;

                case Language.Korean:
                    m_KoreanToggle.isOn = true;
                    break;
                default:
                    Debug.LogWarning($"Can not find the selected language. {m_SelectedLanguage.ToString()}");
                    break;
            }
        }
    }
}