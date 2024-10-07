//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class SurvivalGame : GameBase
    {
        private float m_ElapseSeconds = 0f;
        private float m_LastCreateBuffSeconds = 0f;

        public override GameMode GameMode
        {
            get { return GameMode.Survival; }
        }

        private const int BuffStartId = 80000;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            m_LastCreateBuffSeconds += elapseSeconds;
            if (m_ElapseSeconds >= 1f)
            {
                m_ElapseSeconds = 0f;
                IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
                float randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x +
                                        SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
                float randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z +
                                        SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
                GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(dtAsteroid.Count))
                {
                    Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                });
            }

            /* 每隔5秒创建两个buff */
            if (m_LastCreateBuffSeconds >= 5f)
            {
                m_LastCreateBuffSeconds = 0f;
                IDataTable<DRBuff> dtBuff = GameEntry.DataTable.GetDataTable<DRBuff>();
                /* 随机算法说明
                 * 每次都会随机两个buff，其中一个一定是增益buff,避免从两个减益buff中选择
                 * 如果第一次随机出的是减益buff，那么下一次一定是增益buff
                 * 允许两个增益buff同时存在
                 */
                var index_1 = Utility.Random.GetRandom(dtBuff.Count);
                var buff_1 = dtBuff.GetDataRow(BuffStartId + index_1);
                GameEntry.Entity.ShowBuff(new BuffData(GameEntry.Entity.GenerateSerialId(), BuffStartId, buff_1.Id)
                {
                    Position = new Vector3(3, 0f, 15f),
                });
                var typeId_2 = Utility.Random.GetRandom(dtBuff.Count);
                if (buff_1.IsAdd == 0)
                {
                    var addDtBuffs = dtBuff.GetDataRows((x) => { return x.IsAdd == 1; });
                    typeId_2 = Utility.Random.GetRandom(addDtBuffs.Length);
                }

                var buff_2 = dtBuff.GetDataRow(BuffStartId + typeId_2);

                GameEntry.Entity.ShowBuff(new BuffData(GameEntry.Entity.GenerateSerialId(), BuffStartId, buff_2.Id)
                {
                    Position = new Vector3(-3, 0f, 15f),
                });
            }
        }
    }
}