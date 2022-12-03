using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    子弹被创建出来后执行的操作
 */
public class Fire : FlyableFire
{
    //普通子弹的数据文件
    WeaponInfo info = DataManager.Instance.weaponInfos[0];

    protected override void OnEnable()
    {
        base.OnEnable();
       
        speed = info.FlySpeed;
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
            PoolManager.Instance.SetElement(info.Resource, gameObject);
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
                fsm.Hit(info.Damage);
            }
            else if (fsm_boss != null)
            {
                fsm_boss.parameter.getHit = true;
                fsm_boss.Hit(info.Damage);
            }
            else
            {
                fsm_kun.Hit(info.Damage);
            }
            //进入缓存池
            PoolManager.Instance.SetElement(info.Resource, gameObject);
            needDestroy = false;
        }
    }
    #endregion

    #region 碰撞器相关
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            print("碰撞到了");
            PoolManager.Instance.SetElement(info.Resource, gameObject);
        }
    }
    #endregion
}
