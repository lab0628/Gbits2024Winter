using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : Singleton<MusicMgr>
{
    //唯一的背景音乐组件
    private AudioSource bkMusic = null;
    //音乐大小
    private float bkValue = 1;

    //音效依附对象
    private GameObject soundObj = null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效大小
    private float soundValue = 1;

    public MusicMgr()
    {
        MonoMgr.Ins.AddUpdateListener(Update);
    }

    private void Update()
    {
        for( int i = soundList.Count - 1; i >=0; --i )
        {
            if(!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public AudioClip PlayBGM(string name, bool dontDestroyOnLoad = false)
    {
        if(bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BkMusic";
            bkMusic = obj.AddComponent<AudioSource>();
            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(obj);
            }
        }
        //异步加载背景音乐 加载完成后 播放
        var clip = ResMgr.Ins.Load<AudioClip>("Music/BGM/" + name);
        bkMusic.clip = clip;
        bkMusic.loop = true;
        bkMusic.volume = bkValue;
        bkMusic.Play();
        return clip;
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBGM()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    /// <summary>
    /// 改变背景音乐 音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBGMValue(float v)
    {
        bkValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkValue;
    }

    /// <summary>
    /// 获取背景音乐音量大小
    /// </summary>
    /// <param name="v"></param>
    public float GetBGMValue()
    {
        return bkValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
            Object.DontDestroyOnLoad(soundObj);
        }
        //当音效资源异步加载结束后 再添加一个音效
        var clip = ResMgr.Ins.Load<AudioClip>("Music/Sound/" + name);
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = isLoop;
        source.volume = soundValue;
        source.Play();
        soundList.Add(source);
        if(callBack != null)
            callBack(source);
    }

    /// <summary>
    /// 改变所有音效声音大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeAllSoundValue( float value )
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; ++i)
            soundList[i].volume = value;
    }
    
    public void ChangeSoundValue(AudioSource audioSource, float value )
    {
        audioSource.volume = value;
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if( soundList.Contains(source) )
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// 获取音效音量大小
    /// </summary>
    /// <param name="v"></param>
    public float GetSoundValue()
    {
        return soundValue;
    }
}
