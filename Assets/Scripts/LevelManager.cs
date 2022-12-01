using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    游戏进程的管理
 */

public class LevelManager : MonoBehaviour
{
    #region 单例模式
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    //当前关卡是否可以过关
    public bool isSucceed;
    //当前关卡剩余的怪物数量
    public int enemyNum;

    //玩家对象
    public PlayerInfo player;

    public GameObject playerObj;

    //玩家是否死亡
    public bool isDead;
    //玩家是否到达Boss房
    public bool isArrive;
    //Boss对象
    public GameObject boss;
    //游戏UI界面
    public GamePanel gamePanel;
    //提示UI
    public TipsPanel tipsPanel;

    void Start()
    {
        print("Start执行力");
        print(DataManager.Instance.role);
        player = DataManager.Instance.playerInfos[DataManager.Instance.role];
        //Bug记录：初始化游戏面板放在Start中会报空！异步加载的延后性
        InitPlayer();
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel", (panel) =>
        {
            gamePanel = panel;
            //print("43行打印信息"+gamePanel);
        });

        //print("46行打印信息"+gamePanel);
        UIManager.Instance.ShowPanel<MiniMapPanel>("MiniMapPanel");

        UIManager.Instance.ShowPanel<TipsPanel>("TipsPanel", (panel) =>
        {
            tipsPanel = panel;
        });

        //播放BGM
        AudioManager.Instance.PlayBGM("地下城");
    }



    void Update()
    {
        //测试随机地图
        if (Input.GetKeyDown(KeyCode.T))
        {
            ScenesManager.Instance.LoadSceneAsync("Level02",() =>
            {
                InitPlayer();
                print(playerObj);
            });
        }

        //测试按键
        if (Input.GetKeyDown(KeyCode.G))
        {
            LevelManager.Instance.gamePanel.UpdateBossBar(false);
            tipsPanel.UpdateInfo("呜呜呜呜呜", 2f);
        }
        //当设置面板存在时，游戏暂停
        if (UIManager.Instance.GetPanel<SettingPanel>("SettingPanel") != null ||
            UIManager.Instance.GetPanel<InstructionPanel>("InstructionPanel") != null)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        if (enemyNum <= 0)
        {
            isSucceed = true;
        }
        else
        {
            isSucceed = false;
        }

    }

    //初始化玩家的逻辑(切换场景的时候重新调用)
    public void InitPlayer()
    {
        playerObj = ResourcesManager.Instance.Load<GameObject>(player.Resource);
        playerObj.transform.position = Vector2.zero;
        playerObj.transform.rotation = Quaternion.identity;
        playerObj.name = "Player";
    }
}
