using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class FollowAircraft : Aircraft
    {
        [SerializeField] private FollowAircarftData m_AircraftData = null;

        private Vector3 m_TargetPosition = Vector3.zero;
        private MyAircraft m_MyAircraft = null;
        private float m_MyAircraftWidth = 0;

        public MyAircraft MyAircraft
        {
            get => m_MyAircraft;
            set
            {
                m_MyAircraft = value;
                m_TargetPosition = m_MyAircraft.CachedTransform.localPosition;
                var coll = m_MyAircraft.GetComponent<Collider>();
                m_MyAircraftWidth = coll.bounds.size.x / 2;
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
                m_TargetPosition = m_MyAircraft.CachedTransform.localPosition + new Vector3(m_MyAircraftWidth, 0, 0);
            }

            Vector3 direction = m_TargetPosition - CachedTransform.localPosition;
            if (direction.sqrMagnitude <= Vector3.kEpsilon)
            {
                return;
            }

            var speed = Vector3.ClampMagnitude(direction.normalized * m_AircraftData.Speed * elapseSeconds, direction.magnitude);
            CachedTransform.localPosition = new Vector3
            (
                CachedTransform.localPosition.x + speed.x,
                0f,
                CachedTransform.localPosition.z + speed.z
            );
        }
    }
}