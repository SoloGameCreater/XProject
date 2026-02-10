using Framework;

namespace GameplayRuntime
{
    public class GameplayInstaller : IGameplayInstaller
    {
        private readonly SubSystemManager _subSystemManager;
        private readonly string _defaultGroupName;

        public GameplayInstaller(SubSystemManager subSystemManager, string defaultGroupName)
        {
            _subSystemManager = subSystemManager;
            _defaultGroupName = defaultGroupName;
        }

        public T AddSubSystem<T>(string groupName = null) where T : new()
        {
            if (_subSystemManager == null)
            {
                return default;
            }

            return _subSystemManager.AddSubSystem<T>(string.IsNullOrEmpty(groupName) ? _defaultGroupName : groupName);
        }
    }
}
