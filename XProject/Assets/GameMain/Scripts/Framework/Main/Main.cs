
using UnityEngine;

namespace Framework
{
    public abstract class Main : MonoBehaviour
    {
        protected static Game _game;
        public static Game Game { get => _game; }

        protected abstract Game createGame();

        private void Awake()
        {
            //initDebug();

            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _game = createGame();
            if (_game == null)
            {
                DebugUtil.LogError("Framework.Main: no game instance created");
            }
            else
            {
                _game.Init();
            }

            DontDestroyOnLoad(gameObject);
        }

        private void initDebug()
        {
#if DEBUG || DEVELOPMENT_BUILD || UNITY_EDITOR
            // if (Debug.isDebugBuild)
            // {
            //     SRDebug.Init();
            // }
            //Debug.unityLogger.logEnabled = true;
            GameObject.Instantiate(ResourcesManager.Instance.LoadResource<GameObject>("Prefabs/UI/UIDebug/DebugProfilePanel"), UIRoot.Instance.mRootCanvas.transform);
#else
            //Debug.unityLogger.logEnabled = false;
#endif
        }

        private void Start()
        {
            _game.Start();
        }

        private void Update()
        {
            _game.Update();
        }

        private void LateUpdate()
        {
            _game.LateUpdate();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _game.OnApplicationPause(pauseStatus);
        }

        private void OnDestroy()
        {
            _game.Release();
        }

    }
}