using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �ӵ�������������ִ�еĲ���
 */
public class Sword : FlyableFire
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
            PoolManager.Instance.SetElement("Fire/����", gameObject);
        }
    }

    #region ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ӵ���������Թ�������˺����������ӵ������������ɲ�����Ч
        if (collision.CompareTag("Enemy"))
        {
            FSM fsm = collision.GetComponent<FSM>();
            //TODO:�˺���ֵ����
            fsm.Hit(6);
            fsm.parameter.getHit = true;
        }
    }
    #endregion
}
