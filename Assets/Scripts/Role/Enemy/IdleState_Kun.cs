using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//未发现玩家时的静止状态
public class IdleState_Kun : IState
{
    //状态机
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //计时器
    private float timer;

    public IdleState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //进入时将计时器记为0
        parameter.anim.Play("Kun_Idle");
        timer = 0;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        //死亡
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        timer += Time.deltaTime;
        //检测玩家是否进入Boss房
        if (parameter.player != null)
        {
            if (timer >= 2)
            {
                //两秒攻击冷却
                //当发现玩家时，随机切换状态
                int rand = Random.Range(2, 7);

                manager.TransitionState((StateType_Kun)rand);
                timer = 0;
            }
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

//雷电形态
public class ThouderState_Kun : IState
{
    //状态机
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //落雷攻击CD
    private float skillCD = 1;
    private float atkTimer;
    //状态持续计时器
    private float stateTimer;

    public ThouderState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //初始计时器
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //死亡
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //可以降下落雷(分批次可以开启协程)
            atkTimer = 0;
            MonoManager.Instance.StartCoroutine(IE_Thouder());
        }

        if (stateTimer >= parameter.stateCD)
        {
            //该切换状态了
            manager.TransitionState(StateType_Kun.Idle);
        }
    }

    IEnumerator IE_Thouder()
    {
        float randX, randY;
        for (int i = 0;i < 3; i++)
        {
            //随机算出落雷的位置
            randX = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            randY = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            //降下落雷
            GameObject obj = PoolManager.Instance.GetElement("Fire/Kun/Thouder");
            Vector3 self = manager.transform.position;
            obj.transform.position = new Vector3(self.x + randX, self.y + randY, 0);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
}

//狂风形态
public class WindState_Kun : IState
{
    //状态机
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //落雷攻击CD
    private float skillCD = 1;
    private float atkTimer;
    //状态持续计时器
    private float stateTimer;

    public WindState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //初始计时器
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //死亡
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //召唤和风
            atkTimer = 0;
            GameObject obj = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/Wind");
            obj.transform.position = manager.transform.position;
            GameObject.Destroy(obj.gameObject, 1f);
            //每次恢复20点生命值
            parameter.health += 15;
        }

        if (stateTimer >= parameter.stateCD)
        {
            //该切换状态了
            manager.TransitionState(StateType_Kun.Idle);
        }
    }
}

//冰封形态
public class IceState_Kun : IState
{
    //状态机
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //冰川攻击CD
    private float skillCD = 3;
    private float atkTimer;
    //状态持续计时器
    private float stateTimer;

    private GameObject obj;
    public IceState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        obj = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/Ice");
        obj.transform.position = manager.transform.position;
        //初始计时器
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {
        //状态结束恢复玩家的移动速度
        if (parameter.player != null)
            parameter.player.GetComponent<Player>().speed = parameter.player.GetComponent<Player>().baseSpeed;
        GameObject.Destroy(obj.gameObject);
    }

    public void OnUpdate()
    {
        //死亡
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);
        //冰霜领域Buff全局减速
        if (parameter.player != null)
            parameter.player.GetComponent<Player>().speed = 2;

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //召唤寒冰枷锁
            atkTimer = 0;
            GameObject ice = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/Ice2");
            if (parameter.player != null)
                ice.transform.position = parameter.player.transform.position;
            else
                //测试用
                ice.transform.position = manager.transform.position;
            GameObject.Destroy(ice.gameObject, 1.5f);
        }

        if (stateTimer >= parameter.stateCD)
        {
            //该切换状态了
            manager.TransitionState(StateType_Kun.Idle);
        }
    }
}

//水流状态
public class WaterState_Kun : IState
{
    //状态机
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //落雷攻击CD
    private float skillCD = 0.9f;
    private float atkTimer;
    //状态持续计时器
    private float stateTimer;

    public WaterState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //初始计时器
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //死亡
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //可以降下落雷(分批次可以开启协程)
            atkTimer = 0;
            MonoManager.Instance.StartCoroutine(IE_Water());
        }

        if (stateTimer >= parameter.stateCD)
        {
            //该切换状态了
            manager.TransitionState(StateType_Kun.Idle);
        }
    }

    IEnumerator IE_Water()
    {
        float randX, randY;
        for (int i = 0; i < 3; i++)
        {
            //随机算出水流的位置
            randX = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            randY = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            //降下落雷
            GameObject obj = PoolManager.Instance.GetElement("Fire/Kun/Water");
            Vector3 self = manager.transform.position;
            obj.transform.position = new Vector3(self.x + randX, self.y + randY, 0);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}

//烈焰状态
public class BurningState_Kun : IState
{
    //状态机
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //落雷攻击CD
    private float skillCD = 0.9f;
    private float atkTimer;
    //状态持续计时器
    private float stateTimer;

    public BurningState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //初始计时器
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //死亡
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //可以降下落雷(分批次可以开启协程)
            atkTimer = 0;
            MonoManager.Instance.StartCoroutine(IE_Burning());
        }

        if (stateTimer >= parameter.stateCD)
        {
            //该切换状态了
            manager.TransitionState(StateType_Kun.Idle);
        }
    }

    IEnumerator IE_Burning()
    {
        float randX, randY;
        for (int i = 0; i < 3; i++)
        {
            //随机算出水流的位置
            randX = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            randY = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            //降下落雷
            GameObject obj = PoolManager.Instance.GetElement("Fire/Kun/Burning");
            Vector3 self = manager.transform.position;
            obj.transform.position = new Vector3(self.x + randX, self.y + randY, 0);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}

