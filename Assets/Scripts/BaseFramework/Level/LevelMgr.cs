using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelMgr : Singleton<LevelMgr>
{
    private string lastLevel = "";

    private void AfterLevelLoad(string sceneName, UnityAction onComplete = null)
    {
        if (sceneName != "Home")
            lastLevel = sceneName;
        onComplete?.Invoke();
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    /// <param name="targetScene">场景名</param>
    public void LevelLoad(string targetScene)
    {
        ScenesMgr.Ins.LoadSceneAsyn(targetScene
            , () =>
            {
                AfterLevelLoad(targetScene);
            }, (float progress) =>
            {
                Debug.Log("progress = " + progress.ToString());
            }
        );
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    /// <param name="targetScene">场景名</param>
    public void LevelLoad(string targetScene, UnityAction onComplete = null)
    {
        ScenesMgr.Ins.LoadSceneAsyn(targetScene
            , () =>
            {
                AfterLevelLoad(targetScene, onComplete);
            }, (float progress) =>
            {
                Debug.Log("progress = " + progress.ToString());
            }
        );
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    /// <param name="targetScene">场景build序号</param>
    public void LevelLoad(int targetScene)
    {
        ScenesMgr.Ins.LoadSceneAsyn(targetScene
            , () =>
            {
                Scene s = SceneManager.GetSceneByBuildIndex(targetScene);
                //Debug.Log("BuildIndex - Name : "+s.buildIndex.ToString()+" - "+s.name);
                AfterLevelLoad(s.name);
            }, (float progress) =>
            {
                Debug.Log("progress = " + progress.ToString());
            }
        );
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    /// <param name="targetScene">场景build序号</param>
    public void LevelLoad(int targetScene, UnityAction onComplete = null)
    {
        ScenesMgr.Ins.LoadSceneAsyn(targetScene
            , () =>
            {
                Scene s = SceneManager.GetSceneByBuildIndex(targetScene);
                Debug.Log("BuildIndex - Name : " + s.buildIndex.ToString() + " - " + s.name);
                AfterLevelLoad(s.name, onComplete);
            }, (float progress) =>
            {
                Debug.Log("progress = " + progress.ToString());
            }
        );
    }

    /// <summary>
    /// 重载当前关卡
    /// </summary>
    public void LevelReload()
    {
        Scene scene = SceneManager.GetActiveScene();
        LevelLoad(scene.name);
    }

    /// <summary>
    /// 自动加载下一关，按build序号切换至下一场景
    /// 超出场景数，则切至主页面
    /// </summary>
    /// <param name="playLevelBGM">是否播放关卡BGM</param>
    public void LevelNext(bool playLevelBGM = true)
    {
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        if(i >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Target Scene "+i+" Out Of Build Scenes");
            i = 0;
        }
        else if(playLevelBGM)
        {
            LevelLoad(i
                , () =>
                {
                    //*MusicMgr.Ins.PlayBGM("Level");
                });
            return;
        }     
        LevelLoad(i);
    }

    /// <summary>
    /// 返回主页
    /// </summary>
    public void Home()
    {
        LevelLoad("Home"
            //, () =>
            //{
            //MusicMgr.Ins.StopBGM();
            //}
            );
    }

    /// <summary>
    /// 继续进度
    /// </summary>
    public void Continue()
    {
        LevelNext();
        
        //* LevelLoad(lastLevel==""?"1":lastLevel
        //     , () =>
        //     {
        //         MusicMgr.Ins.PlayBGM("Level");
        //     }
        //     );
    }
}
