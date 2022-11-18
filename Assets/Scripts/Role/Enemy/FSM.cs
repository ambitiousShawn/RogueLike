using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    有限状态机逻辑
 */

//怪物的所有状态
public enum StateType
{
    Idle,Patrol,Chase,React,Attack,Hit,Death
}

//参数
[Serializable]
public class Parameter
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Animator anim;
    //玩家目标
    public Transform player;
    //敌人攻击相关参数
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;

    //敌人是否被攻击
    public bool getHit;
}


public class FSM : MonoBehaviour
{
    //当前的状态
    private IState currState;
    //存储状态
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    //属性
    public Parameter parameter;

    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        //设置初始状态为Idle
        TransitionState(StateType.Idle);
    }

    void Update()
    {
        currState.OnUpdate();

        //TEST:模拟受伤
        if (Input.GetKeyDown(KeyCode.Space))
        {
            parameter.getHit = true;
        }
    }

    //转移状态逻辑
    public void TransitionState(StateType type)
    {
        //转移状态之前先执行当前状态退出逻辑
        if (currState != null)
            currState.OnExit();

        //更改当前状态
        currState = states[type];
        //执行新状态进入逻辑
        currState.OnEnter();
        
    }

    //改变敌人朝向
    public void FlipTo(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
                transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            else if (transform.position.x < target.position.x)
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    #region 触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parameter.player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parameter.player = null;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
}
