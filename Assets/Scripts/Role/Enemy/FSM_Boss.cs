using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Boss行为有限状态机
 */

//Boss的所有状态:
/*
    切换关系：
        1.默认播放Idle动画，当玩家走进Boss房间，切换为Chase状态
        2.当怪物与玩家距离达到远程攻击距离时，切换Atk2进行攻击
        3.当怪物与玩家距离达到近战攻击距离时，切换Atk1进行攻击（远近战共用计时器）
        4.独立采用技能计时器，当计时器时间到达指定时间时，Boss释放大招。
        5.Boss释放完一次大招时，会立刻进入贤者模式Fatigue。
        6.当处于追逐状态时，受击会优先切换为Hit状态。
        7.当处于Hit状态并且生命值降低为0时，切换为Death状态。
 */
public enum StateType_Boss
{
    Idle,Chase,Fatigue,Atk1,Atk2,Skill,Hit,Death
}

[Serializable]
public class Parameter_Boss
{
    public int health;
    public int maxHealth;
    public int chaseSpeed;
    public Animator anim;
    //玩家目标
    public Transform player;
    //攻击相关参数
    public float Atk1Range;
    public float Atk2Range;
    public int baseDamage;
    //冷却相关
    public float atkCD;
    public float skillCD;
    public float fatigueTime;
    //是否被攻击
    public bool getHit;
    //受击身体变红
    public SpriteRenderer shader;

    //冷却计时器
    public float currAtkCD;
    public float currSkillCD;
}

public class FSM_Boss : MonoBehaviour
{
    //当前状态
    private IState currState;
    //存储状态对应关系
    private Dictionary<StateType_Boss, IState> states_boss = new Dictionary<StateType_Boss, IState>();
    //属性
    public Parameter_Boss parameter;

    //独眼巨人特有
    public Transform laserPoint;
    public Transform ThrowPoint;

    //Boss是否死亡
    private bool isDie;
    void Start()
    {
        states_boss.Add(StateType_Boss.Idle, new IdleState_Boss(this));
        states_boss.Add(StateType_Boss.Chase, new ChaseState_Boss(this));
        states_boss.Add(StateType_Boss.Fatigue, new FatigueState_Boss(this));
        states_boss.Add(StateType_Boss.Atk1, new Atk1State_Boss(this));
        states_boss.Add(StateType_Boss.Atk2, new Atk2State_Boss(this));
        states_boss.Add(StateType_Boss.Skill, new SkillState_Boss(this));
        states_boss.Add(StateType_Boss.Hit, new HitState_Boss(this));
        states_boss.Add(StateType_Boss.Death, new DeathState_Boss(this));

        //设置初始状态为Idle
        TransitionState(StateType_Boss.Idle);
        //test
        //GameManager.Instance.isArrive = true;
    }

    // Update is called once per frame
    void Update()
    {
        currState.OnUpdate();
        CoolDown();
        if (LevelManager.Instance.isArrive)
        {
            if (parameter.health <= 0 && !isDie)
            {
                //如果Boss死亡，产生传送门，可让玩家传送至下一关
                GameObject obj = ResourcesManager.Instance.Load<GameObject>("Room/Portal");
                obj.transform.position = transform.position;
                isDie = true;
            }
            else
            {
                parameter.player = LevelManager.Instance.playerObj.transform;
                LevelManager.Instance.gamePanel.UpdateBossBar(true, parameter.health, parameter.maxHealth);
                
            }
        }
    }

    private void OnDestroy()
    {
        LevelManager.Instance.gamePanel.UpdateBossBar(false);
    }

    //冷却逻辑
    private void CoolDown()
    {
        parameter.currAtkCD += Time.deltaTime;
        parameter.currSkillCD += Time.deltaTime;
        GetComponent<SpriteRenderer>().color = new Color(1, (parameter.skillCD - parameter.currSkillCD) / parameter.skillCD * 2, (parameter.skillCD - parameter.currSkillCD) / parameter.skillCD*2, 1);
        if (parameter.currAtkCD >= parameter.atkCD)
            parameter.currAtkCD = parameter.atkCD;
        if (parameter.currSkillCD >= parameter.skillCD)
            parameter.currSkillCD = parameter.skillCD;
        
    }

    //转移状态逻辑
    public void TransitionState(StateType_Boss type)
    {
        //转移状态之前先执行当前状态退出逻辑
        if (currState != null)
            currState.OnExit();

        //更改当前状态
        currState = states_boss[type];
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

    //敌人受伤逻辑(给玩家调用)
    public void Hit(int damage)
    {
        parameter.health -= damage;
        print("当前Boss剩余血量 " + parameter.health);
    }

    #region 动画事件
    //独眼巨人激光眼
    public void Laser()
    {
        GameObject laser = PoolManager.Instance.GetElement("Fire/Laser");
        laser.transform.position = laserPoint.position;
        if (transform.localScale.x > 0)
            laser.transform.localScale = new Vector3(1, 1, 1);
        else
            laser.transform.localScale = new Vector3(-1, 1, 1);
    }

    //独眼巨人投掷
    public void Throw()
    {
        PoolManager.Instance.GetElement("Fire/Rock");
    }

    //烈火审判释放
    public void FireTrial()
    {
        GameObject fireTrial = PoolManager.Instance.GetElement("Fire/FireTrial");
        fireTrial.transform.position = transform.position + Vector3.up;
    }
    #endregion

    #region 画线逻辑
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, parameter.Atk1Range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parameter.Atk2Range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.up+transform.position, 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.up+transform.position, 3.5f);

    }
    #endregion
}
