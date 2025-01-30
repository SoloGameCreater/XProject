
namespace TripleMerge
{
    public interface ITripleMergeComponent
    {
        protected internal void OnInitialize();

        protected internal void OnDispose();

        protected internal void OnUpdate();

        protected internal void OnFixedUpdate();

        protected internal void OnLateUpdate();
    }

    public abstract class TripleMergeComponent : ITripleMergeComponent
    {
        protected abstract void OnInitialize();
        protected abstract void OnDispose();
        protected virtual void OnUpdate()
        {
        }
        protected virtual void OnFixedUpdate()
        {
        }
        protected virtual void OnLateUpdate()
        {
        }

        void ITripleMergeComponent.OnInitialize()
        {
            OnInitialize();
        }

        void ITripleMergeComponent.OnDispose()
        {
            OnDispose();
        }

        void ITripleMergeComponent.OnUpdate()
        {
            OnUpdate();
        }

        void ITripleMergeComponent.OnFixedUpdate()
        {
            OnFixedUpdate();
        }

        void ITripleMergeComponent.OnLateUpdate()
        {
            OnLateUpdate();
        }
    }
}