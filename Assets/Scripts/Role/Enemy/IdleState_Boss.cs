using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ֹ״̬
public class IdleState_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    public IdleState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Idle");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        //�������Ƿ����Boss��
        if (parameter.player != null)
        {
            manager.TransitionState(StateType_Boss.Chase);
        }
    }
}

//׷��״̬
public class ChaseState_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    public ChaseState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Chase");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����Ƿ�����
        if (parameter.getHit)
        {
            parameter.anim.Play("Hit");
            manager.TransitionState(StateType_Boss.Hit);
        }
            
        //ʵʱ���¹��ﳯ��
        if (parameter.player != null)
        {
            manager.FlipTo(parameter.player);
            //����׷�����
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.player.position, parameter.chaseSpeed * Time.deltaTime);
            //���ռ����ܼ�ʱ���ﵽʱ���ͷŴ���
            //Debug.Log(parameter.currSkillCD + " " + parameter.skillCD);
            if (parameter.currSkillCD >= parameter.skillCD)
                manager.TransitionState(StateType_Boss.Skill);
            //������Ҿ������,�л�ΪAtk1,���������л�ΪAtk2
            else if (Vector3.Distance(manager.transform.position, parameter.player.position) <= parameter.Atk1Range
                && parameter.player.position.y >= manager.transform.position.y
                && parameter.player.position.y < manager.transform.position.y + 1.8)
                manager.TransitionState(StateType_Boss.Atk1);
            else if (Vector3.Distance(manager.transform.position, parameter.player.position) <= parameter.Atk2Range)
                manager.TransitionState(StateType_Boss.Atk2);
        }

        
    }
}

//����ģʽ
public class FatigueState_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    //����ʱ�������
    private float cd;

    public FatigueState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        cd = 0;
        parameter.anim.Play("Fatigue");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        //����Ƿ�����
        if (parameter.getHit)
        {
            manager.TransitionState(StateType_Boss.Hit);
        }

        cd += Time.deltaTime;
        if (cd >= parameter.fatigueTime)
        {
            manager.TransitionState(StateType_Boss.Chase);
        }
    }
}

//��ս����
public class Atk1State_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    //�����Ĳ��Ž���
    private AnimatorStateInfo info;

    public Atk1State_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        if (parameter.currAtkCD >= parameter.atkCD)
        {
            parameter.anim.Play("Atk1");
            parameter.currAtkCD = 0;
        }
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����Ƿ�����(�޽�ֱ)
        if (parameter.getHit)
        {
            manager.TransitionState(StateType_Boss.Hit);
        }

        info = parameter.anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType_Boss.Chase);
        }
    }
}

//Զ�̹���
public class Atk2State_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;
    //�����Ĳ��Ž���
    private AnimatorStateInfo info;
    public Atk2State_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        if (parameter.currAtkCD >= parameter.atkCD)
        {
            parameter.anim.Play("Atk2");
            parameter.currAtkCD = 0;
        }
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����Ƿ�����(�޽�ֱ)
        if (parameter.getHit)
        {
            manager.TransitionState(StateType_Boss.Hit);
        }

        info = parameter.anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType_Boss.Chase);
        }
    }
}

//�ռ�����
public class SkillState_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;
    //�����Ĳ��Ž���
    private AnimatorStateInfo info;
    public SkillState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
        
    }

    public void OnEnter()
    {
        if (Vector3.Distance(manager.transform.position, parameter.player.position) <= parameter.Atk2Range)
        {
            parameter.anim.Play("Skill");
            parameter.currSkillCD = 0;
        }
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
            manager.TransitionState(StateType_Boss.Hit);
        }

        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType_Boss.Fatigue);
        }
    }
}

//����״̬
public class HitState_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;
    //�����Ĳ��Ž���
    private AnimatorStateInfo info;
    public HitState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {   //�����ֱ�������������ټ�
        //��������״̬ʱ��������
        parameter.shader.color = Color.red;
    }

    public void OnExit()
    {
        parameter.getHit = false;
        parameter.shader.color = Color.white;
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if (parameter.health <= 0)
        {
            manager.TransitionState(StateType_Boss.Death);
        }
        else
        {
            if (info.normalizedTime >= 0.95f)
            {
                parameter.player = GameObject.FindWithTag("Player").transform;
                manager.TransitionState(StateType_Boss.Chase);
            }
        }
    }
}

//����״̬
public class DeathState_Boss : IState
{
    //״̬��
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    public DeathState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Death");
        GameObject.Destroy(manager.gameObject, 5f);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}
