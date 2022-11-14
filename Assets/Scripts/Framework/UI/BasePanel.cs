using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  ʹ�÷������������̳и��ֱ࣬������������д���Ĵ��뼴�ɣ�������дͨ�õ�ƥ��������߼���
 * 
    ��Ҫ�ҵ��Լ�����µĿؼ�����
    ��Ӧ���ṩ��ʾ �� �����Լ�����Ϊ
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

    //��ʾ�Լ�
    public virtual void ShowPanel()
    {

    }

    //�����Լ�
    public virtual void HideMe()
    {

    }

    //��ť�ĵ���¼�
    //Param1:��ť������Ϸ������
    protected virtual void OnClick(string btnName)
    {

    }

    //��ѡ��ĸ�ֵ�¼�
    //Param1:��ѡ��������Ϸ������
    //Param2:��ѡ��Ĭ��״ֵ̬
    protected virtual void OnValueChanged(string toggleName,bool value)
    {

    }

    //�ҵ��Ӷ���Ķ�Ӧ�ؼ�
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

            //����ǰ�ť������Զ�Ϊ����ӵ������
            if (control is Button)
            {
                (control as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //����ǵ�ѡ��������Զ�Ϊ����Ӹ�ֵ����
            else if (control is Toggle)
            {
                (control as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }

    //�õ���Ӧ���ֵĶ�Ӧ�ű�
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            //����ֵ��д��ڶ�Ӧ����
            for (int i = 0;i < controlDic[controlName].Count; ++i)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }
}
