using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager 
{

    #region 单例模式
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    #endregion

    #region 数据文件
    public List<PlayerInfo> playerInfos;
    public List<WeaponInfo> weaponInfos;
    #endregion

    public DataManager()
    {
        playerInfos = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
        weaponInfos = JsonMgr.Instance.LoadData<List<WeaponInfo>>("WeaponInfo");
    }
}
