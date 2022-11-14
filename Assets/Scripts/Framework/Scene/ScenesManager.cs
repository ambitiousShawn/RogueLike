using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/*
    ͬ�������л�
    �첽�����л�
 */

public class SceneManager : Singleton<SceneManager>
{
    #region ͬ�����س���
    public void LoadScene(string sceneName,UnityAction callback)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        callback();
    }

    #endregion

    #region �첽���س���
    public void LoadSceneAsync(string sceneName, UnityAction callback)
    {
        MonoManager.Instance.StartCoroutine(IE_LoadSceneAsync(sceneName, callback));
    }

        IEnumerator IE_LoadSceneAsync(string sceneName,UnityAction callback)
    {
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        //�õ����صĽ���
        while (!ao.isDone)
        {
            //ͨ���¼�����������ַ����ؽ���
            EventManager.Instance.EventTrigger<float>("Loading", ao.progress);
            yield return ao.progress;
        }
        callback();
    }
    #endregion
}
