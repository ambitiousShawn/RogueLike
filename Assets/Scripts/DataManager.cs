using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �����ʼ���ղ��ᱻ���٣���Ϊȫ�����ݹ�����ʹ�á�
 */

public class DataManager 
{

    #region ����ģʽ
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    #endregion

    #region �ռ������
    //���
    public int goldCoin = 0;
    //Կ��
    public int key = 0;
    //ը��
    public int bomb = 0;
    //����
    public int chicken = 0;
    #endregion

    #region ��ҿɱ仯������
    //�������ֵ
    public int maxHealth;
    //��ǰ����ֵ
    public int currHealth;
    //��ǰ������
    public int currDamage;
    //��ǰ�ƶ��ٶ�
    public float speed;
    #endregion

    #region �����ļ�
    public List<PlayerInfo> playerInfos;
    public List<WeaponInfo> weaponInfos;
    //������С
    public float BGM_Volume;
    //��Ч��С
    public float Sound_Volume;
    #endregion

    #region �����Ϣ
    //��ǰ���ѡ��Ľ�ɫ���
    public int role;
    //��ǰ���ʹ�õ��������
    public int weapon;
    //ÿ������Ĺ�������
    public int[] enemyNums = new int[] { 0, 2, 4, 2, 1, 1, 2 };
    #endregion

    public DataManager()
    {
        playerInfos = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
        weaponInfos = JsonMgr.Instance.LoadData<List<WeaponInfo>>("WeaponInfo");
        BGM_Volume = 1;
        Sound_Volume = 1;
    }
}
