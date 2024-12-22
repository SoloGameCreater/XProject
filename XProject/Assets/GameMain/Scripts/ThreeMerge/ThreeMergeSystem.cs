using Framework;

namespace ThreeMerge
{
    public class ThreeMergeSystem
    {
        private SubSystemManager _subSystemManager = new SubSystemManager();
        
        public void Enter(FsmParamTMerge fsmParamTMatch)
        {
            _subSystemManager.Init();
        }

        public void Update(float deltaTime)
        {
            _subSystemManager.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            _subSystemManager.LateUpdate(deltaTime);  
        }

        public void Exit()
        {
            _subSystemManager.Release();
        }
    }
}