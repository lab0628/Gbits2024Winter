using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonObject : CanSleepObject
{
    public SkeletonAnimation skeleton;
    private float skeletonOriginScale;
    public EnergyClass[] energys;
    private bool lastIsDown = false;
    private bool _isDown;
    private FSM aniFSM;
    private bool hasBody;// 判断是否有物体压着
    private HashSet<GameObject> bodies = new HashSet<GameObject>() ;
    public bool isDown
    {
        get { return _isDown; }

    }
    private enum AnimationState{
        IDLE,
        IDLE_SLEEPING,
        DOWN_IDLE,
        UP,
        DOWN
    }

    void Start(){
        skeleton = transform.Find("@Spine").GetComponent<SkeletonAnimation>();
        skeletonOriginScale = skeleton.transform.localScale.x;
        RegisterAnimationFSM();
        
        onSleep.AddListener(OnSleep);
        onCancelSleep.AddListener(OnCancelSleep);



    }

    private void RegisterAnimationFSM(){
        aniFSM = gameObject.AddComponent<FSM>();
        FSMState idle = aniFSM.RegisterState(AnimationState.IDLE);
        idle.OnEnter += ()=>{
            skeleton.state.SetAnimation(0, "idle", true);
        };
        idle.OnUpdate += ()=>{
            if(_isDown){
                aniFSM.ChangeState(AnimationState.DOWN);
            }
            else
            {
                if(isSleeping)aniFSM.ChangeState(AnimationState.IDLE_SLEEPING);
            }
        };
        aniFSM.RegisterState(AnimationState.DOWN).OnEnter += ()=>{
            
            var te = skeleton.state.SetAnimation(0, "down", false);
            te.TimeScale = 2.0f;
            te.Complete += track =>{
                aniFSM.ChangeState(AnimationState.DOWN_IDLE);
            };
        };
        FSMState downIdle = aniFSM.RegisterState(AnimationState.DOWN_IDLE);
        downIdle.OnEnter += ()=>{
            // skeleton.state.SetAnimation(0, "idle2", true);
            skeleton.state.TimeScale = 0f;
        };
        downIdle.OnExit += () =>
        {
            skeleton.state.TimeScale = 1f;
        };
        
        downIdle.OnUpdate += ()=>{
            if(!_isDown){
                aniFSM.ChangeState(AnimationState.UP);
            }
        };
        
        aniFSM.RegisterState(AnimationState.UP).OnEnter += ()=>{
            var te = skeleton.state.SetAnimation(0, "up", false);
            te.Complete += track =>{
                aniFSM.ChangeState(AnimationState.IDLE);
            };
        };
        
        aniFSM.RegisterState(AnimationState.IDLE_SLEEPING).OnEnter += ()=> {
            skeleton.state.SetAnimation(0, "idle3", true); 
        };
          
        aniFSM.RegisterState(AnimationState.IDLE_SLEEPING).OnUpdate += ()=> {
            if(!isSleeping)aniFSM.ChangeState(AnimationState.IDLE);
        };
        
        aniFSM.ChangeState(AnimationState.IDLE);
        
    }
    
    
    protected new void Update()
    {
        base.Update();
        OnEnergy();
        if(isSleeping)return;
        _isDown = bodies.Count > 0;
    }

    void OnTriggerStay2D(Collider2D collider)
    {

        if(isSleeping)return;
        if(collider.gameObject.layer == LayerMask.NameToLayer("Player") || collider.gameObject.layer == LayerMask.NameToLayer("Interact")){
            bodies.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        bodies.Remove(collider.gameObject);
    }



    private void OnEnergy()
    {
        if(energys != null)
            foreach (EnergyClass energy in energys)
            {
                if (energy != null)
                {
                    if(energy.energy)energy.obj.Energized(isDown);
                    else
                        energy.obj.Energized(!isDown);
                }
                
            }
    }

    public void OnSleep()
    {
        // buttonJoint.rb.bodyType = RigidbodyType2D.Static;
        skeleton.timeScale = 0f;
    }

    public void OnCancelSleep()
    {
        // buttonJoint.rb.bodyType = RigidbodyType2D.Dynamic;
        skeleton.timeScale = 1f;

    }

    public override void PlayScaleAnimation(){
        base.PlayScaleAnimation();
        skeleton.transform.DOScale(skeletonOriginScale + 0.2f, 0.2f);
    }

    public override void ExitScaleAnimation(){
        skeleton.transform.DOScale(skeletonOriginScale, 0.2f);
    }
}
