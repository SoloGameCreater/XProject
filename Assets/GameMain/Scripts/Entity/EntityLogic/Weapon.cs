
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class Weapon : Entity
    {
        private const string AttachPoint = "Weapon Point";

        [SerializeField] private WeaponData m_WeaponData = null;

        private float m_NextAttackTime = 0f;
        private float m_AttackInterval = 0f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            m_AttackInterval = m_WeaponData.AttackInterval;
            GameModule.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachPoint);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }

        public void TryAttack()
        {
            if (Time.time < m_NextAttackTime)
            {
                return;
            }

            m_NextAttackTime = Time.time + m_AttackInterval;
            GameModule.Entity.ShowBullet(new BulletData(GameModule.Entity.GenerateSerialId(), m_WeaponData.BulletId, m_WeaponData.OwnerId, m_WeaponData.OwnerCamp,
                m_WeaponData.Attack, m_WeaponData.BulletSpeed)
            {
                Position = CachedTransform.position,
            });
            GameModule.Sound.PlaySound(m_WeaponData.BulletSoundId);
        }

        public void Upgrade()
        {
            if (m_AttackInterval <= 0.001f) return;
            
            m_AttackInterval /= 2;
        }
    }
}