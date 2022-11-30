using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //���������
    private Scrollbar Scrollbar_Atk;
    //�����ռ����б�
    private Text Txt_BombNum;
    private Text Txt_KeyNum;
    private Text Txt_MoneyNum;
    private Text Txt_Chicken;

    //�����ӵ������Լ���ǰ����
    private Text Text_BulletNum;
    private Image currWeapon;
    //���Ͻ�ɫ��Ϣ��
    private Image Img_RoleHead;
    private Scrollbar Scrollbar_Blood;
    private Text Text_Health;
    //�������ð�ť
    private Button Btn_Settings;

    protected override void Init()
    {
        //�����ֵ
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

        //��ʼ��Ѫ��UI
        InitPlayerHead();
        UpdateBloodBar(LevelManager.Instance.player.Health, LevelManager.Instance.player.Health);
        //��ʼ���ռ���UI
        UpdateCollections(0, 0, 0, 0);
        //��ʼ��������Ϣ
        SwitchWeapon(999, 999);
    }

    //��ʼ�������½�����״̬
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

    //�л�����ͼ�꼰�䵯������
    public void SwitchWeapon(int currNum, int maxNum)
    {
        //���µ�����
        Text_BulletNum.text = currNum + " / " + maxNum;
        //��������ͼ��(��Ϊ������Դ�ļ�)
        currWeapon.sprite = Resources.Load<Sprite>(DataManager.Instance.weaponInfos[DataManager.Instance.weapon].UISprite);
    }

    //��ʼ����ɫͷ��
    public void InitPlayerHead()
    {
        Img_RoleHead.sprite = Resources.Load<Sprite>(LevelManager.Instance.player.UISprite);
    }

    //���½�ɫ��Ϣ(Ѫ��)
    public void UpdateBloodBar(int currBlood , int maxBlood)
    {
        //�޸Ļ�����
        Scrollbar_Blood.size = (float)currBlood / maxBlood;
        //�޸��ı�
        Text_Health.text = currBlood + "/" + maxBlood;
    }

    //�����ռ�����Ϣ(ը����Կ�ף���ң�����)
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
