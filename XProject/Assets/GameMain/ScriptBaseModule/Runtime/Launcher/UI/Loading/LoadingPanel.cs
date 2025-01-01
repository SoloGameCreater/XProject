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
    }
}

