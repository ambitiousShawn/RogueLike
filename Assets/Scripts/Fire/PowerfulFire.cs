using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerfulFire : FlyableFire
{
    WeaponInfo info = DataManager.Instance.weaponInfos[1];

    protected override void OnEnable()
    {
        base.OnEnable();

        //Test:���ٶȸ�ֵ�����ڶ���
        speed = info.FlySpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //�ӵ����ƶ�
        transform.Translate(shootDir * speed * Time.deltaTime, 0, 0);
    }

    //�ֶ������ӵ�
    protected override void DestroyBullet()
    {
        if (needDestroy)
        {
            PoolManager.Instance.SetElement(info.Resource, gameObject);
        }
    }

    #region ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ӵ���������Թ�������˺����������ӵ������������ɲ�����Ч
        if (collision.CompareTag("Enemy"))
        {
            FSM fsm = collision.GetComponent<FSM>();
            fsm.Hit(info.Damage);
            fsm.parameter.getHit = true;
        }
    }
    #endregion
}
