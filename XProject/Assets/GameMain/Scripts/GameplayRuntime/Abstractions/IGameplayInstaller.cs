namespace GameplayRuntime
{
    public interface IGameplayInstaller
    {
        T AddSubSystem<T>(string groupName = null) where T : new();
    }
}
