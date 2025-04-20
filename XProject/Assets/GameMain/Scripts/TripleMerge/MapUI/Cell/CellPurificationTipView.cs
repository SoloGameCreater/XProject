using System;
using Localizetion;
using TripleMerge;
using UnityEngine;
using UnityEngine.UI;

namespace TripleMerge
{
    public class CellPurificationTipView : ITripleMergeMapView
    {
        private const float BASE_CAMERA_SIZE = 20;
        
        public Transform ViewRoot { get; set; }

        private Slider _sliderProgress;

        private LocalizeTextMeshProUGUI _textProgress;
        private int _maxValue;
        private Func<int> _currentProgressGetter;

        private LocalizeTextMeshProUGUI _textDesc;

        public void OnInitialize()
        {
            _textDesc = ViewRoot.Find("Root/DescText").GetComponent<LocalizeTextMeshProUGUI>();
            _textDesc.SetText("Keep merge to unlock this land!");

            _sliderProgress = ViewRoot.Find("Root/Progress").GetComponent<Slider>();

            _textProgress = ViewRoot.Find("Root/Progress/TextProgress").GetComponent<LocalizeTextMeshProUGUI>();
        }

        public void OnShow(params object[] extra)
        {
            var originScale = ViewRoot.localScale;
            var targetSize = TripleMergeSystem.Instance.Gameplay.MapManager.MapCamera.orthographicSize * 1.0f / BASE_CAMERA_SIZE;
            ViewRoot.localScale = originScale * targetSize;

            if (extra.Length > 0)
            {
                var yOffset = (float)extra[0];
                if (yOffset != 0)
                {
                    var pos = ViewRoot.position;
                    pos.y += yOffset * targetSize;
                    ViewRoot.position = pos;
                }

                if (extra.Length > 1)
                {
                    var descId = (string)extra[1];
                    _textDesc.SetTerm(descId);
                }
            }
        }

        public void OnHide()
        {
            
        }

        public void OnUpdate()
        {
            if (Input.anyKeyDown)
            {
                TripleMergeSystem.Instance.Gameplay.MapUIManager.Hide(this);
            }

            if (_currentProgressGetter != null)
            {
                var current = _currentProgressGetter.Invoke();
                _textProgress.SetText($"{current}/{_maxValue}");
                _sliderProgress.value = current * 1.0f / _maxValue;
                if (current >= _maxValue)
                {
                    TripleMergeSystem.Instance.Gameplay.MapUIManager.Hide(this);
                }
            }
        }

        public void SetProgress(int current, int max, Func<int> currentProgressGetter = null)
        {
            _currentProgressGetter = currentProgressGetter;
            _maxValue = max;
            _textProgress.SetText($"{current}/{max}");
            _sliderProgress.value = current * 1.0f / max;
        }
    }
}