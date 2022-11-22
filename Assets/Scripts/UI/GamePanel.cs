using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //���������
    private Scrollbar Scrollbar_Atk;
    //�����ռ����б�
    private Image c1;
    private Image c2;
    private Image c3;
    private Image c4;
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
        c1 = GetControl<Image>("c1");
        c2 = GetControl<Image>("c2");
        c3 = GetControl<Image>("c3");
        c4 = GetControl<Image>("c4");

        Text_BulletNum = GetControl<Text>("Text_BulletNum");
        currWeapon = GetControl<Image>("currWeapon");

        Img_RoleHead = GetControl<Image>("Img_RoleHead");
        Scrollbar_Blood = GetControl<Scrollbar>("Scrollbar_Blood");
        Text_Health = GetControl<Text>("Text_Health");

        Btn_Settings = GetControl<Button>("Btn_Settings");

        SwitchBarState(false);

        //��ʼ��Ѫ��UI
        InitPlayerHead();
        UpdateBloodBar(GameManager.Instance.player.Health, GameManager.Instance.player.Health);
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
    public void SwitchWeapon(int currNum, int maxNum, int i)
    {
        //���µ�����
        Text_BulletNum.text = currNum + " / " + maxNum;
        //TODO:��������ͼ��(��Ϊ������Դ�ļ�)
        currWeapon.sprite = GetControl<Image>("c" + DataManager.Instance.weaponInfos[i].ID).sprite;
    }

    //��ʼ����ɫͷ��
    public void InitPlayerHead()
    {
        Img_RoleHead.sprite = Resources.Load<Sprite>(GameManager.Instance.player.UISprite);
    }

    //���½�ɫ��Ϣ(Ѫ��)
    public void UpdateBloodBar(int currBlood , int maxBlood)
    {
        //�޸Ļ�����
        Scrollbar_Blood.size = (float)currBlood / maxBlood;
        //�޸��ı�
        Text_Health.text = currBlood + "/" + maxBlood;
    }
}
