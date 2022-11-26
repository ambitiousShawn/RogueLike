using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //玩家是否死亡
    public bool isDead;
    //玩家是否到达Boss房
    public bool isArrive;
    //Boss对象
    public GameObject boss;

    #region 收集物相关
    public int money = 0;
    public int key = 0;
    public int bomb = 0;
    public int chicken = 0;
    #endregion

    private void Awake()
    {
        instance = this;
        player = DataManager.Instance.playerInfos[num];
    }

    void Start()
    {
        enemyNum = 0;
        UIManager.Instance.ShowPanel<MiniMapPanel>("MiniMapPanel");
        isDead = false;
        Init();
    }

    void Update()
    {
        //测试随机地图
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);
        }

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
