
public struct PlayerCursorEnterInteractEvent : IEvent
{
    public CanSleepObject obj;

    public PlayerCursorEnterInteractEvent(CanSleepObject obj)
    {
        this.obj = obj;
    }
}


public struct PlayerCursorExitInteractEvent : IEvent
{
    public CanSleepObject obj;

    public PlayerCursorExitInteractEvent(CanSleepObject obj)
    {
        this.obj = obj;
    }
}
