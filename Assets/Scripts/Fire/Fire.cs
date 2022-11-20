using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �ӵ�������������ִ�еĲ���
 */
public class Fire : MonoBehaviour
{
    //���ڶ�����ֵ��Ŀǰ�ȸ�Ĭ��ֵ
    private float speed;

    //��ȡ��ɫ�������ж��ӵ��˶�����
    private Transform player;
    private int shootDir;
    //�Ƿ���Ҫ��ʱ����
    private bool needDestroy = true;

    void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        //�洢�ӵ����������
        shootDir = player.localScale.x > 0 ? 1 : -1;
        speed = 6;

        Invoke("DestroyBullet", 3f);
    }

    void Update()
    {
        //�ӵ����ƶ�
        transform.Translate(shootDir * speed * Time.deltaTime, 0, 0);
    }

    //�ֶ������ӵ�
    private void DestroyBullet()
    {
        if (needDestroy)
        {
            PoolManager.Instance.SetElement("Fire/�����ӵ�", gameObject);
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
            fsm.Hit(2);
            fsm.parameter.getHit = true;
            //���뻺���
            PoolManager.Instance.SetElement("Fire/�����ӵ�", gameObject);
            needDestroy = false;
        }
    }
    #endregion
}
