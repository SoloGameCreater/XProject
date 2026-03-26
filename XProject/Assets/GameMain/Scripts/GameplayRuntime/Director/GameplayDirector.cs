using System.Threading.Tasks;
using UnityEngine;

namespace GameplayRuntime
{
    public class GameplayDirector
    {
        private static readonly GameplayDirector s_instance = new();
        public static GameplayDirector Instance => s_instance;

        private IGameplayContext _context;
        private IGameplaySession _currentSession;
        private string _currentGameplayId;

        public string CurrentGameplayId => _currentGameplayId;
        public IGameplaySession CurrentSession => _currentSession;

        public void Initialize(IGameplayContext context)
        {
            _context = context;
        }

        public async Task<bool> EnterDefaultAsync(object param = null)
        {
            if (!GameplayCatalog.Instance.TryGetDefaultModule(out var defaultModule))
            {
                Debug.LogError("GameplayDirector EnterDefaultAsync failed, no default module.");
                return false;
            }

            return await EnterAsync(defaultModule.Id, param);
        }

        public async Task<bool> EnterAsync(string gameplayId, object param = null)
        {
            if (string.IsNullOrEmpty(gameplayId))
            {
                return false;
            }

            if (_currentSession != null && _currentGameplayId == gameplayId)
            {
                return true;
            }

            var hasActiveSession = _currentSession != null;
            ExitCurrentSession();

            if (!GameplayCatalog.Instance.TryGetModule(gameplayId, out var module))
            {
                Debug.LogError($"GameplayDirector EnterAsync failed, module not found: {gameplayId}");
                return false;
            }

            _currentSession = module.CreateSession(_context);
            if (_currentSession == null)
            {
                Debug.LogError($"GameplayDirector EnterAsync failed, create session failed: {gameplayId}");
                return false;
            }

            var enterResult = await _currentSession.EnterAsync(param);
            if (!enterResult)
            {
                _currentSession.Exit();
                _currentSession = null;
                _currentGameplayId = null;
                return false;
            }

            _currentGameplayId = gameplayId;
            if (hasActiveSession)
            {
                _currentSession.EnterFinish();
            }

            return true;
        }

        public void EnterFinish()
        {
            _currentSession?.EnterFinish();
        }

        public void Update(float deltaTime)
        {
            _currentSession?.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            _currentSession?.LateUpdate(deltaTime);
        }

        public void ExitCurrentSession()
        {
            _currentSession?.Exit();
            _currentSession = null;
            _currentGameplayId = null;
        }

        public bool IsInGameplay(string gameplayId)
        {
            return !string.IsNullOrEmpty(gameplayId)
                   && _currentSession != null
                   && _currentGameplayId == gameplayId;
        }
    }
}
