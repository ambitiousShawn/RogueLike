using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/*
    同步场景切换
    异步场景切换
 */

public class ScenesManager : Singleton<ScenesManager>
{
    #region 同步加载场景
    public void LoadScene(string sceneName,UnityAction callback)
    {
        SceneManager.LoadScene(sceneName);
        callback();
    }

    #endregion

    #region 异步加载场景
    public void LoadSceneAsync(string sceneName, UnityAction callback)
    {
        MonoManager.Instance.StartCoroutine(IE_LoadSceneAsync(sceneName, callback));
    }

        IEnumerator IE_LoadSceneAsync(string sceneName,UnityAction callback)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        //得到加载的进度
        while (!ao.isDone)
        {
            //通过事件管理器向外分发加载进度
            EventManager.Instance.EventTrigger<float>("Loading", ao.progress);
            yield return ao.progress;
        }
        callback();
    }
    #endregion
}
