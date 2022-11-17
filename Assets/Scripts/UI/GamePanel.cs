using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //滑动条组件
    private Scrollbar Scrollbar_Atk;

    protected override void Init()
    {
        Scrollbar_Atk = GetControl<Scrollbar>("Scrollbar_Atk");
        SwitchBarState(false);
    }

    //开始蓄力更新进度条状态
    public void UpdateBarState(float currCD , float atkCd)
    {
        Scrollbar_Atk.size = currCD / atkCd;
        Image barImg = Scrollbar_Atk.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        if (Scrollbar_Atk.size == 1)
        {
            barImg.color = Color.red;
        }
        else
        {
            barImg.color = new Color(1,0.6f,0,1);
        }
    }
    public void SwitchBarState(bool isOpen)
    {
        Scrollbar_Atk.gameObject.SetActive(isOpen);
    }
}
