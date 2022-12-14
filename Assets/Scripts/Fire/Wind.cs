using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : FlyableFire
{
    //龙卷风的数据文件
    WeaponInfo info = DataManager.Instance.weaponInfos[2];
    //风力强度
    private float force = 80f;


    //龙卷风是否已经到达目的地
    private bool isArrived;

    protected override void OnEnable()
    {
        DestroyTime = 5f;
        base.OnEnable();
        isArrived = false;
        speed = info.FlySpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //子弹的移动
        if (!isArrived)
            transform.Translate(shootDir * speed * Time.deltaTime, 0, 0);
        else
            Adsorption();
    }

    //手动销毁子弹
    protected override void DestroyBullet()
    {
        if (needDestroy)
        {
            PoolManager.Instance.SetElement("Fire/Wind", gameObject);
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

            //龙卷风碰到敌人，停止运动，并且持续吸引周围的敌人
            isArrived = true;
        }

        
    }
    #endregion

    //吸附敌人逻辑
    private void Adsorption()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 5f, 1 << LayerMask.NameToLayer("Enemy"));
        
        foreach (Collider2D c in coll)
        {
            c.GetComponent<Rigidbody2D>().AddForce((transform.position - c.transform.position) * force, ForceMode2D.Force); 
        }
    }
}
