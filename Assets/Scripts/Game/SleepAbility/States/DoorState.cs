
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class DoorState : CanSleepObject
{
    public GameObject doorSprite;

    [SerializeField, SetProperty("isOpen")]
    private bool _isOpen = false;

    private SkeletonAnimation skeleton;
    private float skeletonOriginScaleX;
    private float skeletonOriginScaleY;
    public float moveTime = 0.5f;
    public float openHeight = 0.33f;
    public bool isOpen
    {
        get { return _isOpen; }
        set
        {
            if (isSleeping)
            {
                _isOpen = !value;
                return;
            }
            _isOpen = value;
            doorFSM.ChangeState(isOpen?State.OPEN:State.CLOSE);
        }
    }
    private FSM doorFSM;
    private enum State
    {
        OPEN,
        CLOSE,
    }
    

    void Start()
    {
        skeleton = transform.Find("@DoorFrameSpine").GetComponent<SkeletonAnimation>();
        if (skeleton != null)
        {
            skeletonOriginScaleX = skeleton.transform.localScale.x;
            skeletonOriginScaleY = skeleton.transform.localScale.y;
        }
            
        // 监听接受电信号事件
        gameObject.GetComponent<EnergiedComponent>().onEnergized.AddListener(OnEnergized);
        // 监听睡眠事件
        onSleep.AddListener(OnSleep);
        onCancelSleep.AddListener(OnCancelSleep);

        doorFSM = gameObject.AddComponent<FSM>();
        FSMState openState = doorFSM.RegisterState(State.OPEN);
        FSMState closeState = doorFSM.RegisterState(State.CLOSE);
        openState.OnEnter = () =>
        {

            doorSprite.transform.DOKill();
            doorSprite.transform.DOLocalMoveY(0.5f + openHeight, GetMoveTime(true));
        };
        closeState.OnEnter = () =>
        {

            doorSprite.transform.DOKill();
            doorSprite.transform.DOLocalMoveY(0.5f, GetMoveTime(false));
        };
        doorFSM.ChangeState(isOpen?State.OPEN:State.CLOSE);
    }
    new void Update()
    {
        base.Update();
        // 让碰撞跟着贴图
        // coll.offset = doorSprite.transform.localPosition;
        
    }

    private float GetMoveTime(bool isOpen)
    {
        float distance = openHeight + 0.5f;
        float speed = distance / moveTime;
        // 获取剩下的路程
        if (isOpen)
        {
            distance -= doorSprite.transform.localPosition.y;
        }
        else
        {
            distance = doorSprite.transform.localPosition.y - 0.5f;
        }
        return distance/speed;
    }
    public void OnSleep()
    {
        if(skeleton!=null)
            skeleton.state.SetAnimation(0,"sleep",true);
        doorSprite.transform.DOPause();
    }

    public void OnCancelSleep()
    {
        if(skeleton!=null)
            skeleton.state.SetAnimation(0,"idle",true);
        doorSprite.transform.DOPlay();
    }


    public void OnEnergized(bool energy)
    {
        if (isSleeping) return;
        if (energy)
        {
            // 通电是开门
            doorFSM.ChangeState(State.OPEN);
            
        }
        else
        {
            // 关门
            doorFSM.ChangeState(State.CLOSE);
        }
    }
    
    public override void PlayScaleAnimation(){
        base.PlayScaleAnimation();
        if (skeleton != null)
        {
            skeleton.transform.DOScaleX(skeletonOriginScaleX + 0.1f, 0.2f);
            skeleton.transform.DOScaleY(skeletonOriginScaleY + 0.4f, 0.2f);
        }
            
    }

    public override void ExitScaleAnimation(){
        if (skeleton != null)
        {
            skeleton.transform.DOScaleX(skeletonOriginScaleX, 0.2f);
            skeleton.transform.DOScaleY(skeletonOriginScaleY, 0.2f);
        }
            
    }
}
