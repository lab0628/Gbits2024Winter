
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{

    private SkeletonAnimation skeleton;
    void Start()
    {
        // RegisterFSM();
        RegisterAnimationFSM();
        RegisterCursor();
        skeleton = transform.Find("@PlayerSpine").GetComponent<SkeletonAnimation>();
        pullHand = transform.Find("@PullHand").gameObject;
        keyHint = transform.Find("@KeyHint").gameObject;

        rb = GetComponent<Rigidbody2D>();
        ListenAbilityEvent();

        pullHand.SetActive(false);
        keyHint.SetActive(false);
        
    }

    private void Update()
    {
        // 右键解除所有
        UpdateCheckCancelControlSleep();
        UpdateCheckAnimation();
        CheckBox();
        UpdateCursor();
    }
    

    /// <summary>
    /// 玩家死亡，重载当前关卡
    /// </summary>
    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");    // 防止死亡后触发多余效果
        LevelMgr.Ins.LevelReload();
    }

    /// <summary>
    /// 重载当前关卡，由输入系统调用
    /// </summary>
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>()!=1)
            return;
        gameObject.layer = LayerMask.NameToLayer("Default");    // 防止死亡后触发多余效果
        LevelMgr.Ins.LevelReload();
    }

    private void OnDrawGizmos()
    {
        DrawMovementGizmos();
        DrawCatchGizmos();
    }

    // ESC
    public void OnPause(InputAction.CallbackContext context)
    {
        float _input = context.ReadValue<float>();
        if (_input == 0) return;
        SetPanel pausePanel = UIMgr.Ins.GetPanel<SetPanel>("SetPanel");
        if (pausePanel && pausePanel.gameObject.activeSelf)
            UIMgr.Ins.HidePanel("SetPanel");
        else
            UIMgr.Ins.ShowPanel<SetPanel>("SetPanel");
    }

    
}
