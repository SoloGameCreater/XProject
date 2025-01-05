using Framework;

namespace TripleMerge
{
    public class TripleMergeSystem : GlobalSystem<TripleMergeSystem>, IUpdatable, ILateUpdatable
    {
        public TripleMergeGameplay Gameplay { get; private set; }
        //public TripleMergeModel Model { get; } = new();
        
        public void OnEnterTripleMerge()
        {
            Gameplay = new TripleMergeGameplay();
            Gameplay.Init();
        }

        public void Update(float deltaTime)
        {
            Gameplay?.OnUpdate(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            Gameplay?.OnLateUpdate(deltaTime);  
        }

        public void Exit()
        {
            Gameplay?.Release();
            Gameplay = null;
        }
    }
}