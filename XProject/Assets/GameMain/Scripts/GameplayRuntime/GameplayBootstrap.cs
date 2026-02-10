using Modules.TripleMerge;

namespace GameplayRuntime
{
    public static class GameplayBootstrap
    {
        private static bool s_builtInRegistered;

        public static void RegisterBuiltInModules()
        {
            if (s_builtInRegistered)
            {
                return;
            }

            GameplayCatalog.Instance.RegisterModule(new TripleMergeModule());
            GameplayCatalog.Instance.SetDefaultGameplay(GameplayIds.TripleMerge);

            s_builtInRegistered = true;
        }
    }
}
