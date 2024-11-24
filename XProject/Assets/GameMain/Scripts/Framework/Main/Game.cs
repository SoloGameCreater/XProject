
using UnityEngine;

namespace Framework
{
    public abstract class Game
    {
        protected readonly SubSystemManager _subSystemManager = new SubSystemManager();
        protected Fsm _fsm;

        protected abstract void OnUpdate(float deltaTime);
        protected abstract void OnLateUpdate(float deltaTime);
        protected abstract void OnInit();

        public Fsm Fsm { get => _fsm; set => _fsm = value; }

        public void Init()
        {
            _fsm = new Fsm();
            _subSystemManager.Init();
            OnInit();
        }

        public void Start()
        {
            _subSystemManager.Start();
        }

        public void Update()
        {
            _subSystemManager.Update(Time.deltaTime);
            _fsm.Update(Time.deltaTime);
            OnUpdate(Time.deltaTime);
        }

        public void LateUpdate()
        {
            _subSystemManager.LateUpdate(Time.deltaTime);
            _fsm.LateUpdate(Time.deltaTime);
            OnLateUpdate(Time.deltaTime);
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            _subSystemManager.OnApplicationPause(pauseStatus);
        }

        public void Release()
        {
            _subSystemManager.Release();
        }

        public bool IsThreeMerge()
        {
            return _fsm.CurrentState is StateThreeMerge;
        }
        
        /// <summary>
        /// 是否刚刚在主线玩法中胜利
        /// </summary>
        /// <returns></returns>
        public bool IsJustWinInTMatch()
        {
            return MyMain.myGame.IsThreeMerge()
                   && MyMain.myGame.Fsm.PreviousState.Type == FsmStateType.MainGame;
        }
    }
}