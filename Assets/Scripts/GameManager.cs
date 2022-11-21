using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    游戏进程的管理
 */

public class GameManager : MonoBehaviour
{
    #region 单例模式
    private static GameManager instance;
    public static GameManager Instance => instance;
    #endregion

    //当前关卡是否可以过关
    public bool isSucceed;
    //当前关卡剩余的怪物数量
    public int enemyNum;

    //玩家对象
    public PlayerInfo player;

    public GameObject playerObj;
    //当前玩家选择的角色
    public int num;
    //当前玩家选择的武器
    public int weaponNum;

    private void Awake()
    {
        instance = this;
        player = DataManager.Instance.playerInfos[num];
    }

    void Start()
    {
        enemyNum = 0;
        UIManager.Instance.ShowPanel<MiniMapPanel>("MiniMapPanel");
        Init();
    }

    void Update()
    {
        if (enemyNum <= 0)
        {
            isSucceed = true;
        }
        else
        {
            isSucceed = false;
        }

    }

    //初始化玩家的逻辑
    private void Init()
    {
        playerObj = ResourcesManager.Instance.Load<GameObject>(player.Resource);
        playerObj.transform.position = Vector2.zero;
        playerObj.transform.rotation = Quaternion.identity;
        playerObj.name = "Player";
    }
}
