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

    //��ǰѡ���Ӣ�۱��
    private int role = 0;

    protected override void Init()
    {
        Dropdown_ChooseHero = GetControl<Dropdown>("Dropdown_ChooseHero");
        Btn_Start = GetControl<Button>("Btn_Start");
        Btn_Choose = GetControl<Button>("Btn_Choose");
        Btn_Setting = GetControl<Button>("Btn_Setting");
        Btn_About = GetControl<Button>("Btn_About");
        Btn_Exit = GetControl<Button>("Btn_Exit");

        //ѡӢ�ۣ���ѡ��ʱ���ı�Manager�е�ǰӢ�۵����
        Dropdown_ChooseHero.onValueChanged.AddListener((value) =>
        {
            role = value;
        });

        //��ʼ��Ϸ����ѡ���Ӣ����Ϣ��ֵ��ȫ�ֱ��������л���������
        Btn_Start.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("MainPanel");
            DataManager.Instance.role = role;
            //�˴��漰�첽�л�����������ʹ��Э��
            StartCoroutine(IE_LoadSceneAsync());
        });

        //ѡ��Ӣ�۰�ť���л�ѡ��Ӣ����ѡ�����ʾ���
        Btn_Choose.onClick.AddListener(() =>
        {
            if (Dropdown_ChooseHero.gameObject.activeInHierarchy)
                Dropdown_ChooseHero.gameObject.SetActive(false);
            else
                Dropdown_ChooseHero.gameObject.SetActive(true);
        });

        //������壬����������ò���������弴��
        Btn_Setting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
            UIManager.Instance.HidePanel("MainPanel",false);
        });

        //TODO:���ڣ�������ƹ����������ӹ���
        Btn_About.onClick.AddListener(() =>
        {
            print("About���������");
        });

        //�˳���ֱ���˳�����
        Btn_Exit.onClick.AddListener(() =>
        {
            print("Exit���������");
            Application.Quit();
        });
    }

    //�첽�л�����Э��
    IEnumerator IE_LoadSceneAsync()
    {
        //�첽���س���
        yield return SceneManager.LoadSceneAsync(1);
    }
}
