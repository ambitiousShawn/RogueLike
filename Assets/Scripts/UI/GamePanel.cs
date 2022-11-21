using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //滑动条组件
    private Scrollbar Scrollbar_Atk;
    //左下武器列表
    private Image weapon1;
    private Image weapon2;
    private Image weapon3;
    private Image weapon4;
    //右下子弹数量以及当前武器
    private Text Text_BulletNum;
    private Image currWeapon;

    protected override void Init()
    {
        //组件赋值
        Scrollbar_Atk = GetControl<Scrollbar>("Scrollbar_Atk");
        weapon1 = GetControl<Image>("weapon1");
        weapon2 = GetControl<Image>("weapon2");
        weapon3 = GetControl<Image>("weapon3");
        weapon4 = GetControl<Image>("weapon4");

        Text_BulletNum = GetControl<Text>("Text_BulletNum");
        currWeapon = GetControl<Image>("currWeapon");

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

    //切换武器图标及其弹夹数量
    public void SwitchWeapon(int currNum,int maxNum,int i)
    {
        //更新弹夹数
        Text_BulletNum.text = currNum + " / " + maxNum;
        //更新右下图标
        currWeapon.sprite = GetControl<Image>("weapon" + DataManager.Instance.weaponInfos[i].ID).sprite;
    }
}
