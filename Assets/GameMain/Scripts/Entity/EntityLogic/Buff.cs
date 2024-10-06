
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Buff : Entity
    {
        private const float Speed = 5f;
#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.back * Speed * elapseSeconds, Space.World);
        }
    }
}