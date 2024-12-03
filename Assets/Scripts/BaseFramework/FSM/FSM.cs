using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FSMState{
    public int StateID;
    public UnityAction OnEnter = null;
    public UnityAction OnExit = null;
    public UnityAction OnUpdate = null;
    public UnityAction OnFixedUpdate = null;
    public FSMState(int StateID){
        this.StateID = StateID;
    }
}
public class FSM : MonoBehaviour
{
    private FSMState _curState;
    private Dictionary<int, FSMState> _states = new Dictionary<int, FSMState>();
    
    public FSMState RegisterState<T>(T stateEnum)where T : Enum {
        int stateID = System.Convert.ToInt32(stateEnum);
        FSMState state;
        if(!_states.TryGetValue(stateID, out state)){
            state = new FSMState(stateID);
            _states.Add(stateID, state);
        }else{
            Debug.LogWarning($"[warning] Register State Repeat: {stateID}");
        }
        
        return state;
    }
    public void ChangeState<T>(T stateEnum)where T : Enum {
        int stateID = System.Convert.ToInt32(stateEnum);
        _curState?.OnExit?.Invoke();
        FSMState nextState;
        if(_states.TryGetValue(stateID, out nextState))
        {
            if (nextState == _curState) return;
            nextState.OnEnter?.Invoke();
        }else{
            Debug.LogWarning($"[warning] Change State Error, Can't Get {stateID} State");
        }
        _curState = nextState;

    }

    public T GetCurrentState<T>() where T : Enum
    {
        if (_curState != null)
        {
            return (T)Enum.ToObject(typeof(T), _curState.StateID);
        }
        return default(T);
    }
    
    void Update()
    {
        _curState?.OnUpdate?.Invoke();
    }
    
    void FixedUpdate(){
        _curState?.OnFixedUpdate?.Invoke();
    }
}
