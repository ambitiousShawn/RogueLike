using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Dropdown Dropdown_ChooseHero;
    private Button Btn_Start;
    private Button Btn_Choose;
    private Button Btn_Setting;
    private Button Btn_About;
    private Button Btn_Exit;

    //当前选择的英雄编号
    private int role = 0;

    protected override void Init()
    {
        Dropdown_ChooseHero = GetControl<Dropdown>("Dropdown_ChooseHero");
        Btn_Start = GetControl<Button>("Btn_Start");
        Btn_Choose = GetControl<Button>("Btn_Choose");
        Btn_Setting = GetControl<Button>("Btn_Setting");
        Btn_About = GetControl<Button>("Btn_About");
        Btn_Exit = GetControl<Button>("Btn_Exit");

        //选英雄，当选完时，改变Manager中当前英雄的序号
        Dropdown_ChooseHero.onValueChanged.AddListener((value) =>
        {
            role = value;
        });

        //开始游戏，将选择的英雄信息赋值给全局变量，并切换场景即可
        Btn_Start.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("MainPanel");
            DataManager.Instance.role = role;
            //此处涉及异步切换场景，可以使用协程
            StartCoroutine(IE_LoadSceneAsync());
        });

        //选择英雄按钮，切换选择英雄下选框的显示与否
        Btn_Choose.onClick.AddListener(() =>
        {
            if (Dropdown_ChooseHero.gameObject.activeInHierarchy)
                Dropdown_ChooseHero.gameObject.SetActive(false);
            else
                Dropdown_ChooseHero.gameObject.SetActive(true);
        });

        //设置面板，点击弹出设置并隐藏主面板即可
        Btn_Setting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
            UIManager.Instance.HidePanel("MainPanel",false);
        });

        //TODO:关于，后期设计关于面板再添加功能
        Btn_About.onClick.AddListener(() =>
        {
            print("About被点击啦！");
        });

        //退出，直接退出即可
        Btn_Exit.onClick.AddListener(() =>
        {
            print("Exit被点击啦！");
            Application.Quit();
        });
    }

    //异步切换场景协程
    IEnumerator IE_LoadSceneAsync()
    {
        //异步加载场景
        yield return SceneManager.LoadSceneAsync(1);
    }
}
