using System;
using System.Threading.Tasks;

namespace Framework
{
    public enum FsmStateType
    {
        Non,
        Launch,      //启动的loading
        ThreeMerge,  //合成/兼大厅
        MainGame,    //主游戏
    }
    public interface IFsmState
    {
        FsmStateType Type { get; }

        //转场退场动画开始时已进入当前状态
        Task<bool> PreEnterAsync(FsmParam param);
        void EnterFinish();
        void Update(float deltaTime);
        void LateUpdate(float deltaTime);            
        void Exit(FsmStateType toStateType);
    }
}