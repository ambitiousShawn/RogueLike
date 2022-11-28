using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    private Image Img_Tips;
    private Text Txt_Info;

    protected override void Init()
    {
        Img_Tips = GetControl<Image>("Image");
        Txt_Info = GetControl<Text>("Txt_Info");

        SwitchOpen(false);
    }

    //�޸���ʾ����(�ⲿ����)
    public void UpdateInfo(string info , float time)
    {
        Txt_Info.text = info;
        SwitchOpen(true);
        Invoke("DelayHide", time);
    }

    private void DelayHide()
    {
        SwitchOpen(false);
    }

    //�л���ʾ״̬
    private void SwitchOpen(bool isOpen)
    {
        Img_Tips.gameObject.SetActive(isOpen);
    }
}
