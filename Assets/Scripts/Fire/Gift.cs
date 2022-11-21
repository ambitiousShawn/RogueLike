using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    //���ڶ�����ֵ��Ŀǰ�ȸ�Ĭ��ֵ
    private float speed;

    //��ȡ��ɫ�������ж��ӵ��˶�����
    private Transform player;
    private int shootDir;

    private bool needDestroy = true;

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
        transform.Translate(shootDir * speed * Time.deltaTime, 0, 0,Space.World);
        transform.Rotate(new Vector3(0, 0, 60),Space.Self);
    }

    //�ֶ������ӵ�
    private void DestroyBullet()
    {
        if (needDestroy)
            PoolManager.Instance.SetElement("Fire/Gift", gameObject);
    }

    #region ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
            FSM fsm = collision.GetComponent<FSM>();
            //TODO:�˺���ֵ����
            fsm.Hit(2);
            fsm.parameter.getHit = true;

            PoolManager.Instance.SetElement("Fire/Gift", gameObject);
            needDestroy = false;
        }
    }
    #endregion
}
