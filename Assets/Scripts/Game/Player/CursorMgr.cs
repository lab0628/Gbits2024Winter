using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CursorType{
    DEAFULT,
    ZZZ,
    SLEEP_CLOCK,
    CANCEL_ZZZ,
    CANCEL_SLEEP_CLOCK
}
public class CursorMgr : SingletonAutoMono<CursorMgr>
{
    private CursorType curType = CursorType.DEAFULT;

    public void UseCursor(CursorType cursorType){
        if(curType == cursorType)return;
        switch (cursorType)
        {
            case CursorType.ZZZ:
                Texture2D cursorTex = ResMgr.Ins.Load<Texture2D>("Cursors/zzzCursor");
                Cursor.SetCursor(cursorTex,new Vector2(100,100),CursorMode.Auto);
                break;
            case CursorType.DEAFULT:
                Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);break;

            case CursorType.SLEEP_CLOCK:
                cursorTex = ResMgr.Ins.Load<Texture2D>("Cursors/sleepClockCursor");
                Cursor.SetCursor(cursorTex,new Vector2(100,100),CursorMode.Auto);
                break;
            
            case CursorType.CANCEL_SLEEP_CLOCK:
                cursorTex = ResMgr.Ins.Load<Texture2D>("Cursors/cancelSleepClockCursor");
                Cursor.SetCursor(cursorTex,new Vector2(100,100),CursorMode.Auto);
                break;
            case CursorType.CANCEL_ZZZ:
                cursorTex = ResMgr.Ins.Load<Texture2D>("Cursors/cancelCursor");
                Cursor.SetCursor(cursorTex,new Vector2(100,100),CursorMode.Auto);
                break;
            default:Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);break;
        }
        curType = cursorType;
        
    }
}
