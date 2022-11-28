using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ����״̬���߼�
 */

//���������״̬
public enum StateType
{
    Idle,Patrol,Chase,React,Attack,Hit,Death
}

//����
[Serializable]
public class Parameter
{
    public int health;
    public int minMoveSpeed;
    public int maxMoveSpeed;
    public int minChaseSpeed;
    public int maxChaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Animator anim;
    //���Ŀ��
    public Transform player;
    //���˹�����ز���
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
    public int damage;

    //�����Ƿ񱻹���
    public bool getHit;
    //���˵���ɫ�����(�ܻ�������)
    public SpriteRenderer shader;

    //������ȴʱ��
    public float currCD = 0;
}


public class FSM : MonoBehaviour
{
    //��ǰ��״̬
    private IState currState;
    //�洢״̬
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    //����
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

        //���ó�ʼ״̬ΪIdle
        TransitionState(StateType.Idle);
    }

    void Update()
    {
        currState.OnUpdate();
        CoolDown();
    }

    //��ȴ�߼�
    private void CoolDown()
    {
        parameter.currCD += Time.deltaTime;
        if (parameter.currCD >= 1.5f)
        {
            parameter.currCD = 1.5f;
        }
    }

    //ת��״̬�߼�
    public void TransitionState(StateType type)
    {
        //ת��״̬֮ǰ��ִ�е�ǰ״̬�˳��߼�
        if (currState != null)
            currState.OnExit();

        //���ĵ�ǰ״̬
        currState = states[type];
        //ִ����״̬�����߼�
        currState.OnEnter();
        
    }

    //�ı���˳���
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

    //���������߼�(����ҵ���)
    public void Hit(int damage)
    {
        parameter.health -= damage;
        print("��ǰ����ʣ��Ѫ�� " + parameter.health);
    }

    #region ������
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

    #region �����¼�
    public void Atk()
    {
        if (parameter.player != null )
        {
            parameter.player.GetComponent<PlayerInteraction>().Wound(parameter.damage);
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
}
