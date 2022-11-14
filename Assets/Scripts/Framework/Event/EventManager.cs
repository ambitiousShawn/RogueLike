using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    使用方法：
        对需要监听某事件的游戏对象AddEventListener，当对应事件
        触发时，事件中心会依次对监听了该事件的对象执行对应逻辑。
        书写泛型重载。
        利用泛型和里式转换原则避免了从object转类型的装箱拆箱，并通过方法重载
        提高了时间管理中心的泛用性。
 */
public interface IEventInfo{  };

//泛型包装类
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions; 

    public EventInfo(UnityAction<T> actions)
    {
        this.actions = actions;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction actions)
    {
        this.actions = actions;
    }
}

public class EventManager : Singleton<EventManager>
{
    //存储事件的容器：事件名 = 具体委托
    private Dictionary<string, IEventInfo> events = new Dictionary<string, IEventInfo>();


    #region 添加事件监听
    //添加事件监听
    public void AddEventListener<T>(string name,UnityAction<T> action)
    {

        if (events.ContainsKey(name))
        {
            //存在对应事件
            (events[name] as EventInfo<T>).actions += action;
        }
        else
        {
            //不存在对应事件
            events.Add(name, new EventInfo<T>(action));
        }
    }

    public void AddEventListener(string name, UnityAction action)
    {

        if (events.ContainsKey(name))
        {
            //存在对应事件
            (events[name] as EventInfo).actions += action;
        }
        else
        {
            //不存在对应事件
            events.Add(name, new EventInfo(action));
        }
    }
    #endregion


    #region 事件触发
    //事件触发,可能传递参数
    public void EventTrigger<T>(string name,T param)
    {
        if (events.ContainsKey(name))
        {
            //当存在对应事件时
            (events[name] as EventInfo<T>).actions?.Invoke(param);
        }
    }

    public void EventTrigger(string name)
    {
        if (events.ContainsKey(name))
        {
            //当存在对应事件时
            (events[name] as EventInfo).actions?.Invoke();
        }
    }

    #endregion


    #region 移除事件监听
    //事件中心有加就有减
    //移除事件监听
    public void RemoveEventListener<T>(string name,UnityAction<T> action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo<T>).actions -= action;
        }
    }

    public void RemoveEventListener(string name, UnityAction action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo).actions -= action;
        }
    }

    #endregion

    //清空事件中心
    //主要用于场景切换
    public void ClearAll()
    {
        events.Clear();
    }
}
