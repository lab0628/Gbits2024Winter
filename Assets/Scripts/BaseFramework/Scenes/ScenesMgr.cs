using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class ScenesMgr : Singleton<ScenesMgr>
{
    /// <summary>
    /// 同步 加载场景
    /// </summary>
    /// <param name="name">场景名称</param>
    /// <param name="onComplete">加载完毕后回调</param>
    public void LoadScene(string name, UnityAction onComplete = null)
    {
        //场景同步加载
        SceneManager.LoadScene(name);
        //加载完成过后 才会去执行fun
        onComplete?.Invoke();
    }

    /// <summary>
    /// 同步 加载场景
    /// </summary>
    /// <param name="buildIndex">场景build序号</param>
    /// <param name="onComplete">加载完毕后回调</param>
    public void LoadScene(int buildIndex, UnityAction onComplete = null)
    {
        //场景同步加载
        SceneManager.LoadScene(buildIndex);
        //加载完成过后 才会去执行fun
        onComplete?.Invoke();
    }

    /// <summary>
    /// 异步 加载场景
    /// </summary>
    /// <param name="name">场景名称</param>
    /// <param name="onComplete">加载完成后回调函数</param>
    /// <param name="onProgress">加载场景时回调加载的进度</param>
    public void LoadSceneAsyn(string name, UnityAction onComplete=null, UnityAction<float> onProgress=null)
    {
        MonoMgr.Ins.StartCoroutine(ReallyLoadSceneAsyn(name, onComplete, onProgress));
    }

    /// <summary>
    /// 异步 加载场景
    /// </summary>
    /// <param name="buildIndex">场景build序号</param>
    /// <param name="onComplete">加载完成后回调函数</param>
    /// <param name="onProgress">加载场景时回调加载的进度</param>
    public void LoadSceneAsyn(int buildIndex, UnityAction onComplete = null, UnityAction<float> onProgress = null)
    {
        MonoMgr.Ins.StartCoroutine(ReallyLoadSceneAsyn(buildIndex, onComplete, onProgress));
    }

    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction onComplete=null, UnityAction<float> onProgress=null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //可以得到场景加载的一个进度
        while(!ao.isDone)
        {
            onProgress?.Invoke(ao.progress);
            //这里面去更新进度条
            yield return ao.progress;
        }
        //加载完成过后 才会去执行fun
        onComplete?.Invoke();
    }

    private IEnumerator ReallyLoadSceneAsyn(int buildIndex, UnityAction onComplete = null, UnityAction<float> onProgress = null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(buildIndex);
        //可以得到场景加载的一个进度
        while (!ao.isDone)
        {
            onProgress?.Invoke(ao.progress);
            //这里面去更新进度条
            yield return ao.progress;
        }
        //加载完成过后 才会去执行fun
        onComplete?.Invoke();
    }
}
