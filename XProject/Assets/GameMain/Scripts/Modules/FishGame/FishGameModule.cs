using System.Collections.Generic;
using Config.FishGame;
using GameplayRuntime;
using SaveFile;

namespace Modules.FishGame
{
    public class FishGameModule : IGameplayModule
    {
        public string Id => GameplayIds.FishGame;
        public int Priority => 120;

        public void Install(IGameplayInstaller installer)
        {
            installer.AddSubSystem<FishGameConfigManager>();
        }

        public void RegisterSaveFiles(List<SaveFileBase> saveFiles)
        {
            saveFiles.Add(new SaveFileFishGame());
        }

        public IGameplaySession CreateSession(IGameplayContext context)
        {
            return new FishGameSession();
        }
    }
}
