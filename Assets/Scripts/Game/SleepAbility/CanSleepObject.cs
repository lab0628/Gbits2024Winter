
using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public interface ICanSleep
{
    void Sleep();

    void CancelSleep();
}
public abstract class CanSleepObject : MonoBehaviour,ICanSleep
{
    public bool isSleeping{get;private set;}
    private SkeletonAnimation skeletonAnimation;
    public Vector2 zzzEffectOffset;
    public UnityEvent onSleep = new UnityEvent();
    public UnityEvent onCancelSleep = new UnityEvent();
    /// <summary>
    /// 播放睡眠特效
    /// </summary>
    public void PlaySleepEffect()
    {
        if (skeletonAnimation == null)
        {
            // 添加随眠预制体
            GameObject zzzEffect = Instantiate(ResMgr.Ins.Load<GameObject>("EffectPrefabs/ZzzEffect"));
            zzzEffect.transform.parent = transform;
            skeletonAnimation = zzzEffect.GetComponent<SkeletonAnimation>();
        }

        Vector2 center = transform.position;
        skeletonAnimation.transform.eulerAngles = new Vector3(0, 0, 0);
        skeletonAnimation.transform.position = center + zzzEffectOffset;
        skeletonAnimation.gameObject.SetActive(true);
        skeletonAnimation.state.SetAnimation(0, "zzz", true);
    }

    /// <summary>
    /// 取消睡眠特效
    /// </summary>
    public void CancelSleepEffect()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.gameObject.SetActive(false);
        }
    }
    public virtual void Sleep()
    {
        // 如果在设置界面 不检测
        SetPanel pausePanel = UIMgr.Ins.GetPanel<SetPanel>("SetPanel");
        if (pausePanel && pausePanel.gameObject.activeSelf)
        {
            return;
        }
        isSleeping = true;
        PlaySleepEffect();
        onSleep.Invoke();
    }

    protected void Update()
    {
        // _isSleep = isSleeping;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,100.0f,
            1 << LayerMask.NameToLayer("Interact"));
        
        inObject = hit.collider == coll;
        if (lastInObject != inObject)
        {
            if (inObject)
            {
                EventSystem.Ins.Emit(new PlayerCursorEnterInteractEvent(this));
            }
            else
            {
                EventSystem.Ins.Emit(new PlayerCursorExitInteractEvent(this));
            }
        }
        lastInObject = inObject;
        // if(hit.collider!=null)Debug.Log(hit.collider.gameObject.name);
        if (Input.GetMouseButtonDown(0) && inObject)
        {
            OnInteract();
        }
    }
    public virtual void CancelSleep()
    {
        isSleeping = false;
        CancelSleepEffect();
        onCancelSleep.Invoke();
    }

    private void OnDestroy()
    {
        // 被控制的物体被销毁
        EventSystem.Ins.Emit(new SleepObjectDestroyEvent(this));
    }

    public void OnInteract()
    {
        if (!isSleeping)
        {
            Sleep();
            EventSystem.Ins.Emit(new SleepEvent(this));
        }
            
        else
        {

            CancelSleep();
            EventSystem.Ins.Emit(new CancelSleepEvent(this));
        }
            
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector2 center = transform.position;
        Gizmos.DrawWireSphere(center + zzzEffectOffset, 0.1f);
    }


protected Collider2D coll;
    private bool inObject = false;
    private bool lastInObject = false;
    protected void Awake()
    {
        coll = GetComponent<Collider2D>();
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }
    


    public virtual void PlayScaleAnimation()
    {
        
    }

    public virtual void ExitScaleAnimation(){

    }
}
