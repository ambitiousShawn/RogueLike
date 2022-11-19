using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ֹ״̬
public class IdleState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    //��ʱ��
    private float timer;

    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Idle");
    }

    public void OnExit()
    {
        timer = 0;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        //����Ƿ�����
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //����Ƿ������
        if (parameter.player != null)
        {
            manager.TransitionState(StateType.React);
        }

        //��ֹʱ�䵽ʱ��ת��ΪѲ��״̬
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }
}

//Ѳ��״̬
public class PatrolState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    //Ѳ��λ��
    private int patrolPosition;

    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Walk");
    }

    public void OnExit()
    {
        //Ѳ�߽���ʱ���޸�Ѳ�ߵ��±�
        patrolPosition++;
        //��ֹԽ��
        if (patrolPosition >= parameter.patrolPoints.Length)
            patrolPosition = 0;
    }

    public void OnUpdate()
    {
        //Ѳ��״̬���ı���ﳯ��
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);
        //�ù����ƶ���Ŀ���
        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);

        //����Ƿ�����
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //����Ƿ������
        if (parameter.player != null)
        {
            manager.TransitionState(StateType.React);
        }

        //����ߵ�Ѳ�ߵ㣬�л�����ֹ״̬
        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) < 0.1f)
            manager.TransitionState(StateType.Idle);
    }
}

//׷��״̬
public class ChaseState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //����׷������
        parameter.anim.Play("Walk");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //׷��ʱ���˳������
        manager.FlipTo(parameter.player);

        //����Ƿ�����
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //����׷��
        if (parameter.player != null)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.player.position,
                parameter.chaseSpeed * Time.deltaTime);

        //��������Χʱ,�л�����ֹ״̬
        else
        {
            manager.TransitionState(StateType.Idle);
        }

        //����ڹ��������⵽��ң��л�������״̬
        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            manager.TransitionState(StateType.Attack);
        }
    }
}

//��Ӧ״̬
public class ReactState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    //�����Ĳ��Ž���
    private AnimatorStateInfo info;

    public ReactState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("React");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);
        
        //����Ƿ�����
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //����Ӧ�첥��ʱ���л���׷��״̬
        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }
}

//����״̬
public class AttackState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    //�����Ĳ��Ž���
    private AnimatorStateInfo info;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Atk");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        //����Ƿ�����
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }
}

//�ܻ�״̬
public class HitState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    //�����Ĳ��Ž���
    private AnimatorStateInfo info;

    public HitState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Hit");
        //��������״̬ʱ��������
        parameter.shader.color = Color.red;
    }

    public void OnExit()
    {
        parameter.getHit = false;
        //��������״̬ʱ��������ɫ�ָ�
        parameter.shader.color = Color.white ;
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if (parameter.health <= 0)
        {
            manager.TransitionState(StateType.Death);
        }
        else
        {
            if (info.normalizedTime >= 0.95f)
            {
                parameter.player = GameObject.FindWithTag("Player").transform;
                manager.TransitionState(StateType.Chase);
            }
        }
    }
}

//����״̬
public class DeathState : IState
{
    //״̬��
    private FSM manager;
    private Parameter parameter;

    //�����Ĳ��Ž���
    private AnimatorStateInfo info;

    public DeathState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Death");
        GameObject.Destroy(manager.gameObject,5f);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        
    }
}


