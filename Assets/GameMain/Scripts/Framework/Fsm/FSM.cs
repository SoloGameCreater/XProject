using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public class Fsm
    {
        private Dictionary<FsmStateType, IFsmState> _stateDic;

        private IFsmState _currentState;
        private IFsmState _previousState;

        public IFsmState CurrentState => _currentState;
        public IFsmState PreviousState => _previousState;

        public Fsm()
        {
            _stateDic = new Dictionary<FsmStateType, IFsmState>();
        }

        public void RigsterState(FsmStateType type, IFsmState state)
        {
            _stateDic.Add(type, state);
        }

        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            _currentState?.LateUpdate(deltaTime);
        }

        public void ChangeState(FsmStateType newStateType, FsmParam stateParam, bool force = false, Action callBack = null)
        {
            _stateDic.TryGetValue(newStateType, out var newState);
            changeStateAsync(newState, stateParam, force, callBack);
        }

        private async void changeStateAsync(IFsmState newState, FsmParam stateParam, bool force = false, Action onChangeFinised = null)
        {
            if (newState == null)
            {
                Debug.LogError($"ChangeState, new state is null!");
            }
            else if (force || newState != _currentState)
            {
                var needShowLoading =
                    _currentState != null && _currentState.Type == FsmStateType.MainGame && newState.Type == FsmStateType.ThreeMerge ||
                    _currentState != null && _currentState.Type == FsmStateType.ThreeMerge && newState.Type == FsmStateType.MainGame;

                var enterResult = await doChangeToStateAsync(newState, stateParam, onChangeFinised);
                if (enterResult) EnterFinish();
            }
        }

        async Task<bool> doChangeToStateAsync(IFsmState newState, FsmParam stateParam, Action onChangeFinised)
        {
            _previousState = _currentState;
            _currentState?.Exit(newState.Type);
            _currentState = newState;
            var preEnterResult = await _currentState?.PreEnterAsync(stateParam);
            onChangeFinised?.Invoke();

            return preEnterResult;
        }

        void EnterFinish()
        {
            _currentState?.EnterFinish();
        }
    }
}