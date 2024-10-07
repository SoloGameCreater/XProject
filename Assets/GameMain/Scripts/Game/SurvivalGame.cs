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
            get
            {
                return GameMode.Survival;
            }
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            m_LastCreateBuffSeconds += elapseSeconds;
            if (m_ElapseSeconds >= 1f)
            {
                m_ElapseSeconds = 0f;
                IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
                float randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
                float randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
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
                var typeId_1 = Utility.Random.GetRandom(dtBuff.Count);
                var typeId_2 = Utility.Random.GetRandom(dtBuff.Count);
                Log.Info($"randomID : {typeId_1}, typeID : {typeId_2}");
                GameEntry.Entity.ShowBuff(new BuffData(GameEntry.Entity.GenerateSerialId(), 80000 + typeId_1)
                {
                    Position = new Vector3(3, 0f, 15f),
                });
                GameEntry.Entity.ShowBuff(new BuffData(GameEntry.Entity.GenerateSerialId(), 80000 + typeId_2)
                {
                    Position = new Vector3(-3, 0f, 15f),
                });
            }
        }
    }
}
