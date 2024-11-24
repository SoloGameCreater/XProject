
using Framework;
using Gameplay.SubSystems;
using Game = Framework.Game;

public class MyGame : Game
{
    private const float UPDATE_INTERVAL = 1.0f / 30.0f; // 逻辑帧率(1秒30帧)
    private float _elapsed;

    protected override void OnInit()
    {
        initSubSystems();
        initFsm();
        _fsm.ChangeState(FsmStateType.Launch, null);
    }

    protected override void OnUpdate(float elapsed)
    {
        //GameGuideMgr?.Update();
        //WorldGuideCk5?.Update();

        _elapsed += elapsed;
        if (_elapsed < UPDATE_INTERVAL) return;

        elapsed = _elapsed;

        //WorldGuideMgr?.Update();

        _elapsed = 0;
    }

    protected override void OnLateUpdate(float elapsed)
    {
    }

    public void InitManagers()
    {
        // 从老CK继承过来的单例类
        CommonSetModel.Instance.Init();
    }

    private void initSubSystems()
    {
        if (_subSystemManager.AddGroup("Framework"))
        {
            _subSystemManager.AddSubSystem<CoroutineManager>();
            _subSystemManager.AddSubSystem<RecycleSystem>();
            _subSystemManager.AddSubSystem<RenderTextureFactory>();
            _subSystemManager.AddSubSystem<UIViewSystem>();
        }

        if (_subSystemManager.AddGroup("Gameplay"))
        {
            //计时器
            _subSystemManager.AddSubSystem<TimeTickModel>();

            _subSystemManager.AddSubSystem<AudioSysManager>();
            _subSystemManager.AddSubSystem<GameSettingSubSystem>();
            _subSystemManager.AddSubSystem<GamePauseManager>();
            //todo 待定
            //_subSystemManager.AddSubSystem<LittleGameManager>();

            //Model
            //_subSystemManager.AddSubSystem<StorageSubSystem>();

            //配置
            //_subSystemManager.AddSubSystem<My.Config.Game.GameConfigManager>();
            
        }
    }

    private void initFsm()
    {
        _fsm.RigsterState(FsmStateType.Launch,      new StateLaunch());
        _fsm.RigsterState(FsmStateType.ThreeMerge,  new StateThreeMerge());
        //TODO 还没想好
        //_fsm.RigsterState(FsmStateType.MainGame,       new StateLogin());
    }
}
