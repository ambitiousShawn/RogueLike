using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ��Ϸ���̵Ĺ���
 */

public class GameManager : SingletonMono<GameManager>
{
    //��ǰ�ؿ��Ƿ���Թ���
    public bool isSucceed;
    //��ǰ�ؿ�ʣ��Ĺ�������
    public int enemyNum;

    void Start()
    {
        enemyNum = 1;
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
}
