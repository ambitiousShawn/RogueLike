using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
    ʹ��˵������Ҫ��Resources��UIĿ¼�´���CanvasԤ����(���ú���ز�����)
              ��Ҫ����EventSystemԤ���塣

    �ṩ���ܣ�
        1.ͨ���㼶ö�ٵõ���Ӧ�ĸ�����
        2.��ʾĳ���
        3.����ĳ���
        4.�õ�ĳ����������
        5.���ؼ�����Զ����¼�����
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

        //�ҵ����㼶
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        GameObject.DontDestroyOnLoad(Resources.Load<GameObject>("UI/EventSystem"));
    }

    //ͨ���㼶ö�ٵõ���Ӧ�ĸ�����
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

    //��ʾĳ���
    //Param3:�����Ԥ���崴���ɹ�ʱ����Ҫ���õ��߼�
    public void ShowPanel<T>(string panelName, E_UI_Layer layer,UnityAction<T> callback = null) where T : BasePanel
    {
        //�������Ѿ����ڣ���ֱ�ӻص���������
        if (panelDic.ContainsKey(panelName))
        {
            if (callback != null)
                callback(panelDic[panelName] as T);

            return;
        }

        //�첽�������
        ResourcesManager.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //Ĭ��Ϊ�ײ�
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
            //���ø�����
            obj.transform.SetParent(father);

            //�������λ�úʹ�С
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = obj.GetComponent<T>();
            if (callback != null)
                callback(panel);

            //�洢���
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

    //���ؼ�����Զ����¼�����
    //Param1:�ؼ�����
    //Param2:�¼�����
    //Param3:�¼�����ִ���߼�
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
