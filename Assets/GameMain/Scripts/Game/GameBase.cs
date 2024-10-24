
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode { get; }

        protected ScrollableBackground SceneBackground { get; private set; }

        public bool GameOver { get; protected set; }

        private MyAircraft m_MyAircraft = null;

        public virtual void Initialize()
        {
            GameModule.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameModule.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameModule.Event.Subscribe(BuffOnTriggerEventArgs.EventId, OnMyAircraftAddBuff);

            SceneBackground = Object.FindObjectOfType<ScrollableBackground>();
            if (SceneBackground == null)
            {
                Log.Warning("Can not find scene background.");
                return;
            }

            SceneBackground.VisibleBoundary.gameObject.GetOrAddComponent<HideByBoundary>();
            GameModule.Entity.ShowMyAircraft(new MyAircraftData(GameModule.Entity.GenerateSerialId(), 10000)
            {
                Name = "My Aircraft",
                Position = Vector3.zero,
            });

            GameOver = false;
            m_MyAircraft = null;
        }

        public virtual void Shutdown()
        {
            GameModule.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameModule.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameModule.Event.Unsubscribe(BuffOnTriggerEventArgs.EventId, OnMyAircraftAddBuff);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_MyAircraft != null && m_MyAircraft.IsDead)
            {
                GameOver = true;
                return;
            }
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(MyAircraft))
            {
                m_MyAircraft = (MyAircraft)ne.Entity.Logic;
            }
            else if (ne.EntityLogicType == typeof(FollowAircraft))
            {
                var aircraft = (FollowAircraft)ne.Entity.Logic;
                aircraft.MyAircraft = m_MyAircraft;
                m_MyAircraft.AddAircraft(aircraft);
            }
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        protected virtual void OnMyAircraftAddBuff(object sender, GameEventArgs e)
        {
            if (m_MyAircraft == null) return;

            BuffOnTriggerEventArgs ne = (BuffOnTriggerEventArgs)e;

            if (ne.UserData.IsAdd)
            {
                if (m_MyAircraft.FollowAircraftCnt >= MyAircraft.FollowAircraftLimit)
                {
                    m_MyAircraft.UpgradeAircraft();
                }
                else
                {
                    GameModule.Entity.ShowFollowAircraft(new FollowAircarftData(GameModule.Entity.GenerateSerialId(), 10001)
                        { Position = new Vector3(0, 0, -10f) });
                }
            }
            else
            {
                m_MyAircraft.RemoveAircraft();
            }
        }
    }
}