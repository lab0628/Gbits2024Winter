using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SleepClockTirgger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider2D){
        MusicMgr.Ins.PlaySound("获得道具/Inventory",false);
        EventSystem.Ins.Emit(new PlayerTakePropEvent(PorpType.SleepClock));
        Destroy(this.transform.parent.gameObject);
    }
    
}
