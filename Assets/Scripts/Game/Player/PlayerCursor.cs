using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private CanSleepObject lastObj = null;
    private void RegisterCursor(){
        EventSystem.Ins.OnAutoOff<PlayerCursorEnterInteractEvent>(this, OnPlayerCursorEnterInteractEvent);
        EventSystem.Ins.OnAutoOff<PlayerCursorExitInteractEvent>(this, OnPlayerCursorExitInteractEvent);
    }


    private void OnPlayerCursorEnterInteractEvent(PlayerCursorEnterInteractEvent e){
        if(lastObj != e.obj && lastObj != null){
            e.obj.ExitScaleAnimation();
        }
        lastObj = e.obj;
        
        e.obj.PlayScaleAnimation();
    }

    private void OnPlayerCursorExitInteractEvent(PlayerCursorExitInteractEvent e){
        e.obj.ExitScaleAnimation();
        if(lastObj == e.obj){
            lastObj = null;
        }
    }

    private void UpdateCursor(){
        // 如果在设置界面 不检测
        SetPanel pausePanel = UIMgr.Ins.GetPanel<SetPanel>("SetPanel");
        if (pausePanel && pausePanel.gameObject.activeSelf)
        {
            return;
        }
        
        if(lastObj != null){
            // 没有睡眠
            if(!lastObj.isSleeping){
                if(IsNextControlUseSleepClock()){
                    CursorMgr.Ins.UseCursor(CursorType.SLEEP_CLOCK);
                    // UseCursor(CursorType.SLEEP_CLOCK);
                }
                else{
                    CursorMgr.Ins.UseCursor(CursorType.ZZZ);
                }
            }else{
                if(IsControlWithSleepClock(lastObj)){
                    CursorMgr.Ins.UseCursor(CursorType.CANCEL_SLEEP_CLOCK);
                }else{
                    CursorMgr.Ins.UseCursor(CursorType.CANCEL_ZZZ);
                }
            }
            

        }else{
            CursorMgr.Ins.UseCursor(CursorType.DEAFULT);
        }
    }




}
