using GameFramework.DataTable;
using System;
using UnityEngine;

namespace StarForce
{
    public class BuffData : EntityData
    {
        [SerializeField]
        private float m_Addition = 0;

        [SerializeField]
        private float m_Ratio = 0;
        
        [SerializeField]
        private bool m_IsAdd = false;
        
        [SerializeField]
        private int m_EffectId = 0;
        
        [SerializeField]
        private int m_SoundId = 0;
        
        public float Addition => m_Addition;
        public float Ratio => m_Ratio;
        public bool IsAdd => m_IsAdd;
        public int EffectId => m_EffectId;
        public int SoundId => m_SoundId;

        public BuffData(int entityId, int typeId)
            : base(entityId, typeId)
        {
            IDataTable<DRBuff> dtBuff = GameEntry.DataTable.GetDataTable<DRBuff>();
            DRBuff drAsteroid = dtBuff.GetDataRow(TypeId);
            if (drAsteroid == null)
            {
                return;
            }

            m_Addition = drAsteroid.Addition;
            m_Ratio = drAsteroid.Ratio;
            m_IsAdd = drAsteroid.IsAdd == 1;
            m_EffectId = drAsteroid.EffectId;
            m_SoundId = drAsteroid.SoundId;
        }
    }
}