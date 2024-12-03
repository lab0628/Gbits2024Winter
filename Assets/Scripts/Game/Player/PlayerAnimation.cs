using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine;
using Timers;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private enum PlayerAnimationState{
        WALK,
        IDLE,
        JUMP_UP,
        JUMP_FLOAT,
        JUMP_FALL,
        JUMP_DOWN,
        PUSH,
        PULL
    }
    private FSM aniFSM;
    private bool wantPlayJumpDown = false;
    private void RegisterAnimationFSM(){
        aniFSM = gameObject.AddComponent<FSM>();
        aniFSM.RegisterState(PlayerAnimationState.WALK).OnEnter += ()=>{
            skeleton.state.SetAnimation(0, "walk2", true);
        };
        aniFSM.RegisterState(PlayerAnimationState.IDLE).OnEnter += ()=>{
            skeleton.state.SetAnimation(0, "idle2", true);
        };
        aniFSM.RegisterState(PlayerAnimationState.JUMP_UP).OnEnter += ()=>{
            skeleton.state.SetAnimation(0, "jump_up", true);
        };
        aniFSM.RegisterState(PlayerAnimationState.JUMP_FLOAT).OnEnter += ()=>{
            skeleton.state.SetAnimation(0, "jump_float", true);
        };
        aniFSM.RegisterState(PlayerAnimationState.JUMP_FALL).OnEnter += ()=>{
            skeleton.state.SetAnimation(0, "jump_fall", true);
            wantPlayJumpDown = true;
        };
        aniFSM.RegisterState(PlayerAnimationState.JUMP_DOWN).OnEnter += ()=>{
            // 播放音效
            // if (GetPlayerGroundType() == GroundType.Ground)
            // {
            //     
            //     MusicMgr.Ins.PlaySound("落地/在石砖上/Land Step Stone A", false, (audioSource =>
            //     {
            //         audioSource.pitch = 2f;
            //     }));
            // }
            //
            // if (GetPlayerGroundType() == GroundType.Cloud)
            // {
            //     MusicMgr.Ins.PlaySound("落地/在云上/Land Step Snow A", false);
            // }
            var state = skeleton.state.SetAnimation(0, "jump_down", false);
            state.Complete += (TrackEntry te)=>{
                wantPlayJumpDown = false;
                state.TimeScale = 1.0f;
            };
            // 如果落地就走就加快一下播放
            if(_inputDirection != 0){
                state.TimeScale = 3.5f;
            }else{
                state.TimeScale = 3.0f;
            }
        };
        var pushState = aniFSM.RegisterState(PlayerAnimationState.PUSH);
        pushState.OnEnter += ()=>{
            onceCheckInputStop = false;
            
            var te = skeleton.state.SetAnimation(0, "push", true);
            TimersManager.SetTimer(this, 0.1f, ()=>{onceCheckInputStop = true;});
        };
        pushState.OnUpdate += ()=>{
            if(!onceCheckInputStop)return;
            if(_inputDirection==0)skeleton.state.TimeScale = 0f;
            else skeleton.state.TimeScale = 1f;
        };
        pushState.OnExit += ()=>{skeleton.state.TimeScale = 1f;};
        var pullState = aniFSM.RegisterState(PlayerAnimationState.PULL);
        pullState.OnEnter += ()=>{
            onceCheckInputStop = false;
            var te = skeleton.state.SetAnimation(0, "pull", true);
            TimersManager.SetTimer(this, 0.1f, ()=>{onceCheckInputStop = true;});
        };
        pullState.OnUpdate += ()=>{
            if(!onceCheckInputStop)return;
            if(_inputDirection==0)skeleton.state.TimeScale = 0f;
            else skeleton.state.TimeScale = 1f;
        };
        pullState.OnExit += ()=>{skeleton.state.TimeScale = 1f;};
    }
    private bool onceCheckInputStop = false;

    private void UpdateCheckAnimation(){

        

        if(_movingBox && _targetBox!=null){
            // 进入状态
            if(aniFSM.GetCurrentState<PlayerAnimationState>() != PlayerAnimationState.PUSH && aniFSM.GetCurrentState<PlayerAnimationState>() != PlayerAnimationState.PULL){
                // 改变为正确的方向
                Vector3 origin = skeleton.transform.localScale;
                int mut = BoxIsRight()?1:-1;
                skeleton.transform.localScale = new Vector3(mut * Math.Abs(origin.x), origin.y, origin.z);
                aniFSM.ChangeState(PlayerAnimationState.PULL);
            }
            if(_inputDirection > 0 && BoxIsRight()){
                aniFSM.ChangeState(PlayerAnimationState.PUSH);

            }
            else if(_inputDirection < 0 && BoxIsRight()){
                aniFSM.ChangeState(PlayerAnimationState.PULL);
            }

            else if(_inputDirection > 0 && !BoxIsRight()){
                aniFSM.ChangeState(PlayerAnimationState.PULL);

            }
            else if(_inputDirection < 0 && !BoxIsRight()){
                aniFSM.ChangeState(PlayerAnimationState.PUSH);
            }
            return;
        }
        ChangeFace();
        if(_onGround){
            // 截胡 如果是空中落下来的话要接落地动画
            if(wantPlayJumpDown){
                aniFSM.ChangeState(PlayerAnimationState.JUMP_DOWN);
                return;
            }
            if(_inputDirection != 0)aniFSM.ChangeState(PlayerAnimationState.WALK);
            else aniFSM.ChangeState(PlayerAnimationState.IDLE);
        }else{
            // 根据在空中的速率来更换跳跃动画
            if(rb.velocity.y > 2f){
                aniFSM.ChangeState(PlayerAnimationState.JUMP_UP);
            }
            
            else if(rb.velocity.y < -2f){
                aniFSM.ChangeState(PlayerAnimationState.JUMP_FALL);
            }
            
            else{
                aniFSM.ChangeState(PlayerAnimationState.JUMP_FLOAT);

            }
        }
        
    }
    /// <summary>
    /// 根据输入改变精灵朝向
    /// </summary>
    private void ChangeFace(){
        if(_inputDirection != 0){
            Vector3 origin = skeleton.transform.localScale;
            if(_inputDirection > 0){
                skeleton.transform.localScale = new Vector3(Math.Abs(origin.x), origin.y, origin.z);
            }
            else if(_inputDirection < 0){
                skeleton.transform.localScale = new Vector3(-Math.Abs(origin.x), origin.y, origin.z);
            }
            
        }
    }




}
