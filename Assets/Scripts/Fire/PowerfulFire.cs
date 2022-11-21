using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerfulFire : FlyableFire
{
    WeaponInfo info = DataManager.Instance.weaponInfos[1];

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
            FSM fsm = collision.GetComponent<FSM>();
            fsm.Hit(info.Damage);
            fsm.parameter.getHit = true;
        }
    }
    #endregion
}
