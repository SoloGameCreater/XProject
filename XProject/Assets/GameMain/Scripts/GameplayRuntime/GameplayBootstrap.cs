using Modules.Maintenance;
using Modules.TripleMerge;

namespace GameplayRuntime
{
    public static class GameplayBootstrap
    {
        // 关闭 TripleMerge 时，将该值改为 false。
        private const bool ENABLE_TRIPLE_MERGE = false;
        private static bool s_builtInRegistered;

        public static void RegisterBuiltInModules()
        {
            if (s_builtInRegistered)
            {
                return;
            }

            if (ENABLE_TRIPLE_MERGE)
            {
                GameplayCatalog.Instance.RegisterModule(new TripleMergeModule());
                GameplayCatalog.Instance.SetDefaultGameplay(GameplayIds.TripleMerge);
            }
            else
            {
                GameplayCatalog.Instance.RegisterModule(new MaintenanceModule());
                GameplayCatalog.Instance.SetDefaultGameplay(GameplayIds.Maintenance);
            }

            s_builtInRegistered = true;
        }
    }
}
