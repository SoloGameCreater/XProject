using GameFramework;
using GameFramework.Event;

namespace StarForce
{
    public sealed class BuffOnTriggerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BuffOnTriggerEventArgs).GetHashCode();
        
        public override int Id => EventId;
        public BuffData UserData { get;private set; }
        
        public static BuffOnTriggerEventArgs Create(BuffData buffData)
        {
            BuffOnTriggerEventArgs showEntityFailureEventArgs = ReferencePool.Acquire<BuffOnTriggerEventArgs>();
            showEntityFailureEventArgs.UserData = buffData;
            return showEntityFailureEventArgs;
        }
        
        public override void Clear()
        {
            UserData = default;
        }
    }
}
