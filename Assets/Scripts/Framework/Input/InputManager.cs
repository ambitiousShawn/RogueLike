using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    //检测是否开启
    private bool isOpen = true;

    public InputManager()
    {
        //添加Update的监听
        MonoManager.Instance.AddUpdateListener(InputUpdate);
    }

    private void InputUpdate()
    {
        if (!isOpen) 
            return;
        CheckKey(KeyCode.T);
    }

    private void CheckKey(KeyCode keycode)
    {
        if (Input.GetKeyDown(keycode))
            EventManager.Instance.EventTrigger("KeyDown", keycode);
        if (Input.GetKeyUp(keycode))
            EventManager.Instance.EventTrigger("KeyUp", keycode);
        if (Input.GetKey(keycode))
            EventManager.Instance.EventTrigger("Key", keycode);
    }

    //是否开启输入检测
    public void SwitchState(bool isOpen)
    {
        this.isOpen = isOpen;
    }
}
