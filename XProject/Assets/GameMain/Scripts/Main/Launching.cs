
using System.Threading.Tasks;
using Framework;
using UnityEngine;
using Localizetion;


public class Launching
{
    private bool _firstCopyFilesDone;
    bool ClickedBackButton { get; set; }

    // public float ToProgress
    // {
    //     get => _toProgress;
    //     set => _toProgress = value;
    // }

    //------------------------- 进度控制 -------------------------//
    private float _progress = 0f;
    public static float ToProgress = 0f;

    private bool stopLanchingSequence = false;

    //private Result<ulong> APTRecordResult;

    public Launching()
    {
        RegistEvent();

        startLaunchSequence();
    }

    ~Launching()
    {
        UnRegistEvent();
    }

    private void RegistEvent()
    {
    }

    private void UnRegistEvent()
    {
    }

    private void initLoadUI()
    {
        UILoadingController.Show(() => new UILoadingController.LoadingData(_progress / 100f, string.Empty));
        UILoadingController.SetStep(UILoadingController.LoadingStep.Launching);
    }

    public void Restart()
    {
        startLaunchSequence();
    }

    public static bool LaunchingInitFileFinish = false;

    private async void startLaunchSequence()
    {
        _progress = 0f;
        ToProgress = 0f;

        //本地化配置
        LocalizationManager.Instance.MatchLanguage();
        LocaleConfigManager.Instance.InitConfigs();

        //添加loading页
        initLoadUI();
        ToProgress += 10;
        LaunchingInitFileFinish = true;

        await Task.Delay(300);

        var progressValue = 20f;
        var residueProgress = progressValue;
        
        ToProgress += residueProgress;
        ToProgress = Mathf.Ceil(ToProgress);

        //加载配置文件
        await Task.Delay(300);
        await LoadConfigs();
        ToProgress += 10;
        ToProgress += 10;
        ToProgress += 10;

        //强行把进度条填满
        ToProgress = 100;
        MyMain.myGame.InitManagers();
    }

    public void Update()
    {
        if (_progress < ToProgress)
        {
            if (ToProgress < 100f)
            {
                float diffValue = ToProgress - _progress;
                float deltaValue = 0.02f;

                if (ToProgress > 100f) // 资源文件更新完后，进度条最小走1
                    deltaValue = Mathf.Max(1.0f, diffValue * 0.1f);
                else if (diffValue < 0.5f) //资源下载过程中，进度条精确到小数点后2位
                    deltaValue = Mathf.Max(0.02f, diffValue * 0.1f);
                else
                    deltaValue = Mathf.Max(0.1f, diffValue * 0.1f);

                _progress += deltaValue;
                _progress = Mathf.Min(100f, _progress);
            }
            else
            {
                _progress += 2;
                _progress = Mathf.Min(100f, _progress);
            }
        }

        if (_progress >= 100f)
        {
            Main.Game.Fsm.ChangeState(FsmStateType.Gameplay, null);
        }
    }
    
    private async Task LoadConfigs()
    {
        // 读配置表
        LocalizationManager.Instance.MatchLanguage();
    }
}
