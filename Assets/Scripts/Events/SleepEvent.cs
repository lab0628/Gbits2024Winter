using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SleepEvent : IEvent
{
    public CanSleepObject canSleepObject;

    public SleepEvent(CanSleepObject canSleepObject)
    {
        this.canSleepObject = canSleepObject;
    }
}
public struct CancelSleepEvent : IEvent
{
    public CanSleepObject canSleepObject;

    public CancelSleepEvent(CanSleepObject canSleepObject)
    {
        this.canSleepObject = canSleepObject;
    }
}