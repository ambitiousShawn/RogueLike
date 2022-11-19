using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//静止状态
public class IdleState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    //计时器
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

        //检测是否受伤
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //检测是否发现玩家
        if (parameter.player != null)
        {
            manager.TransitionState(StateType.React);
        }

        //静止时间到时，转换为巡逻状态
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }
}

//巡逻状态
public class PatrolState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    //巡逻位置
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
        //巡逻结束时，修改巡逻点下标
        patrolPosition++;
        //防止越界
        if (patrolPosition >= parameter.patrolPoints.Length)
            patrolPosition = 0;
    }

    public void OnUpdate()
    {
        //巡逻状态，改变怪物朝向
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);
        //让怪物移动到目标点
        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);

        //检测是否受伤
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //检测是否发现玩家
        if (parameter.player != null)
        {
            manager.TransitionState(StateType.React);
        }

        //如果走到巡逻点，切换到静止状态
        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) < 0.1f)
            manager.TransitionState(StateType.Idle);
    }
}

//追赶状态
public class ChaseState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //播放追击动画
        parameter.anim.Play("Walk");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //追击时敌人朝向玩家
        manager.FlipTo(parameter.player);

        //检测是否受伤
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //敌人追击
        if (parameter.player != null)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.player.position,
                parameter.chaseSpeed * Time.deltaTime);

        //当超出范围时,切换到静止状态
        else
        {
            manager.TransitionState(StateType.Idle);
        }

        //如果在攻击距离检测到玩家，切换至攻击状态
        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            manager.TransitionState(StateType.Attack);
        }
    }
}

//反应状态
public class ReactState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    //动画的播放进度
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
        
        //检测是否受伤
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Hit);
        }

        //当反应快播完时，切换至追击状态
        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }
}

//攻击状态
public class AttackState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    //动画的播放进度
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

        //检测是否受伤
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

//受击状态
public class HitState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    //动画的播放进度
    private AnimatorStateInfo info;

    public HitState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Hit");
        //进入受伤状态时，身体变红
        parameter.shader.color = Color.red;
    }

    public void OnExit()
    {
        parameter.getHit = false;
        //结束受伤状态时，身体颜色恢复
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

//死亡状态
public class DeathState : IState
{
    //状态机
    private FSM manager;
    private Parameter parameter;

    //动画的播放进度
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


