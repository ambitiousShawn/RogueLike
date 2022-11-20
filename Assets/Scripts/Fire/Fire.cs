using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    子弹被创建出来后执行的操作
 */
public class Fire : MonoBehaviour
{
    //后期读表填值，目前先给默认值
    private float speed;

    //获取角色的面向，判断子弹运动方向
    private Transform player;
    private int shootDir;
    //是否需要延时销毁
    private bool needDestroy = true;

    void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        //存储子弹的射击方向
        shootDir = player.localScale.x > 0 ? 1 : -1;
        speed = 6;

        Invoke("DestroyBullet", 3f);
    }

    void Update()
    {
        //子弹的移动
        transform.Translate(shootDir * speed * Time.deltaTime, 0, 0);
    }

    //手动销毁子弹
    private void DestroyBullet()
    {
        if (needDestroy)
        {
            PoolManager.Instance.SetElement("Fire/火焰子弹", gameObject);
        }
    }

    #region 触发器相关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //子弹碰到怪物，对怪物造成伤害，并销毁子弹，部分武器可产生特效
        if (collision.CompareTag("Enemy"))
        {
            FSM fsm = collision.GetComponent<FSM>();
            //TODO:伤害数值读表
            fsm.Hit(2);
            fsm.parameter.getHit = true;
            //进入缓存池
            PoolManager.Instance.SetElement("Fire/火焰子弹", gameObject);
            needDestroy = false;
        }
    }
    #endregion
}
