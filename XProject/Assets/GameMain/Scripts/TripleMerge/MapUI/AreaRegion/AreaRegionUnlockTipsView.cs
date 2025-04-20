
using UnityEngine;
using UnityEngine.UI;
using Localizetion;
using Object = UnityEngine.Object;
using Framework;
using Config.TripleMerge;

namespace TripleMerge
{
    public class AreaRegionUnlockTipsView : ITripleMergeMapView
    {
        private const float BASE_CAMERA_SIZE = 20;

        public Transform ViewRoot { get; set; }

        private MapAreaRegion _bindRegion;

        private LocalizeTextMeshProUGUI _textTips;

        public void OnInitialize()
        {
            _textTips = ViewRoot.Find("Root/DescText").GetComponent<LocalizeTextMeshProUGUI>();
        }

        public void OnShow(params object[] extra)
        {
            var originScale = ViewRoot.localScale;
            var targetSize = TripleMergeSystem.Instance.Gameplay.MapManager.MapCamera.orthographicSize * 1.0f / BASE_CAMERA_SIZE;
            ViewRoot.localScale = originScale * targetSize;

            if (extra.Length > 0)
            {
                var yOffset = (float) extra[0];
                if (yOffset != 0)
                {
                    var pos = ViewRoot.position;
                    pos.y += yOffset * targetSize;
                    ViewRoot.position = pos;
                }
            }
        }

        public void OnHide()
        {
            
        }

        public void BindRegionData(MapAreaRegion region)
        {
            _bindRegion = region;
        }

        public void OnUpdate()
        {
            if (Input.anyKeyDown)
            {
                TripleMergeSystem.Instance.Gameplay.MapUIManager.Hide(this);
            }

            if (_textTips != null && _bindRegion != null && _bindRegion.ProgressView != null)
            {
                _textTips.SetTerm("");
                var text = LocalizationManager.Instance.GetLocalizedStringWithFormat("Merge3_Clean_num",
                    (_bindRegion.ProgressView.TotalCellNumOfPreRegion - _bindRegion.ProgressView.TotalUnlockedCellNumOfPreRegion).ToString());
                _textTips.SetText(text);
            }
        }
    }
}