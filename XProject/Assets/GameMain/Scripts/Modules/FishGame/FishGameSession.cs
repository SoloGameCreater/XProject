using System.Threading.Tasks;
using Framework;
using TripleMerge;

namespace Modules.FishGame
{
    public class FishGameSession : GameplayRuntime.IGameplaySession
    {
        public Task<bool> EnterAsync(object param = null)
        {
            return Task.FromResult(true);
        }

        public void EnterFinish()
        {
            UILoadingController.Hide(false);

            if (UIViewSystem.Instance.Get<LobbyMainUI>() != null)
            {
                UIViewSystem.Instance.Close<LobbyMainUI>();
            }

            if (UIViewSystem.Instance.Get<FishGameRuntime.FishGameMainUI>() == null)
            {
                UIViewSystem.Instance.Open<FishGameRuntime.FishGameMainUI>();
            }

            DebugUtil.Log("FishGameSession: 已进入钓鱼玩法。");
        }

        public void Update(float deltaTime)
        {
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void Exit()
        {
            if (UIViewSystem.Instance.Get<FishGameRuntime.FishGameMainUI>() != null)
            {
                UIViewSystem.Instance.Close<FishGameRuntime.FishGameMainUI>();
            }
        }
    }
}
