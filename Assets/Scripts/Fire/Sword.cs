using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    子弹被创建出来后执行的操作
 */
public class Sword : FlyableFire
{
    private int damage = 6;
    protected override void OnEnable()
    {
        base.OnEnable();

        //Test:给速度赋值，后期读表
        speed = 8;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //子弹的移动
        transform.Translate(shootDir * speed * Time.deltaTime, 0, 0);
    }

    //手动销毁子弹
    protected override void DestroyBullet()
    {
        if (needDestroy)
        {
            PoolManager.Instance.SetElement("Fire/剑气", gameObject);
        }
    }

    #region 触发器相关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //子弹碰到怪物，对怪物造成伤害，并销毁子弹，部分武器可产生特效
        if (collision.CompareTag("Enemy"))
        {
            //转换敌人的受伤状态
            FSM fsm = collision.GetComponent<FSM>();
            FSM_Boss fsm_boss = collision.GetComponent<FSM_Boss>();
            FSM_Kun fsm_kun = collision.GetComponent<FSM_Kun>();
            if (fsm != null)
            {
                fsm.parameter.getHit = true;
                //伤害值后期读表
                fsm.Hit(damage);
            }
            else if (fsm_boss != null)
            {
                fsm_boss.parameter.getHit = true;
                fsm_boss.Hit(damage);
            }
            else
            {
                fsm_kun.Hit(damage);
            }
        }
    }
    #endregion
}
