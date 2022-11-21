using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    //后期读表填值，目前先给默认值
    private float speed;

    //获取角色的面向，判断子弹运动方向
    private Transform player;
    private int shootDir;

    private bool needDestroy = true;

    void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        //存储子弹的射击方向
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
        //子弹的移动
        transform.Translate(shootDir * speed * Time.deltaTime, 0, 0,Space.World);
        transform.Rotate(new Vector3(0, 0, 60),Space.Self);
    }

    //手动销毁子弹
    private void DestroyBullet()
    {
        if (needDestroy)
            PoolManager.Instance.SetElement("Fire/Gift", gameObject);
    }

    #region 触发器相关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
            FSM fsm = collision.GetComponent<FSM>();
            //TODO:伤害数值读表
            fsm.Hit(2);
            fsm.parameter.getHit = true;

            PoolManager.Instance.SetElement("Fire/Gift", gameObject);
            needDestroy = false;
        }
    }
    #endregion
}
