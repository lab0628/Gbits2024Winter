using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 通电组件
/// 可以让电信号传送到改组件上，触发onEnergized事件
/// </summary>
public class EnergiedComponent : MonoBehaviour
{
    public UnityEvent<bool> onEnergized = new UnityEvent<bool>();
    public void Energized(bool energy){
        onEnergized.Invoke(energy);
    }
}
