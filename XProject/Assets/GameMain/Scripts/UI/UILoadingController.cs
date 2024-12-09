using System;
using System.Collections.Generic;
using BaseModule;
using Extension;
using Framework;
using Localizetion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingController : MonoBehaviour
{
    public enum LoadingStep
    {
        Launching,
        DownloadRes,
        InitScene,
    }

    public struct LoadingData
    {
        public LoadingData(float progress, string extraInfo)
        {
            this.progress = progress;
            this.extraInfo = extraInfo;
        }

        public float progress;
        public string extraInfo;
    }

    private static UILoadingController s_loadingController;

    private Transform _bgNode { get; set; }
    private Func<LoadingData> _progressUpdater;
    private SliderZero _sliderZero;
    private Image _logo;
    private RawImage _bgImage;
    private LoadingStep _currentStep;
    private LocalizeTextMeshProUGUI _userIdText;

    private int _currentWorldIndex { get; set; }

    private Animator _animator;

    private Dictionary<int, bool> _sendFlag = new Dictionary<int, bool>();

    private void Awake()
    {
        _bgNode = transform.Find("bgNode");

        _sliderZero = gameObject.GetOrCreateComponent<SliderZero>();
        _sliderZero.Slider = transform.Find("Slider").GetComponent<Slider>();
        _sliderZero.Slider.value = 0f;
        var progressTMP = transform.Find("Slider/progressinfo");
        _sliderZero.ProgressText = progressTMP.GetComponent<TextMeshProUGUI>();

        _logo = transform.Find("Logo").GetComponent<Image>();
        _logo.sprite = Resources.Load<Sprite>("Launcher/Textures/ui_loading_logo");
        
        _bgImage = transform.Find("Image").GetComponent<RawImage>();
        if (LoadingPanel.UseOldLoadingImage())
        {
            _bgImage.texture = Resources.Load<Texture>("Launcher/Textures/ui_landing_bg_old");
        }
        else
        {
            _bgImage.texture = Resources.Load<Texture>("Launcher/Textures/ui_landing_bg");
            _logo.gameObject.SetActive(false);
        }

        _userIdText = transform.Find("UIDGroup/UIDText").GetComponent<LocalizeTextMeshProUGUI>();

        
        Debug.Log("Loading View Open!");
    }

    private void OnEnable()
    {
        _sendFlag.Clear();
        if (MyMain.myGame.Fsm.CurrentState is StateLaunch)
        {
            var progressDes = $"{LocalizationManager.Instance.GetLocalizedStringWithFormats("&key.UI_loading_init", "0")}";
            _sliderZero.ProgressText.SetText(progressDes);
        }
        else
        {
            var progressDes = $"{LocalizationManager.Instance.GetLocalizedStringWithFormats("&key.UI_loading_load", "0")}";
            _sliderZero.ProgressText.SetText(progressDes);
        }

        ReloadFromData();
    }

    private void FixedUpdate()
    {
        if (_progressUpdater != null)
        {
            var data = _progressUpdater.Invoke();
            var newProgress = Mathf.Max(data.progress, _sliderZero.Slider.value);
            if (!Mathf.Approximately(_sliderZero.Slider.value, newProgress))
            {
                _sliderZero.Slider.value = newProgress;
                var displayProgress = (int)(newProgress * 100f);
                var desKey = "&key.UI_loading_init";
                switch (_currentStep)
                {
                    case LoadingStep.Launching:
                        desKey = "&key.UI_loading_init";
                        break;
                    case LoadingStep.DownloadRes:
                        desKey = "&key.UI_loading_upgrade";
                        break;
                    case LoadingStep.InitScene:
                        desKey = "&key.UI_loading_load";
                        break;
                }

                var progressDes = $"{LocalizationManager.Instance.GetLocalizedStringWithFormats(desKey, displayProgress.ToString("#0"))} {data.extraInfo}";
                _sliderZero.ProgressText.SetText(progressDes);
            }
        }
    }

    public static void Show(Func<LoadingData> progressUpdater)
    {
        var prefab = ResourcesManager.Instance.LoadResource<GameObject>("Prefabs/UI/UICommon/UILoading");
        var loadingObj = GameObject.Instantiate(prefab);
        loadingObj.transform.SetParent(UIRoot.Instance.mRootCanvas.transform);
        (loadingObj.transform as RectTransform).SetFullRect();
        loadingObj.name = "UILoading";
        s_loadingController = loadingObj.AddComponent<UILoadingController>();
        s_loadingController._progressUpdater = progressUpdater;
    }

    public static void SetStep(LoadingStep step)
    {
        s_loadingController?.setCurrentStep(step);
    }

    private void setCurrentStep(LoadingStep step)
    {
        _currentStep = step;
        _sliderZero.Slider.value = 0;
    }

    public static void Hide(bool unloadBg)
    {
        if (s_loadingController != null)
        {
            Resources.UnloadAsset(s_loadingController._bgImage.texture);
            GameObject.Destroy(s_loadingController.gameObject);
            s_loadingController = null;
        }
    }

    private void ReloadFromData()
    {
        CommonUtils.DestroyAllChildren(_bgNode);
    }

    private void OnDestroy()
    {
        GameObject.Destroy(_logo);
        GameObject.Destroy(_bgImage);
    }

}