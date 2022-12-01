using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    ��Ϸ���̵Ĺ���
 */

public class LevelManager : MonoBehaviour
{
    #region ����ģʽ
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

    //��ǰ�ؿ��Ƿ���Թ���
    public bool isSucceed;
    //��ǰ�ؿ�ʣ��Ĺ�������
    public int enemyNum;

    //��Ҷ���
    public PlayerInfo player;

    public GameObject playerObj;

    //����Ƿ�����
    public bool isDead;
    //����Ƿ񵽴�Boss��
    public bool isArrive;
    //Boss����
    public GameObject boss;
    //��ϷUI����
    public GamePanel gamePanel;
    //��ʾUI
    public TipsPanel tipsPanel;

    void Start()
    {
        print("Startִ����");
        print(DataManager.Instance.role);
        player = DataManager.Instance.playerInfos[DataManager.Instance.role];
        //Bug��¼����ʼ����Ϸ������Start�лᱨ�գ��첽���ص��Ӻ���
        InitPlayer();
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel", (panel) =>
        {
            gamePanel = panel;
            //print("43�д�ӡ��Ϣ"+gamePanel);
        });

        //print("46�д�ӡ��Ϣ"+gamePanel);
        UIManager.Instance.ShowPanel<MiniMapPanel>("MiniMapPanel");

        UIManager.Instance.ShowPanel<TipsPanel>("TipsPanel", (panel) =>
        {
            tipsPanel = panel;
        });

        //����BGM
        AudioManager.Instance.PlayBGM("���³�");
    }



    void Update()
    {
        //���������ͼ
        if (Input.GetKeyDown(KeyCode.T))
        {
            ScenesManager.Instance.LoadSceneAsync("Level02",() =>
            {
                InitPlayer();
                print(playerObj);
            });
        }

        //���԰���
        if (Input.GetKeyDown(KeyCode.G))
        {
            LevelManager.Instance.gamePanel.UpdateBossBar(false);
            tipsPanel.UpdateInfo("����������", 2f);
        }
        //������������ʱ����Ϸ��ͣ
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

    //��ʼ����ҵ��߼�(�л�������ʱ�����µ���)
    public void InitPlayer()
    {
        playerObj = ResourcesManager.Instance.Load<GameObject>(player.Resource);
        playerObj.transform.position = Vector2.zero;
        playerObj.transform.rotation = Quaternion.identity;
        playerObj.name = "Player";
    }
}
