using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using EventHashMap = System.Collections.Generic.Dictionary<int, System.Action<IEvent>>;
/// <summary>
/// 事件数据
/// </summary>
public interface IEvent
{
    
}

public class EventSystem : Singleton<EventSystem>
{
    
    // 存储事件和对应的监听器列表
    private Dictionary<Type, EventHashMap> eventDict = new Dictionary<Type, EventHashMap>();

    /// <summary>
    /// 监听事件 需要手动取消监听
    /// </summary>
    /// <param name="callback">监听的回调函数</param>
    /// <typeparam name="T">继承IEvent的结构体或者类</typeparam>
    public void On<T>(Action<T> callback) where T : IEvent, new()
    {
        Type t = typeof(T);
        EventHashMap map;
        if (!eventDict.TryGetValue(t, out map))
        {
            map = new EventHashMap();
        }

        Action<IEvent> newCallback = (IEvent e) => { callback((T)e); };
        map.Add(callback.GetHashCode(), newCallback);
        eventDict[t] = map;
    }

    /// <summary>
    /// 监听事件
    /// 当物体销毁时会自动取消监听事件
    /// </summary>
    /// <param name="obj">继承mono的物体</param>
    /// <param name="callback">监听的回调函数</param>
    /// <typeparam name="T">继承IEvent的结构体或者类</typeparam>
    public void OnAutoOff<T>(MonoBehaviour obj, Action<T> callback) where T : IEvent, new()
    {
        On(callback);
        SmartOffComponent comp = obj.GetOrAddComponent<SmartOffComponent>();
        comp.AddOffEvent(()=>{Off(callback);});
    }
    
    /// <summary>
    /// 取消监听事件
    /// </summary>
    /// <param name="callback">监听的回调函数</param>
    /// <typeparam name="T">继承IEvent的结构体或者类</typeparam>
    public void Off<T>(Action<T> callback) where T : IEvent, new()
    {
        Type t = typeof(T);
        EventHashMap map;
        if (!eventDict.TryGetValue(t, out map))
        {
            map = new EventHashMap();
        }

        map.Remove(callback.GetHashCode());
        if (map.Count == 0)
        {
            eventDict.Remove(t);
        }
        
    }
    

    /// <summary>
    /// 派发事件，使所以监听到该类型的事件触发
    /// </summary>
    /// <param name="t">事件实例</param>
    /// <typeparam name="T">继承IEvent的结构体或者类</typeparam>
    public void Emit<T>(T t) where T : IEvent,new()
    {
        Type type = typeof(T);
        EventHashMap map;
        if (!eventDict.TryGetValue(type, out map))
        {
            map = new EventHashMap();
        }
        foreach (var kv in map)
        {
            kv.Value?.Invoke(t);
        }
        
    }

    /// <summary>
    /// 清除所有事件和监听器
    /// </summary>
    public void Clear()
    {
        eventDict.Clear();
    }
    public class SmartOffComponent : MonoBehaviour
    {
        private Action offEvents;

        public void AddOffEvent(Action callback)
        {
            offEvents += callback;
        }
        private void OnDestroy()
        {
            offEvents.Invoke();
        }
    }
}


