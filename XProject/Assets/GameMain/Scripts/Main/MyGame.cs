
using Framework;
using GameplayRuntime;
using Gameplay.SubSystems;
using RoleSystem;
using Game = Framework.Game;

public class MyGame : Game
{
    private const float UPDATE_INTERVAL = 1.0f / 60.0f; // 逻辑帧率(1秒30帧)
    private float _elapsed;

    protected override void OnInit()
    {
        GameplayBootstrap.RegisterBuiltInModules();
        GameplayDirector.Instance.Initialize(new GameplayContext(this));

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

            //角色系统
            _subSystemManager.AddSubSystem<RoleManager>();

            //玩法模块安装
            GameplayCatalog.Instance.InstallModules(new GameplayInstaller(_subSystemManager, "Gameplay"));

            //Model
            _subSystemManager.AddSubSystem<SaveFileSystem>();
            _subSystemManager.AddSubSystem<CurrencyModel>();
        }
    }

    private void initFsm()
    {
        _fsm.RigsterState(FsmStateType.Launch, new StateLaunch());
        _fsm.RigsterState(FsmStateType.Gameplay, new StateGameplay());
        // 过渡期兼容
        _fsm.RigsterState(FsmStateType.TripleMerge,  new StateTripleMerge());
    }
}
