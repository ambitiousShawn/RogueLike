using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  使用方法：让面板类继承该类，直接在子类中书写核心代码即可，无需再写通用的匹配组件的逻辑。
 * 
    需要找到自己面板下的控件对象
    他应该提供显示 或 隐藏自己的行为
 */
public abstract class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    //控制面板透明度
    private CanvasGroup canvasGroup;
    //控制面板淡入淡出的速度
    private float alphaSpeed = 10;

    #region 状态变量
    private bool isShow = false;
    #endregion

    //面板隐藏完毕后，执行的逻辑
    private UnityAction hideCallback = null;

    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Slider>();
        FindChildrenControl<Scrollbar>();

        //获取控制透明度的组件
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        Init();
    }

    void Update()
    {
        if (isShow && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += alphaSpeed;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }

        if (!isShow && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallback?.Invoke();
            }
                
        }
    }

    //初始化
    protected abstract void Init();

    //显示自己
    public virtual void ShowSelf()
    {
        //实现思路：将自身面板透明度设置为0，并设置为显示状态，在Update()函数中使其面板透明度渐变显示即可
        canvasGroup.alpha = 0;
        isShow = true;
    }

    //隐藏自己
    //隐藏面板后需要销毁，但需要透明度完全为0时才可执行之后逻辑，使用委托函数
    public virtual void HideSelf(UnityAction callback)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallback = callback;
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
