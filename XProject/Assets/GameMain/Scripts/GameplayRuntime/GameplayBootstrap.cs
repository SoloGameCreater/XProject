using Modules.FishGame;
using Modules.Maintenance;
using Modules.TripleMerge;

namespace GameplayRuntime
{
    public static class GameplayBootstrap
    {
        private const bool ENABLE_FISH_GAME = true;
        // 关闭 TripleMerge 时，将该值改为 false。
        private const bool ENABLE_TRIPLE_MERGE = true;
        private static bool s_builtInRegistered;

        public static void RegisterBuiltInModules()
        {
            if (s_builtInRegistered)
            {
                return;
            }

            if (ENABLE_FISH_GAME)
            {
                GameplayCatalog.Instance.RegisterModule(new FishGameModule());
                GameplayCatalog.Instance.SetDefaultGameplay(GameplayIds.FishGame);
            }

            if (ENABLE_TRIPLE_MERGE)
            {
                GameplayCatalog.Instance.RegisterModule(new TripleMergeModule());
            }

            if (!ENABLE_FISH_GAME && ENABLE_TRIPLE_MERGE)
            {
                GameplayCatalog.Instance.SetDefaultGameplay(GameplayIds.TripleMerge);
            }

            if (!ENABLE_FISH_GAME && !ENABLE_TRIPLE_MERGE)
            {
                GameplayCatalog.Instance.RegisterModule(new MaintenanceModule());
                GameplayCatalog.Instance.SetDefaultGameplay(GameplayIds.Maintenance);
            }

            s_builtInRegistered = true;
        }
    }
}
