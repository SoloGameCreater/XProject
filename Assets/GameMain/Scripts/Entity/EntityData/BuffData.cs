using GameFramework.DataTable;
using System;
using UnityEngine;

namespace StarForce
{
    public class BuffData : EntityData
    {
        [SerializeField] private int m_Addition = 0;

        [SerializeField] private int m_Ratio = 0;

        [SerializeField] private bool m_IsAdd = false;

        [SerializeField] private int m_EffectId = 0;

        [SerializeField] private int m_SoundId = 0;

        public int Addition => m_Addition;
        public int Ratio => m_Ratio;
        public bool IsAdd => m_IsAdd;
        public int EffectId => m_EffectId;
        public int SoundId => m_SoundId;

        public BuffData(int entityId, int typeId,int buffId)
            : base(entityId, typeId)
        {
            IDataTable<DRBuff> dtBuff = GameEntry.DataTable.GetDataTable<DRBuff>();
            DRBuff drBuff = dtBuff.GetDataRow(buffId);
            if (drBuff == null)
            {
                return;
            }

            m_Addition = drBuff.Addition;
            m_Ratio = drBuff.Ratio;
            m_IsAdd = drBuff.IsAdd == 1;
            m_EffectId = drBuff.EffectId;
            m_SoundId = drBuff.SoundId;
        }
    }
}