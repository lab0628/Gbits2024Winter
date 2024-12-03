
/// <summary>
/// 道具类型
/// </summary>
public enum PorpType{
    SleepClock
}
public struct PlayerTakePropEvent : IEvent
{
    public PorpType porpType;
    public PlayerTakePropEvent(PorpType porpType){
        this.porpType = porpType;
    }
}
