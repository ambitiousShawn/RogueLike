using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //滑动条组件
    private Scrollbar Scrollbar_Atk;
    //左下收集物列表
    private Text Txt_BombNum;
    private Text Txt_KeyNum;
    private Text Txt_MoneyNum;
    private Text Txt_Chicken;

    //右下子弹数量以及当前武器
    private Text Text_BulletNum;
    private Image currWeapon;
    //左上角色信息栏
    private Image Img_RoleHead;
    private Scrollbar Scrollbar_Blood;
    private Text Text_Health;
    //右上设置按钮
    private Button Btn_Settings;

    protected override void Init()
    {
        //组件赋值
        Scrollbar_Atk = GetControl<Scrollbar>("Scrollbar_Atk");
        Txt_BombNum = GetControl<Text>("Txt_BombNum");
        Txt_KeyNum = GetControl<Text>("Txt_KeyNum");
        Txt_MoneyNum = GetControl<Text>("Txt_MoneyNum");
        Txt_Chicken = GetControl<Text>("Txt_Chicken");

        Text_BulletNum = GetControl<Text>("Text_BulletNum");
        currWeapon = GetControl<Image>("currWeapon");

        Img_RoleHead = GetControl<Image>("Img_RoleHead");
        Scrollbar_Blood = GetControl<Scrollbar>("Scrollbar_Blood");
        Text_Health = GetControl<Text>("Text_Health");

        Btn_Settings = GetControl<Button>("Btn_Settings");

        SwitchBarState(false);

        //初始化血条UI
        InitPlayerHead();
        UpdateBloodBar(GameManager.Instance.player.Health, GameManager.Instance.player.Health);
        //初始化收集物UI
        UpdateCollections(0, 0, 0, 0);
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
    public void SwitchWeapon(int currNum, int maxNum)
    {
        //更新弹夹数
        Text_BulletNum.text = currNum + " / " + maxNum;
        //TODO:更新右下图标(改为加载资源文件)
        currWeapon.sprite = Resources.Load<Sprite>(DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum].UISprite);
    }

    //初始化角色头像
    public void InitPlayerHead()
    {
        Img_RoleHead.sprite = Resources.Load<Sprite>(GameManager.Instance.player.UISprite);
    }

    //更新角色信息(血量)
    public void UpdateBloodBar(int currBlood , int maxBlood)
    {
        //修改滑动条
        Scrollbar_Blood.size = (float)currBlood / maxBlood;
        //修改文本
        Text_Health.text = currBlood + "/" + maxBlood;
    }

    //更新收集物信息(炸弹，钥匙，金币，鸡腿)
    public void UpdateCollections(int bombNum = 0,int keyNum = 0,int moneyNum = 0,int chicken = 0)
    {
        GameManager.Instance.bomb += bombNum;
        GameManager.Instance.key += keyNum;
        GameManager.Instance.money += moneyNum;
        GameManager.Instance.chicken += chicken;
        Txt_BombNum.text = GameManager.Instance.bomb .ToString();
        Txt_KeyNum.text = GameManager.Instance.key .ToString();
        Txt_MoneyNum.text = GameManager.Instance.money .ToString();
        Txt_Chicken.text = GameManager.Instance.chicken .ToString();
    }

}
