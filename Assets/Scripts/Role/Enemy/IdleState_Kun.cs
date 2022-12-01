using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//未发现玩家时的静止状态
public class IdleState_Kun : IState
{
    //状态机
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
        //检测玩家是否进入Boss房
        if (parameter.player != null)
        {
            int rand = Random.Range(2, 9);
            manager.TransitionState((StateType_Kun)rand);
        }
    }
}

//死亡状态
public class DeathState_Kun : IState
{
    //状态机
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

