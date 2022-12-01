using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//δ�������ʱ�ľ�ֹ״̬
public class IdleState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    public IdleState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Idle");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        //�������Ƿ����Boss��
        if (parameter.player != null)
        {
            int rand = Random.Range(2, 9);
            manager.TransitionState((StateType_Kun)rand);
        }
    }
}

//����״̬
public class DeathState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    public DeathState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Death");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
      
    }
}

