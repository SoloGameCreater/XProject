using System.Collections.Generic;
using SaveFile;

namespace GameplayRuntime
{
    public class GameplayCatalog
    {
        private static readonly GameplayCatalog s_instance = new();
        public static GameplayCatalog Instance => s_instance;

        private readonly Dictionary<string, IGameplayModule> _moduleMap = new();
        private readonly List<IGameplayModule> _modules = new();
        private string _defaultGameplayId;

        public IReadOnlyList<IGameplayModule> Modules => _modules;
        public string DefaultGameplayId => _defaultGameplayId;

        public bool RegisterModule(IGameplayModule module)
        {
            if (module == null || string.IsNullOrEmpty(module.Id))
            {
                return false;
            }

            if (_moduleMap.ContainsKey(module.Id))
            {
                return false;
            }

            _moduleMap.Add(module.Id, module);
            _modules.Add(module);
            return true;
        }

        public void SetDefaultGameplay(string gameplayId)
        {
            if (!string.IsNullOrEmpty(gameplayId) && _moduleMap.ContainsKey(gameplayId))
            {
                _defaultGameplayId = gameplayId;
            }
        }

        public bool TryGetModule(string gameplayId, out IGameplayModule module)
        {
            return _moduleMap.TryGetValue(gameplayId, out module);
        }

        public bool TryGetDefaultModule(out IGameplayModule module)
        {
            module = null;
            if (_modules.Count == 0)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(_defaultGameplayId) && _moduleMap.TryGetValue(_defaultGameplayId, out module))
            {
                return true;
            }

            module = _modules[0];
            for (int i = 1; i < _modules.Count; i++)
            {
                if (_modules[i].Priority > module.Priority)
                {
                    module = _modules[i];
                }
            }

            _defaultGameplayId = module.Id;
            return true;
        }

        public void InstallModules(IGameplayInstaller installer)
        {
            if (installer == null)
            {
                return;
            }

            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].Install(installer);
            }
        }

        public void CollectSaveFiles(List<SaveFileBase> saveFiles)
        {
            if (saveFiles == null)
            {
                return;
            }

            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].RegisterSaveFiles(saveFiles);
            }
        }
    }
}
