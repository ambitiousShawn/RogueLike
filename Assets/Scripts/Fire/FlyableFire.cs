using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    飞行类武器（子弹，剑气等......）的基类，需要实现的方法：
        飞行速度
        玩家的朝向信息
        延迟销毁的条件(碰到敌人 / 随时间自动销毁)
        
        开启时，给变量赋值
        帧更新时，按照给定速度移动物体
        
        触碰到敌人时，造成伤害。
 */

public abstract class FlyableFire : MonoBehaviour
{
    protected float speed;

    //获取角色的面向，判断子弹运动方向
    protected Transform player;
    protected int shootDir;
    //是否需要延时销毁
    protected bool needDestroy = true;

    protected float DestroyTime = 1f;

    //子类给移动速度赋值
    protected virtual void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        //存储子弹的射击方向
        shootDir = player.localScale.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(shootDir * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        Invoke("DestroyBullet", DestroyTime);
    }

    //飞行物的移动逻辑
    protected virtual void FixedUpdate()
    {
        
    }

    //手动销毁子弹
    protected virtual void DestroyBullet()
    {
        
    }
}
