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

        //键盘输入检测
        CheckKey(KeyCode.M);
        CheckKey(KeyCode.J);
        CheckKey(KeyCode.K);
        CheckKey(KeyCode.Alpha1);
        CheckKey(KeyCode.Alpha2);
        CheckKey(KeyCode.Alpha3);
        CheckKey(KeyCode.Alpha4);
    }

    //键盘相关
    private void CheckKey(KeyCode keycode)
    {
        
        if (Input.GetKeyDown(keycode))
            EventManager.Instance.EventTrigger<KeyCode>("KeyDown", keycode);
        if (Input.GetKeyUp(keycode))
            EventManager.Instance.EventTrigger<KeyCode>("KeyUp", keycode);
        if (Input.GetKey(keycode))
            EventManager.Instance.EventTrigger<KeyCode>("Key", keycode);
    }

    //鼠标相关
    private void CheckMouse(int mouseEvent)
    {
        if (Input.GetMouseButtonDown(mouseEvent))
            EventManager.Instance.EventTrigger<int>("MouseDown", mouseEvent);
        if (Input.GetMouseButtonUp(mouseEvent))
            EventManager.Instance.EventTrigger<int>("MouseUp", mouseEvent);
        if (Input.GetMouseButton(mouseEvent))
            EventManager.Instance.EventTrigger<int>("Mouse", mouseEvent);
    }

    //是否开启输入检测
    public void SwitchState(bool isOpen)
    {
        this.isOpen = isOpen;
    }
}
