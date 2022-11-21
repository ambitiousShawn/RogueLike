using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : FlyableFire
{
    //�����������ļ�
    WeaponInfo info = DataManager.Instance.weaponInfos[2];
    //����ǿ��
    private float force = 1.2f;


    //������Ƿ��Ѿ�����Ŀ�ĵ�
    private bool isArrived;

    protected override void OnEnable()
    {
        DestroyTime = 5f;
        base.OnEnable();
        isArrived = false;
        speed = info.FlySpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //�ӵ����ƶ�
        if (!isArrived)
            transform.Translate(shootDir * speed * Time.deltaTime, 0, 0);
        else
            Adsorption();
    }

    //�ֶ������ӵ�
    protected override void DestroyBullet()
    {
        if (needDestroy)
        {
            PoolManager.Instance.SetElement("Fire/Wind", gameObject);
        }
    }

    #region ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ӵ���������Թ�������˺����������ӵ������������ɲ�����Ч
        if (collision.CompareTag("Enemy"))
        {
            FSM fsm = collision.GetComponent<FSM>();
            fsm.Hit(info.Damage);
            fsm.parameter.getHit = true;

            //������������ˣ�ֹͣ�˶������ҳ���������Χ�ĵ���
            isArrived = true;
        }
    }
    #endregion

    //���������߼�
    private void Adsorption()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 5f, 1 << LayerMask.NameToLayer("Enemy"));
        
        foreach (Collider2D c in coll)
        {
            c.GetComponent<Rigidbody2D>().AddForce((transform.position - c.transform.position) * force, ForceMode2D.Impulse); 
        }
    }
}
