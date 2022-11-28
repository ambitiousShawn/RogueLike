using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager 
{

    #region 单例模式
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    #endregion

    #region 收集物管理
    //金币
    public int goldCoin = 0;
    //钥匙
    public int key = 0;
    //炸弹
    public int bomb = 0;
    //鸡腿
    public int chicken = 0;
    #endregion

    #region 玩家可变化的属性
    //最大生命值
    public int maxHealth;
    //当前生命值
    public int currHealth;
    //当前攻击力
    public int currDamage;
    //当前移动速度
    public float speed;
    #endregion

    #region 数据文件
    public List<PlayerInfo> playerInfos;
    public List<WeaponInfo> weaponInfos;
    //音量大小
    public float BGM_Volume;
    //音效大小
    public float Sound_Volume;
    #endregion

    public DataManager()
    {
        playerInfos = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
        weaponInfos = JsonMgr.Instance.LoadData<List<WeaponInfo>>("WeaponInfo");
        BGM_Volume = 1;
        Sound_Volume = 1;
    }
}
