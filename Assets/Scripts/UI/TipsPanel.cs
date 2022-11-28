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

    //修改提示内容(外部调用)
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

    //切换显示状态
    private void SwitchOpen(bool isOpen)
    {
        Img_Tips.gameObject.SetActive(isOpen);
    }
}
