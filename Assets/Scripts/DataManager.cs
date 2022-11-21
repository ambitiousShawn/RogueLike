using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager 
{

    #region ����ģʽ
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    #endregion

    #region �����ļ�
    public List<PlayerInfo> playerInfos;
    public List<WeaponInfo> weaponInfos;
    #endregion

    public DataManager()
    {
        playerInfos = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
        weaponInfos = JsonMgr.Instance.LoadData<List<WeaponInfo>>("WeaponInfo");
    }
}
