using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    //���з���
    private Vector3 dir;
    //�����ٶ�
    private float speed = 8 ;
    private Transform currPlayerPos;
    private Transform throwPoint;
    private void OnEnable()
    {
        currPlayerPos = GameObject.Find("Player").transform;
        //Ͷ����
        throwPoint = GameManager.Instance.boss.transform.Find("ThrowPoint");
        transform.position = throwPoint.position;
        dir = (currPlayerPos.position - throwPoint.position).normalized;
        Invoke("delayPut", 3);
    }

    void Update()
    {
        //Ͷ��
        transform.Translate(dir * speed * Time.deltaTime,Space.World);
        //��ת
        transform.Rotate(new Vector3(0, 0, 6),Space.Self);
    }

    private void delayPut()
    {
        PoolManager.Instance.SetElement("Fire/Rock",gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteraction>().Wound(4);
        }
        else if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.name != "Cyclops") 
        {
            collision.gameObject.GetComponent<FSM>().Hit(4);
        }
    }
}
