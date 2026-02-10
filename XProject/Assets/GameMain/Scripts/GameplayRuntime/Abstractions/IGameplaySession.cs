using System.Threading.Tasks;

namespace GameplayRuntime
{
    public interface IGameplaySession
    {
        Task<bool> EnterAsync(object param = null);
        void EnterFinish();
        void Update(float deltaTime);
        void LateUpdate(float deltaTime);
        void Exit();
    }
}
