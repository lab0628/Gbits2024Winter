using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private void Start()
    {
        var ins = CursorMgr.Ins;
    }

    private void OnDestroy()
    {
        CursorMgr.Ins.UseCursor(CursorType.DEAFULT);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        CursorMgr.Ins.UseCursor(CursorType.ZZZ);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorMgr.Ins.UseCursor(CursorType.DEAFULT);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LevelMgr.Ins.LevelNext();
    }
}
