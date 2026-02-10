using System.Threading.Tasks;
using TripleMerge;

namespace Modules.TripleMerge
{
    public class TripleMergeSession : GameplayRuntime.IGameplaySession
    {
        public Task<bool> EnterAsync(object param = null)
        {
            TripleMergeSystem.Instance.OnEnterTripleMerge();
            return Task.FromResult(true);
        }

        public void EnterFinish()
        {
            UILoadingController.Hide(false);
            if (UIViewSystem.Instance.Get<LobbyMainUI>() != null)
            {
                UIViewSystem.Instance.Get<LobbyMainUI>().Show();
            }
            else
            {
                UIViewSystem.Instance.Open<LobbyMainUI>();
            }
        }

        public void Update(float deltaTime)
        {
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void Exit()
        {
            TripleMergeSystem.Instance.Exit();
        }
    }
}
