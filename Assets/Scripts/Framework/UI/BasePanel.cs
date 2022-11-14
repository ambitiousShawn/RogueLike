using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  使用方法：让面板类继承该类，直接在子类中书写核心代码即可，无需再写通用的匹配组件的逻辑。
 * 
    需要找到自己面板下的控件对象
    他应该提供显示 或 隐藏自己的行为
 */
public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Slider>();
    }

    //显示自己
    public virtual void ShowPanel()
    {

    }

    //隐藏自己
    public virtual void HideMe()
    {

    }

    //按钮的点击事件
    //Param1:按钮所在游戏对象名
    protected virtual void OnClick(string btnName)
    {

    }

    //单选框的改值事件
    //Param1:单选框所在游戏对象名
    //Param2:单选框默认状态值
    protected virtual void OnValueChanged(string toggleName,bool value)
    {

    }

    //找到子对象的对应控件
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();

        foreach (T control in controls)
        {
            string objName = control.gameObject.name;
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(control);
            else
                controlDic.Add(objName, new List<UIBehaviour>() { control });

            //如果是按钮组件，自动为其添加点击监听
            if (control is Button)
            {
                (control as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //如果是单选框组件，自动为其添加改值监听
            else if (control is Toggle)
            {
                (control as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }

    //得到对应名字的对应脚本
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            //如果字典中存在对应对象
            for (int i = 0;i < controlDic[controlName].Count; ++i)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }
}
