using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    统一管理帧更新，使用事件和协程
 */

public class MonoController : MonoBehaviour
{
    //统一管理帧更新的事件
    private event UnityAction updateEvent;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateEvent != null)
            updateEvent.Invoke();
    }

    //给外部提供的添加帧更新的函数
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    //给外部提供的移除帧更新的函数
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
