using UnityEngine;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace StarForce
{
    public class FollowAircraft : Aircraft
    {
        [SerializeField] private FollowAircarftData m_AircraftData = null;

        private Vector3 m_TargetPosition = Vector3.zero;

        private MyAircraft m_MyAircraft = null;

        private Vector3 m_OffsetPosition = Vector3.zero;

        private float m_FollowDelay = 0;
        private float m_RemindTime = 0;

        private bool m_IsSelfMoving = false;
        private Tweener m_MoveTweener;
        public MyAircraft MyAircraft
        {
            get => m_MyAircraft;
            set
            {
                m_MyAircraft = value;
                m_TargetPosition = m_MyAircraft.CachedTransform.localPosition;
                var coll = m_MyAircraft.GetComponent<Collider>();
                //m_MyAircraftWidth = coll.bounds.size.x / 2;
                var sign = m_MyAircraft.FollowAircraftCnt % 2 == 0 ? -1 : 1;
                // ReSharper disable once PossibleLossOfFraction
                var offset = sign * (1 + (m_MyAircraft.FollowAircraftCnt - 1) / 2);
                var offsetX = offset * 0.5f;
                var offsetZ = -Mathf.Abs(offset) * 0.5f;
                Log.Warning($"offset is {offset}");
                m_OffsetPosition = new Vector3(offsetX, 0, offsetZ);
                m_TargetPosition = m_MyAircraft.CachedTransform.localPosition + m_OffsetPosition;
                m_FollowDelay = 0.1f * m_MyAircraft.FollowAircraftCnt;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_AircraftData = userData as FollowAircarftData;
            if (m_AircraftData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_MyAircraft == null) return;

            foreach (var weapon in m_Weapons)
            {
                weapon.TryAttack();
            }
            
            if (m_MyAircraft.IsMoving)
            {
                m_TargetPosition = m_MyAircraft.CachedTransform.localPosition + m_OffsetPosition;
            }

            if (!m_IsSelfMoving)
            {
                MoveToTargetWithDelay();
            }
            // Vector3 direction = m_TargetPosition - CachedTransform.localPosition;
            // if (direction.sqrMagnitude <= Vector3.kEpsilon)
            // {
            //     m_IsSelfMoving = false;
            //     return;
            // }
            //
            // m_IsSelfMoving = true;
            // var speed = Vector3.ClampMagnitude(direction.normalized * m_AircraftData.Speed * elapseSeconds, direction.magnitude);
            // CachedTransform.localPosition = new Vector3
            // (
            //     CachedTransform.localPosition.x + speed.x,
            //     -1f,
            //     CachedTransform.localPosition.z + speed.z
            // );
        }

        private void MoveToTargetWithDelay()
        {
            // 使用 DOTween 动画到目标位置
            m_MoveTweener = CachedTransform.DOLocalMove(m_TargetPosition, 0.5f)
                .SetDelay(m_FollowDelay) // 延迟执行
                .SetEase(Ease.Linear) // 可以根据需要选择不同的缓动函数
                .OnStart(() => m_IsSelfMoving = true) // 开始时设置为正在移动
                .OnComplete(() => m_IsSelfMoving = false); // 完成时重置为不在移动
        }
    }
}