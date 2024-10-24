using System.Text;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Buff : Entity
    {
        [SerializeField] private BuffData m_BuffData = null;
        private Transform m_AddTrans = null;
        private Transform m_ReduceTrans = null;
        private TextMeshPro m_Text = null;
        public BuffData BuffDataInfo => m_BuffData;
        private const float Speed = 5f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_BuffData = userData as BuffData;
            m_AddTrans = transform.Find("Add");
            m_ReduceTrans = transform.Find("Reduce");
            m_Text = transform.Find("Text")?.GetComponent<TextMeshPro>();
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_BuffData = userData as BuffData;
            if (m_BuffData == null)
            {
                Log.Error("Buff data is invalid.");
                return;
            }
            m_AddTrans.gameObject.SetActive(m_BuffData.IsAdd);
            m_ReduceTrans.gameObject.SetActive(!m_BuffData.IsAdd);
            StringBuilder displayStr = new StringBuilder();
            if (m_BuffData.Addition > 0)
            {
                var signStr = m_BuffData.IsAdd ? "+" : "-";
                displayStr.Append(signStr);
                displayStr.Append(m_BuffData.Addition);
            }
            else
            {
                var signStr = m_BuffData.IsAdd ? "x" : "÷";
                displayStr.Append(signStr);
                displayStr.Append(m_BuffData.Ratio);
            }
            //todo 后期需要调整功能
            #region 临时写法
            displayStr.Clear();
            displayStr.Append(m_BuffData.IsAdd ? "+1" : "-1");
            #endregion
            
            m_Text.text = displayStr.ToString();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.back * Speed * elapseSeconds, Space.World);
        }
    }
}