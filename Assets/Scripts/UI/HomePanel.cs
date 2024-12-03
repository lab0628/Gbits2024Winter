using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 首页主界面
/// 统一管理按钮逻辑
/// </summary>
public class HomePanel : BasePanel
{
    //public Button btnStart;
    //public Button btnSet;
    //public Button btnExit;
    //private void Start()
    //{
    //    //btnStart.onClick.AddListener(()=> { OnClick("Start"); });
    //    //btnSet.onClick.AddListener(()=> { OnClick("Set"); });
    //    //btnExit.onClick.AddListener(()=> { OnClick("Exit"); });
    //}

    private void Start()
    {
        // 首页音乐
        MusicMgr.Ins.PlayBGM("Home", true);
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "ButtonStart":
                GameStart();
                break;            
            case "ButtonSet":
                Debug.Log("Set");
                Set();
                break;
            case "ButtonQuit":
                Debug.Log("Quit");
                Application.Quit();
                break;
        }
    }

    private void GameStart()
    {
        StartCoroutine(Mask(
            () => {
                //Debug.LogWarning("Scene Only For Test");
                //LevelMgr.Ins.LevelLoad("1");
                //MusicMgr.Ins.PlayBGM("Level", true);
                LevelMgr.Ins.Continue();
            }
        ));
    }

    private IEnumerator Mask(UnityAction onComplete = null, float duration = 0.3f)
    {
        Image mask = GetControl<Image>("Mask");
        float deltaTime = 0.04f;
        float deltaAlpha = 1 / (duration / deltaTime);
        float currentAlpha = 0;
        while (mask.color.a < 1)
        {
            currentAlpha = (currentAlpha + deltaAlpha) > 1 ? 1 : (currentAlpha + deltaAlpha);
            mask.color = new Color(0, 0, 0, currentAlpha);
            yield return new WaitForSecondsRealtime(deltaTime);
        }
        onComplete?.Invoke();
    }

    /// <summary>
    /// 打开设置窗口，用于输入系统
    /// </summary>
    public void Set(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1f)
            Set();
    }

    /// <summary>
    /// 打开设置窗口
    /// </summary>
    public void Set()
    {
        SetPanel pausePanel = UIMgr.Ins.GetPanel<SetPanel>("SetHomePanel");
        if (pausePanel && pausePanel.gameObject.activeSelf)
            UIMgr.Ins.HidePanel("SetHomePanel");
        else
            UIMgr.Ins.ShowPanel<SetPanel>("SetHomePanel");
    }

    //private void Mask()
    //{
    //    Image mask = GetControl<Image>("Mask");
    //    mask.DOColor(Color.black, 0.3f);
    //}

    public override void OnHide()
    {
        
    }

    public override void OnShow()
    {
        
    }
}
