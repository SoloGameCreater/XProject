using System.Collections.Generic;
using GameplayRuntime;
using SaveFile;

namespace Modules.Maintenance
{
    /// <summary>
    /// 维护模式模块：用于玩法关闭场景的兜底模块。
    /// </summary>
    public class MaintenanceModule : IGameplayModule
    {
        public string Id => GameplayIds.Maintenance;
        public int Priority => -100;

        public void Install(IGameplayInstaller installer)
        {
            // 维护模式不需要额外子系统
        }

        public void RegisterSaveFiles(List<SaveFileBase> saveFiles)
        {
            // 维护模式不注册存档
        }

        public IGameplaySession CreateSession(IGameplayContext context)
        {
            return new MaintenanceSession();
        }
    }
}
