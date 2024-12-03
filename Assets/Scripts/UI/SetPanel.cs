using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetPanel : BasePanel
{
    public override void OnHide()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnShow()
    {
        // 音量
        Toggle[] toggles = this.GetComponentsInChildren<Toggle>();
        foreach(Toggle t in toggles)
        {
            if(t.gameObject.name == "Music")
                t.isOn = MusicMgr.Ins.GetBGMValue() > 0;
            if(t.gameObject.name == "Sound")
                t.isOn = MusicMgr.Ins.GetSoundValue() > 0;
        }
        //// 按钮：返回主页、重载本关
        //bool isHome = SceneManager.GetActiveScene().name == "Home";
        //if(isHome)
        //    hideMode = HideMode.Destory;
        //Button[] buttons = this.GetComponentsInChildren<Button>();
        //foreach (Button b in buttons)
        //{
        //    if (b.gameObject.name == "ButtonClose") // 仅Home页显示
        //        b.gameObject.SetActive(isHome);
        //    if (b.gameObject.name == "ButtonHome" || b.gameObject.name == "ButtonReplay")   // 仅Level页显示
        //        b.gameObject.SetActive(!isHome);
        //}
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ButtonHome":
                LevelMgr.Ins.Home();
                break;
            case "ButtonReplay":
                LevelMgr.Ins.LevelReload();
                break;
        }
        UIMgr.Ins.HidePanel("SetPanel");
    }
    protected override void OnValueChanged(string toggleName, bool value)
    {
        //Debug.Log(value);
        switch (toggleName)
        {
            case "Music":
                MusicMgr.Ins.ChangeBGMValue(value ? 1 : 0);
                //Debug.Log(MusicMgr.Ins.GetBGMValue());
                break;
            case "Sound":
                MusicMgr.Ins.ChangeAllSoundValue(value ? 1 : 0);
                //Debug.Log(MusicMgr.Ins.GetSoundValue());
                break;
            default:
                break;
        }
    }
}
