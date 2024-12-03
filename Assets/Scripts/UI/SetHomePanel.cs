using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetHomePanel : SetPanel
{
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ButtonClose":
                UIMgr.Ins.HidePanel("SetHomePanel");
                break;
        }
    }
}
