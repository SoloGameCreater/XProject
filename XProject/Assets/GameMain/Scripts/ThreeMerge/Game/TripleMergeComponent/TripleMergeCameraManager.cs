using Extension;

namespace TripleMerge
{
    public class TripleMergeCameraManager : TripleMergeComponent
    {
        internal TripleMergeCameraComponent _cameraController = null;
        
        protected override void OnInitialize()
        {
            _cameraController = TripleMergeSystem.Instance.Gameplay.MapManager.MapRoot.GetOrCreateComponent<TripleMergeCameraComponent>();
        }

        protected override void OnDispose()
        {
            _cameraController.Reset();
            _cameraController = null;
        }
    }
}