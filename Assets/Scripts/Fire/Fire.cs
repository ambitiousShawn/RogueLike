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
            //ת�����˵�����״̬
            FSM fsm = collision.GetComponent<FSM>();
            FSM_Boss fsm_boss = collision.GetComponent<FSM_Boss>();
            FSM_Kun fsm_kun = collision.GetComponent<FSM_Kun>();
            if (fsm != null)
            {
                fsm.parameter.getHit = true;
                //�˺�ֵ���ڶ���
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
