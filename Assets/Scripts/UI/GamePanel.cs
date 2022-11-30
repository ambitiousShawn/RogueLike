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
        UpdateBloodBar(LevelManager.Instance.player.Health, LevelManager.Instance.player.Health);
        //初始化收集物UI
        UpdateCollections(0, 0, 0, 0);
        //初始化武器信息
        SwitchWeapon(999, 999);
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
        //更新右下图标(改为加载资源文件)
        currWeapon.sprite = Resources.Load<Sprite>(DataManager.Instance.weaponInfos[DataManager.Instance.weapon].UISprite);
    }

    //初始化角色头像
    public void InitPlayerHead()
    {
        Img_RoleHead.sprite = Resources.Load<Sprite>(LevelManager.Instance.player.UISprite);
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
        DataManager.Instance.bomb += bombNum;
        DataManager.Instance.key += keyNum;
        DataManager.Instance.goldCoin += moneyNum;
        DataManager.Instance.chicken += chicken;
        Txt_BombNum.text = DataManager.Instance.bomb .ToString();
        Txt_KeyNum.text = DataManager.Instance.key .ToString();
        Txt_MoneyNum.text = DataManager.Instance.goldCoin .ToString();
        Txt_Chicken.text = DataManager.Instance.chicken .ToString();
    }

}
