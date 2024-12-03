using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源管理器
/// </summary>
public class ResMgr : Singleton<ResMgr>
{
    /// <summary>
    /// 同步加载资源，如果是预制体类型会自动实例化并返回
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Load<T>(string path) where T:Object
    {
        T res = Resources.Load<T>(path);
        return res;
    }


    /// <summary>
    /// 异步加载资源，如果是预制体类型会自动实例化并返回
    /// </summary>
    /// <param name="path"></param>
    /// <param name="onComplete"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string path, UnityAction<T> onComplete) where T:Object
    {
        //开启异步加载的协程
        MonoMgr.Ins.StartCoroutine(ReallyLoadAsync(path, onComplete));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> onComplete) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        onComplete(r.asset as T);
    }


}
