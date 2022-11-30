using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    该类从始至终不会被销毁，作为全局数据管理器使用。
 */

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

    #region 玩家信息
    //当前玩家选择的角色编号
    public int role;
    //当前玩家使用的武器编号
    public int weapon;
    //每个房间的怪物数量
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
