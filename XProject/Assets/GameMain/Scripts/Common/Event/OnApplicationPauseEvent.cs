
public class OnApplicationPauseEvent : BaseEvent
{
    public bool pause;

    public OnApplicationPauseEvent(bool pause) : base(EventEnum.OnApplicationPause)
    {
        this.pause = pause;
    }
}