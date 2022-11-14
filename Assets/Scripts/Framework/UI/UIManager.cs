using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
    使用说明：需要在Resources的UI目录下创建Canvas预制体(设置好相关参数！)
              需要创建EventSystem预制体。

    提供功能：
        1.通过层级枚举得到对应的父对象
        2.显示某面板
        3.隐藏某面板
        4.拿到某个激活的面板
        5.给控件添加自定义事件监听
 */

public enum E_UI_Layer
{
    Bot,Mid,Top,System
}

public class UIManager : Singleton<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot, mid, top, system;

    public RectTransform canvas;

    public UIManager()
    {
        canvas = ResourcesManager.Instance.Load<GameObject>("UI/Canvas").transform as RectTransform;
        GameObject.DontDestroyOnLoad(canvas.gameObject);

        //找到各层级
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        GameObject.DontDestroyOnLoad(Resources.Load<GameObject>("UI/EventSystem"));
    }

    //通过层级枚举得到对应的父对象
    public Transform GetLayerObject(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return bot;
            case E_UI_Layer.Mid:
                return mid;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;

        }
        return null;
    }

    //显示某面板
    //Param3:当面板预设体创建成功时，需要调用的逻辑
    public void ShowPanel<T>(string panelName, E_UI_Layer layer,UnityAction<T> callback = null) where T : BasePanel
    {
        //如果面板已经存在，则直接回调函数即可
        if (panelDic.ContainsKey(panelName))
        {
            if (callback != null)
                callback(panelDic[panelName] as T);

            return;
        }

        //异步加载面板
        ResourcesManager.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //默认为底层
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            //设置父对象
            obj.transform.SetParent(father);

            //设置相对位置和大小
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = obj.GetComponent<T>();
            if (callback != null)
                callback(panel);

            //存储面板
            panelDic.Add(panelName, panel);
        });
    }

    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    //给控件添加自定义事件监听
    //Param1:控件对象
    //Param2:事件类型
    //Param3:事件具体执行逻辑
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
           trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}
