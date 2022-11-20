using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �ӵ�������������ִ�еĲ���
 */
public class Sword : MonoBehaviour
{
    //���ڶ�����ֵ��Ŀǰ�ȸ�Ĭ��ֵ
    private float speed;

    //��ȡ��ɫ�������ж��ӵ��˶�����
    private Transform player;
    private int shootDir;

    void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        //�洢�ӵ����������
        shootDir = player.localScale.x > 0 ? 1 : -1;
        if (shootDir == -1)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (shootDir == 1)
            transform.localScale = new Vector3(1, 1, 1);
        speed = 8;

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
        PoolManager.Instance.SetElement("Fire/����", gameObject);
    }

    #region ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            print("������������");
            FSM fsm = collision.GetComponent<FSM>();
            //TODO:�˺���ֵ����
            fsm.Hit(6);
            fsm.parameter.getHit = true;
        }
    }
    #endregion
}
