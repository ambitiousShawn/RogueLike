using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    游戏进程的管理
 */

public class GameManager : SingletonMono<GameManager>
{
    //当前关卡是否可以过关
    public bool isSucceed;
    //当前关卡剩余的怪物数量
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
