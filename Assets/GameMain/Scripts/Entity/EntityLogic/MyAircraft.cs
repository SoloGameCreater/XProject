//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class MyAircraft : Aircraft
    {
        [SerializeField] private MyAircraftData m_MyAircraftData = null;

        public const int FollowAircraftLimit = 2;
        private Rect m_PlayerMoveBoundary = default(Rect);
        private Vector3 m_TargetPosition = Vector3.zero;
        private bool m_IsMoving = false;
        private List<FollowAircraft> m_ListFollowAircraft = new();
        public bool IsMoving => m_IsMoving;

        /// <summary>
        /// 僚机数量。
        /// </summary>
        public int FollowAircraftCnt => m_ListFollowAircraft.Count;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_MyAircraftData = userData as MyAircraftData;
            if (m_MyAircraftData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }

            ScrollableBackground sceneBackground = FindObjectOfType<ScrollableBackground>();
            if (sceneBackground == null)
            {
                Log.Warning("Can not find scene background.");
                return;
            }

            m_PlayerMoveBoundary = new Rect(sceneBackground.PlayerMoveBoundary.bounds.min.x, sceneBackground.PlayerMoveBoundary.bounds.min.z,
                sceneBackground.PlayerMoveBoundary.bounds.size.x, sceneBackground.PlayerMoveBoundary.bounds.size.z);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (Input.GetMouseButton(0))
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_TargetPosition = new Vector3(point.x, 0f, point.z);
            }

            foreach (var weapon in m_Weapons)
            {
                weapon.TryAttack();
            }

            var direction = m_TargetPosition - CachedTransform.localPosition;
            if (direction.sqrMagnitude <= Vector3.kEpsilon)
            {
                m_IsMoving = false;
                return;
            }

            m_IsMoving = true;
            Vector3 speed = Vector3.ClampMagnitude(direction.normalized * m_MyAircraftData.Speed * elapseSeconds, direction.magnitude);
            CachedTransform.localPosition = new Vector3
            (
                Mathf.Clamp(CachedTransform.localPosition.x + speed.x, m_PlayerMoveBoundary.xMin, m_PlayerMoveBoundary.xMax),
                0f,
                Mathf.Clamp(CachedTransform.localPosition.z + speed.z, m_PlayerMoveBoundary.yMin, m_PlayerMoveBoundary.yMax)
            );
        }

        /// <summary>
        /// 获得buff
        /// </summary>
        /// <param name="buffData"></param>
        public void GetBuff(BuffData buffData)
        {
        }

        public void AddAircraft(FollowAircraft aircraft)
        {
            m_ListFollowAircraft.Add(aircraft);
        }

        public void UpgradeAircraft()
        {
            RemoveAircraft(FollowAircraftLimit);
            foreach (var weapon in m_Weapons)
            {
                weapon.Upgrade();
            }
        }
        public void RemoveAircraft(int num = 1)
        {
            if(num <= 0) return;
            var actualNumToRemove = Math.Min(num, m_ListFollowAircraft.Count);
            
            for (int i = m_ListFollowAircraft.Count - 1; i >= m_ListFollowAircraft.Count - actualNumToRemove; i--)
            {
                GameModule.Entity.HideEntity(m_ListFollowAircraft[i]);
            }
            m_ListFollowAircraft.RemoveRange(m_ListFollowAircraft.Count - actualNumToRemove, actualNumToRemove);
        }
    }
}