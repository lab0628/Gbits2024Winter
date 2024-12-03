using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class ExitRegion : MonoBehaviour
{
    [Tooltip("自动跳转至下一关")]
    public bool autoNext = true;
    //#if autoNext
    //    [HideInInspector]
    //#endif
    [Tooltip("手动设置要跳转的build序号")]
    public int sceneIndex = 0;

    private SkeletonAnimation skeleton;
    private bool wait = false;

    void Start()
    {
        skeleton = GetComponentInChildren<SkeletonAnimation>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        int targetLayer = LayerMask.NameToLayer("Player");
        // Debug.Log(targetLayer);
        if(collision.gameObject.layer == targetLayer && !wait)
        {
            wait = true;
            // 播放拾取音效
            MusicMgr.Ins.PlaySound("获得道具/Inventory",false);
            skeleton.gameObject.SetActive(false);
            //Debug.LogWarning("Scene Only For Test");
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.5f);
        if (autoNext)
        {
            LevelMgr.Ins.LevelNext();
        }
        else
        {
            LevelMgr.Ins.LevelLoad(sceneIndex);
        }

        yield return null;
    }
}
