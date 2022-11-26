using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �ӵ�������������ִ�еĲ���
 */
public class Cjb : FlyableFire
{
    protected override void OnEnable()
    {
        base.OnEnable();

        //Test:���ٶȸ�ֵ�����ڶ���
        speed = 8;
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
            PoolManager.Instance.SetElement("Fire/�����", gameObject);
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
                fsm.Hit(6);
            }
            else
            {
                fsm_boss.parameter.getHit = true;
                fsm_boss.Hit(6);
            }
        }
    }
    #endregion
}
