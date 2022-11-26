using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCJB : FlyableFire
{
    //��ɫ�����������
    WeaponInfo info = DataManager.Instance.weaponInfos[3];

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
            //ת�����˵�����״̬
            FSM fsm = collision.GetComponent<FSM>();
            FSM_Boss fsm_boss = collision.GetComponent<FSM_Boss>();
            if (fsm != null)
            {
                fsm.parameter.getHit = true;
                //�˺�ֵ���ڶ���
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
