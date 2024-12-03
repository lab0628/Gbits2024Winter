using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoHandler : MonoBehaviour
{
    public static VideoHandler instance;
    
    private VideoPlayer videoPlayer;
    private Collider2D coll;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MusicMgr.Ins.StopBGM();
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();

        StartCoroutine(VideoComplete(videoPlayer, () =>
        {
            LevelMgr.Ins.LevelNext();
        }));
        
        
        
    }



    private IEnumerator VideoComplete(VideoPlayer VideoObject, UnityAction action) //方法可以传递参数
    {
        yield return new WaitForSeconds(0.2f);
        while (VideoObject.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }
        action.Invoke();
        yield return null;

    }

    
}
