using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SleepObjectDestroyEvent : IEvent
{
    public CanSleepObject obj;
    public SleepObjectDestroyEvent(CanSleepObject obj)
    {
        this.obj = obj;
    }
}
