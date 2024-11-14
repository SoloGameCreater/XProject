using System;
using System.Threading.Tasks;

namespace Framework
{
    public enum FsmStateType
    {
        Non,
        Launch,      //启动的loading
        Login,       //登录
        Activity,    //运营活动
        ThreeMerge,  //合成
        CG,          //CG视频
        Transition,  //过渡场景
        TripleMatch, //点消
        Makeover,    //医美小游戏
        ASMR,
        Plot,   //小游戏-plot
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