using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �������������ӵ���������......���Ļ��࣬��Ҫʵ�ֵķ�����
        �����ٶ�
        ��ҵĳ�����Ϣ
        �ӳ����ٵ�����(�������� / ��ʱ���Զ�����)
        
        ����ʱ����������ֵ
        ֡����ʱ�����ո����ٶ��ƶ�����
        
        ����������ʱ������˺���
 */

public abstract class FlyableFire : MonoBehaviour
{
    protected float speed;

    //��ȡ��ɫ�������ж��ӵ��˶�����
    protected Transform player;
    protected int shootDir;
    //�Ƿ���Ҫ��ʱ����
    protected bool needDestroy = true;

    protected float DestroyTime = 1f;

    //������ƶ��ٶȸ�ֵ
    protected virtual void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        //�洢�ӵ����������
        shootDir = player.localScale.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(shootDir * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        Invoke("DestroyBullet", DestroyTime);
    }

    //��������ƶ��߼�
    protected virtual void FixedUpdate()
    {
        
    }

    //�ֶ������ӵ�
    protected virtual void DestroyBullet()
    {
        
    }
}
