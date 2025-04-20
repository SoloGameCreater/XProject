using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Framework;
using Config.TripleMerge;
using Localizetion;

namespace TripleMerge
{
    /// <summary>
    /// 区域解锁进度视图
    /// </summary>
    public class AreaRegionUnlockProgressView : ITripleMergeMapView
    {
        private const float SPEED = 0.25f;

        private TextMeshProUGUI _textProgress;

        private Slider _sliderProgress;

        private MapAreaRegion _bindRegion;

        public int TotalUnlockedCellNumOfPreRegion { private set; get; }

        public int TotalCellNumOfPreRegion { private set; get; }

        private int _displayUnlockedCellNum;
        public Transform ViewRoot { get; set; }

        #region 生命周期方法
        public void OnInitialize()
        {
            _sliderProgress = ViewRoot.Find("Progress").GetComponent<Slider>();

            _textProgress = ViewRoot.Find("Progress/TextProgress").GetComponent<TextMeshProUGUI>();

            ViewRoot.Find("LockBtn").GetComponent<Button>().onClick.AddListener(OpenTipsView);
        }

        public void OnShow(params object[] extra)
        {
            
        }

        public void OnHide()
        {
            
        }

        public void OnUpdate()
        {
            if (_bindRegion == null)
            {
                return;
            }

            if (_bindRegion.CfgData.IsValid)
            {
                if (_displayUnlockedCellNum >= TotalUnlockedCellNumOfPreRegion)
                {
                    if (TotalUnlockedCellNumOfPreRegion >= TotalCellNumOfPreRegion)
                    {
                        Hide();

                        _bindRegion.BecomeUnlock();
                    }

                    return;
                }
            }

            var nextProgress = _sliderProgress.value + SPEED * Time.deltaTime;

            var targetProgress = TotalUnlockedCellNumOfPreRegion * 1.0f / TotalCellNumOfPreRegion;

            if (nextProgress > targetProgress)
            {
                nextProgress = targetProgress;
            }

            _displayUnlockedCellNum = Mathf.RoundToInt(TotalCellNumOfPreRegion * nextProgress);

            _sliderProgress.value = nextProgress;
            _textProgress.SetText($"{_displayUnlockedCellNum}/{TotalCellNumOfPreRegion}");
        }
        public void SetProgress(int totalUnlockedCellNumOfPreRegion, int totalCellNumOfPreRegion)
        {
            _displayUnlockedCellNum = TotalUnlockedCellNumOfPreRegion = totalUnlockedCellNumOfPreRegion;
            TotalCellNumOfPreRegion = totalCellNumOfPreRegion;

            _textProgress.SetText($"{TotalUnlockedCellNumOfPreRegion}/{TotalCellNumOfPreRegion}");
            _sliderProgress.value = TotalUnlockedCellNumOfPreRegion * 1.0f / TotalCellNumOfPreRegion;
        }

        public void AddProgress(int addNum = 1)
        {
            TotalUnlockedCellNumOfPreRegion += addNum;
            _textProgress.SetText($"{TotalUnlockedCellNumOfPreRegion}/{TotalCellNumOfPreRegion}");
            _sliderProgress.value = TotalUnlockedCellNumOfPreRegion * 1.0f / TotalCellNumOfPreRegion;
        }

        private void OpenTipsView()
        {
            if (!_bindRegion.CfgData.IsValid)
            {
                DebugUtil.LogWarning("区域未解锁");
                return;
            }
        }

        private void Hide()
        {
            TripleMergeSystem.Instance.Gameplay.MapUIManager.Hide(this);
        }

        public void BindRegion(MapAreaRegion bindRegion)
        {
            _bindRegion = bindRegion;
        }
        #endregion
    }
    
}
