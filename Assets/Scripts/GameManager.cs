using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    ��Ϸ���̵Ĺ���
 */

public class GameManager : MonoBehaviour
{
    #region ����ģʽ
    private static GameManager instance;
    public static GameManager Instance => instance;
    #endregion

    //��ǰ�ؿ��Ƿ���Թ���
    public bool isSucceed;
    //��ǰ�ؿ�ʣ��Ĺ�������
    public int enemyNum;

    //��Ҷ���
    public PlayerInfo player;

    public GameObject playerObj;
    //��ǰ���ѡ��Ľ�ɫ
    public int num;
    //��ǰ���ѡ�������
    public int weaponNum;

    //����Ƿ�����
    public bool isDead;
    //����Ƿ񵽴�Boss��
    public bool isArrive;
    //Boss����
    public GameObject boss;

    #region �ռ������
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
        //���������ͼ
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

    //��ʼ����ҵ��߼�
    private void Init()
    {
        playerObj = ResourcesManager.Instance.Load<GameObject>(player.Resource);
        playerObj.transform.position = Vector2.zero;
        playerObj.transform.rotation = Quaternion.identity;
        playerObj.name = "Player";
    }
}
