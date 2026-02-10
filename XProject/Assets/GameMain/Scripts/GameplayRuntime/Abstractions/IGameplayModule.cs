using System.Collections.Generic;
using SaveFile;

namespace GameplayRuntime
{
    public interface IGameplayModule
    {
        string Id { get; }
        int Priority { get; }
        void Install(IGameplayInstaller installer);
        void RegisterSaveFiles(List<SaveFileBase> saveFiles);
        IGameplaySession CreateSession(IGameplayContext context);
    }
}
