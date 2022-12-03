using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType_Kun
{
    Idle, Death, Wind, Burning, Ice, Water, Thunder
}

[Serializable]
public class Parameter_Kun
{
    public int health;
    public int maxHealth;
    public Animator anim;
    //玩家目标
    public Transform player;
    //攻击相关参数
    public float Atk1Range;
    public float Atk2Range;
    public int baseDamage;
    //冷却相关
    public float stateCD;
    //是否被攻击
    public bool getHit;
    public SpriteRenderer shader;

    //冷却计时器
    public float currAtkCD;
}

public class FSM_Kun : MonoBehaviour
{
    //当前状态
    private IState currState;
    //存储状态对应关系
    private Dictionary<StateType_Kun, IState> states_kun = new Dictionary<StateType_Kun, IState>();
    //属性
    public Parameter_Kun parameter;

    //Boss是否死亡
    private bool isDie;

    void Start()
    {
        states_kun.Add(StateType_Kun.Idle, new IdleState_Kun(this));
        states_kun.Add(StateType_Kun.Death, new DeathState_Kun(this));
        states_kun.Add(StateType_Kun.Thunder, new ThouderState_Kun(this));
        states_kun.Add(StateType_Kun.Wind, new WindState_Kun(this));
        states_kun.Add(StateType_Kun.Ice, new IceState_Kun(this));
        states_kun.Add(StateType_Kun.Water, new WaterState_Kun(this));
        states_kun.Add(StateType_Kun.Burning, new BurningState_Kun(this));
        //设置初始状态为Idle
        TransitionState(StateType_Kun.Idle);
    }

    void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.N))
        {
            TransitionState(StateType_Kun.Death);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TransitionState(StateType_Kun.Thunder);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            TransitionState(StateType_Kun.Wind);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TransitionState(StateType_Kun.Ice);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            TransitionState(StateType_Kun.Water);
        }


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
        if (parameter.currAtkCD >= parameter.stateCD)
            parameter.currAtkCD = parameter.stateCD;
    }

    //转移状态逻辑
    public void TransitionState(StateType_Kun type)
    {
        //转移状态之前先执行当前状态退出逻辑
        if (currState != null)
            currState.OnExit();

        //更改当前状态
        currState = states_kun[type];
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
    public void KunDeath()
    {
        Destroy(gameObject);
        //生成爆炸
        GameObject obj = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/KunDeath");
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        Destroy(obj, 1.2f);
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
        Gizmos.DrawWireSphere(Vector3.up + transform.position, 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.up + transform.position, 3.5f);

    }
    #endregion
}
