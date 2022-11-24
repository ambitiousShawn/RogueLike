using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �ӵ�������������ִ�еĲ���
 */
public class Fire : FlyableFire
{
    //��ͨ�ӵ��������ļ�
    WeaponInfo info = DataManager.Instance.weaponInfos[0];

    protected override void OnEnable()
    {
        base.OnEnable();
       
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
            //TODO:�˺���ֵ����
            fsm.Hit(info.Damage);
            fsm.parameter.getHit = true;
            //���뻺���
            PoolManager.Instance.SetElement(info.Resource, gameObject);
            needDestroy = false;
        }
    }
    #endregion

    #region ��ײ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            print("��ײ����");
            PoolManager.Instance.SetElement(info.Resource, gameObject);
        }
    }
    #endregion
}
