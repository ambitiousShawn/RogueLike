using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCJB : FlyableFire
{
    //蓝色冲击波的数据
    WeaponInfo info = DataManager.Instance.weaponInfos[3];

    protected override void OnEnable()
    {
        base.OnEnable();

        //Test:给速度赋值，后期读表
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
            if (fsm != null)
            {
                fsm.parameter.getHit = true;
                //伤害值后期读表
                fsm.Hit(info.Damage);
            }
            else
            {
                fsm_boss.parameter.getHit = true;
                fsm_boss.Hit(info.Damage);
            }
        }
    }
    #endregion
}
