using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//静止状态
public class IdleState_Boss : IState
{
    //状态机
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
        //检测玩家是否进入Boss房
        if (parameter.player != null)
        {
            manager.TransitionState(StateType_Boss.Chase);
        }
    }
}

//追逐状态
public class ChaseState_Boss : IState
{
    //状态机
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
        //检测是否受伤
        if (parameter.getHit)
        {
            parameter.anim.Play("Hit");
            manager.TransitionState(StateType_Boss.Hit);
        }
            
        //实时更新怪物朝向
        if (parameter.player != null)
        {
            manager.FlipTo(parameter.player);
            //怪物追逐玩家
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.player.position, parameter.chaseSpeed * Time.deltaTime);
            //当终极技能计时器达到时，释放大招
            //Debug.Log(parameter.currSkillCD + " " + parameter.skillCD);
            if (parameter.currSkillCD >= parameter.skillCD)
                manager.TransitionState(StateType_Boss.Skill);
            //当与玩家距离过近,切换为Atk1,距离适中切换为Atk2
            else if (Vector3.Distance(manager.transform.position, parameter.player.position) <= parameter.Atk1Range
                && parameter.player.position.y >= manager.transform.position.y
                && parameter.player.position.y < manager.transform.position.y + 1.8)
                manager.TransitionState(StateType_Boss.Atk1);
            else if (Vector3.Distance(manager.transform.position, parameter.player.position) <= parameter.Atk2Range)
                manager.TransitionState(StateType_Boss.Atk2);
        }

        
    }
}

//贤者模式
public class FatigueState_Boss : IState
{
    //状态机
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    //持续时间计数器
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
        //检测是否受伤
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

//近战攻击
public class Atk1State_Boss : IState
{
    //状态机
    private FSM_Boss manager;
    private Parameter_Boss parameter;

    //动画的播放进度
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
        //检测是否受伤(无僵直)
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

//远程攻击
public class Atk2State_Boss : IState
{
    //状态机
    private FSM_Boss manager;
    private Parameter_Boss parameter;
    //动画的播放进度
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
        //检测是否受伤(无僵直)
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

//终极技能
public class SkillState_Boss : IState
{
    //状态机
    private FSM_Boss manager;
    private Parameter_Boss parameter;
    //动画的播放进度
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

        //检测是否受伤
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

//受伤状态
public class HitState_Boss : IState
{
    //状态机
    private FSM_Boss manager;
    private Parameter_Boss parameter;
    //动画的播放进度
    private AnimatorStateInfo info;
    public HitState_Boss(FSM_Boss manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {   //这个僵直在外面有需求再加
        //进入受伤状态时，身体变红
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

//死亡状态
public class DeathState_Boss : IState
{
    //状态机
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
