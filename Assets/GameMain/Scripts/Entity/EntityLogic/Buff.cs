using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Buff : Entity
    {
        [SerializeField] private BuffData m_BuffData = null;
        public BuffData BuffDataInfo => m_BuffData;
        private const float Speed = 5f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_BuffData = userData as BuffData;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.back * Speed * elapseSeconds, Space.World);
        }
    }
}