using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "TestBtn":
                Debug.Log("click test btn");
                break;
        }
    }
    public override void OnShow()
    {
        Debug.Log("OnShow");
    }

    public override void OnHide()
    {
        Debug.Log("OnHide");
    }
    

    
}
