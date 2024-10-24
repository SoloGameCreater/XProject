using System;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseModule
{
    public class LoadingPanel : MonoBehaviour
    {
        public static LoadingPanel Instance;
    
        public static void Show()
        {
            GameObject prefab = Resources.Load<GameObject>("Launcher/Prefab/UILoading");
            GameObject obj = Instantiate(prefab, GameObject.Find("UIRoot/Canvas").transform);
            Instance = obj.AddComponent<LoadingPanel>();
        }

        public static void Hide()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }
        }
    
        private Slider _slider;
        private Text _progressText;
        //private Text _TipsText;

        private string _progressTextFormat;

        private void Awake()
        {
            _slider = transform.Find("Slider").GetComponent<Slider>();
            _progressText = transform.Find("Slider/ProgressInfo").GetComponent<Text>();
            LauncherLocalizationManager.Instance().SetMaterial(_progressText);
            //_TipsText = transform.Find("Tip").GetComponent<Text>();
            //TryToGetStroageInstallAt();
            if (UseOldLoadingImage())
            {
                var _bgImage = transform.Find("Image").GetComponent<RawImage>();
                _bgImage.texture = Resources.Load<Texture>("Launcher/Textures/ui_landing_bg_old");
                transform.Find("Logo").gameObject.SetActive(true);
            }
            else
            {
                var _bgImage = transform.Find("Image").GetComponent<RawImage>();
                _bgImage.texture = Resources.Load<Texture>("Launcher/Textures/ui_landing_bg");
                transform.Find("Logo").gameObject.SetActive(false);
            }
        }
        
        public void SetProgress(float value)
        {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            _slider.value = value;

            if (!string.IsNullOrEmpty(_progressTextFormat))
            {
                _progressText.text = string.Format(_progressTextFormat, (int)(value * 100));
            }
        }

        public float GetProgress()
        {
            return _slider.value;
        }
        public void SetProgressText(string text)
        {
            _progressText.text = text;
        }
        
        public void SetProgressTextFormat(string textFormat)
        {
            _progressTextFormat = textFormat;
        }

        // public void SetTipsText(string param)
        // {
        //     _TipsText.text = param;
        // }
        
        private void TryToGetStroageInstallAt()
        {
            // try
            // {
            //     var storageKey = "StorageData";
            //     string jsonData = "{}";
            //     if (PlayerPrefs.HasKey(storageKey))
            //     {
            //         byte[] encryptData = System.Convert.FromBase64String(PlayerPrefs.GetString(storageKey));
            //         jsonData = RijndaelManager.Instance.DecryptStringFromBytes(encryptData);
            //         JObject obj = JObject.Parse(jsonData);
            //         StorageInstallAt = ulong.Parse(obj["StorageCommon"]["installedAt"].ToString());
            //     }
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError("TryToGetStroageInstallAt Failed");
            // }
        }

        public static ulong StorageInstallAt = 0;
        public static bool UseOldLoadingImage()
        {
            return StorageInstallAt < 1719763200000 || StorageInstallAt > 1724342400000;
        }
    }
}

