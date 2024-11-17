
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [AssetAddress("UIForms/AboutForm")]
    public class AboutForm : UIPopup
    {
        /* 关闭按钮 */
        [ComponentBinder("BackButton")] private Button _btnClose;
        [ComponentBinder("Content")] private RectTransform _contentTransform;
        
        private float m_ScrollSpeed = 1f;

        private float m_InitPosition = 0f;

        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);
            OnInit();
            _btnClose.onClick.AddListener(DoViewClose);
            
            TimeTickModel.Instance.Register(OnTick);
            // 换个音乐
            GameModule.Sound.PlayMusic(3);
        }

        public override Task OnViewClose()
        {
            _btnClose.onClick.RemoveListener(DoViewClose);
            // 还原音乐
            GameModule.Sound.PlayMusic(1);
            
            TimeTickModel.Instance.Remove(OnTick);
            return base.OnViewClose();
        }
        private void OnInit()
        {
            CanvasScaler canvasScaler = transform.GetComponentInParent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Log.Warning("Can not find CanvasScaler component.");
                return;
            }

            m_InitPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;
            

            _contentTransform.SetLocalPositionY(m_InitPosition);
        }
        

        private void OnTick()
        {
            _contentTransform.AddLocalPositionY(m_ScrollSpeed * 1000);
            if (_contentTransform.localPosition.y > _contentTransform.sizeDelta.y - m_InitPosition)
            {
                _contentTransform.SetLocalPositionY(m_InitPosition);
            }
        }
    }
}