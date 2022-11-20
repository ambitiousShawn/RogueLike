using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager 
{
    private static DataManager instance = new DataManager();

    public static DataManager Instance => instance;

    #region 数据文件
    public List<PlayerInfo> playerInfos;
    #endregion

    public DataManager()
    {
        playerInfos = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
    }
}
