using System.Collections.Generic;
using Config.TripleMerge;
using GameplayRuntime;
using SaveFile;
using SaveFile.TripleMerge;
using TripleMerge;

namespace Modules.TripleMerge
{
    public class TripleMergeModule : IGameplayModule
    {
        public string Id => GameplayIds.TripleMerge;
        public int Priority => 100;

        public void Install(IGameplayInstaller installer)
        {
            installer.AddSubSystem<TripleMergeSystem>();
            installer.AddSubSystem<TripleMergeConfigManager>();
            installer.AddSubSystem<MapDataLoader>();
        }

        public void RegisterSaveFiles(List<SaveFileBase> saveFiles)
        {
            saveFiles.Add(new SaveFileTripleMerge());
        }

        public IGameplaySession CreateSession(IGameplayContext context)
        {
            return new TripleMergeSession();
        }
    }
}
