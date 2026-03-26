using System.Collections.Generic;
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
        }

        public void RegisterSaveFiles(List<SaveFileBase> saveFiles)
        {
        }

        public IGameplaySession CreateSession(IGameplayContext context)
        {
            return new FishGameSession();
        }
    }
}
