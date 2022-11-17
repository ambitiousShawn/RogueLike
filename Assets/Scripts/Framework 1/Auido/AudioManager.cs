using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : Singleton<AudioManager>
{
    //BGM组件
    private AudioSource BGM_Audio = GameObject.Find("Audio").GetComponent<AudioSource>();

    //音效组件
    private GameObject Sound_Obj = GameObject.Find("Sound");
    private List<AudioSource> soundList = new List<AudioSource>();

    //控制音效播放组件的移除
    public AudioManager()
    {
        //加入帧更新测试，如果音效播放完成则删除组件
        MonoManager.Instance.AddUpdateListener(() =>
        {
            for (int i = soundList.Count - 1;i >= 0; --i)
            {
                if (!soundList[i].isPlaying)
                {
                    GameObject.Destroy(soundList[i]);
                    soundList.Remove(soundList[i]);
                }   
            }
        });
    }

    #region BGM相关
    //开始播放BGM
    /*
        Param1:bgm在Resources目录下"Audio/BGM/"的文件名
        Param2:bgm是否循环播放
        Param3:bgm播放音量大小
        Param4:是否开局播放
    */
    public void PlayBGM(string bgmName, bool isLoop = true, float volume = 0.8f)
    {
        if (BGM_Audio == null)
            BGM_Audio = new GameObject("Audio").AddComponent<AudioSource>();
        //异步加载
        ResourcesManager.Instance.LoadAsync<AudioClip>("Audio/BGM/" + bgmName, (clip) =>
       {
           BGM_Audio.clip = clip;
           BGM_Audio.loop = isLoop;
           BGM_Audio.volume = volume;
           BGM_Audio.Play();
       });
    }

    //停止音乐播放
    public void StopBGM()
    {
        if (BGM_Audio == null) return;
        BGM_Audio.Stop();
    }

    //暂停播放音乐
    public void PauseBGM()
    {
        if (BGM_Audio == null) return;
        BGM_Audio.Pause();
    }

    //改变背景音乐音量
    public void SwitchVolume(float volume)
    {
        if (BGM_Audio == null) return;
        //对音量大小做个限制(0~1)
        if (volume > 1 || volume < 0) return; 
        BGM_Audio.volume = volume;
    }
    #endregion

    #region 音效相关
    //播放音效
    public void PlaySound(string soundName,UnityAction<AudioSource> callback = null ,bool isLoop = false, float volume = 0.8f)
    {
        if (Sound_Obj == null)
            Sound_Obj = new GameObject("Sound");
        //异步加载资源
        ResourcesManager.Instance.LoadAsync<AudioClip>("Audio/Sound/" + soundName, (clip) =>
         {
             AudioSource sound_Audio = Sound_Obj.AddComponent<AudioSource>();
             sound_Audio.clip = clip;
             sound_Audio.loop = isLoop;
             sound_Audio.volume = volume;
             sound_Audio.Play();
             //加入列表
             soundList.Add(sound_Audio);
             if (callback != null)
             {
                 callback(sound_Audio);
             }
         });
    }

    //停止音效
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            source.Stop();
            soundList.Remove(source);
            GameObject.Destroy(source);
        }
    }
    #endregion
}
