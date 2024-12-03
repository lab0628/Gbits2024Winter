
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public int canControllerCount = 1;
    private int sleepClockCount = 0;
    private List<CanSleepObject> controlSleepList = new List<CanSleepObject>();
    private HashSet<CanSleepObject> clockControlSleepSet = new HashSet<CanSleepObject>();

    private void ListenAbilityEvent(){
        EventSystem.Ins.OnAutoOff<SleepEvent>(this,OnSleep);
        EventSystem.Ins.OnAutoOff<CancelSleepEvent>(this,OnCancelSleep);
        EventSystem.Ins.OnAutoOff<PlayerTakePropEvent>(this,OnPlayerTakePropEvent);
    }
    private void UpdateCheckCancelControlSleep(){
        //// 右键解除所有
        // 右键仅解除技能
        if(Input.GetMouseButtonUp(1))
        {
            // CanSleepObject[] arr = canSleepList.ToArray();
            for (int i = controlSleepList.Count - 1; i >= 0; i--)
            {
                controlSleepList[i].OnInteract();

            }

            //// 接触秒表控制的
            //var arr = clockControlSleepSet.ToArray();
            //foreach (var obj in arr)
            //{
            //    obj.OnInteract();
            //}
            //clockControlSleepSet.Clear();
        }

    }

    private void OnSleep(SleepEvent e)
    {
        //FreeBox();
        // MusicMgr.Ins.PlaySound("sleep", false);
        CanSleepObject obj = e.canSleepObject;
        // 如果是睡眠钟表，则优先使用
        if(sleepClockCount > 0){
            clockControlSleepSet.Add(obj);
            sleepClockCount--;
            return;
        }
        // 超出数量让前面一部分取消
        while (controlSleepList.Count >= canControllerCount)
        {
            controlSleepList[0].OnInteract();
        }
        controlSleepList.Add(obj);
    }

    private void OnCancelSleep(CancelSleepEvent e)
    {
        CanSleepObject obj = e.canSleepObject;
        // 判断是不是钟表控制的
        if(IsControlWithSleepClock(obj)){
            clockControlSleepSet.Remove(obj);
            // 掉落钟表
            WorldMgr.Ins.GnerateProp(PorpType.SleepClock, obj.transform.position);
            return;
        }
        
        controlSleepList.Remove(obj);
    }

    private void OnPlayerTakePropEvent(PlayerTakePropEvent e){
        switch (e.porpType)
        {
            case PorpType.SleepClock:
                sleepClockCount++;
                Debug.Log("捡到了秒表");
                break;
            default:break;
        }
    }


    /// <summary>
    /// 下一次催眠是否是使用时钟
    /// </summary>
    /// <returns></returns>
    public bool IsNextControlUseSleepClock(){
        return sleepClockCount > 0;
    }

    /// <summary>
    /// 判断该物体是否由钟表控制的睡眠
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool IsControlWithSleepClock(CanSleepObject obj){
        return clockControlSleepSet.Contains(obj);
    }
}
