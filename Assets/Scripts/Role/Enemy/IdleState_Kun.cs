using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//δ�������ʱ�ľ�ֹ״̬
public class IdleState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //��ʱ��
    private float timer;

    public IdleState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        //����ʱ����ʱ����Ϊ0
        parameter.anim.Play("Kun_Idle");
        timer = 0;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        //����
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        timer += Time.deltaTime;
        //�������Ƿ����Boss��
        if (parameter.player != null)
        {
            if (timer >= 2)
            {
                //���빥����ȴ
                //���������ʱ������л�״̬
                int rand = Random.Range(2, 7);

                manager.TransitionState((StateType_Kun)rand);
                timer = 0;
            }
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

//�׵���̬
public class ThouderState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //���׹���CD
    private float skillCD = 1;
    private float atkTimer;
    //״̬������ʱ��
    private float stateTimer;

    public ThouderState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //��ʼ��ʱ��
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //���Խ�������(�����ο��Կ���Э��)
            atkTimer = 0;
            MonoManager.Instance.StartCoroutine(IE_Thouder());
        }

        if (stateTimer >= parameter.stateCD)
        {
            //���л�״̬��
            manager.TransitionState(StateType_Kun.Idle);
        }
    }

    IEnumerator IE_Thouder()
    {
        float randX, randY;
        for (int i = 0;i < 3; i++)
        {
            //���������׵�λ��
            randX = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            randY = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            //��������
            GameObject obj = PoolManager.Instance.GetElement("Fire/Kun/Thouder");
            Vector3 self = manager.transform.position;
            obj.transform.position = new Vector3(self.x + randX, self.y + randY, 0);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
}

//�����̬
public class WindState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //���׹���CD
    private float skillCD = 1;
    private float atkTimer;
    //״̬������ʱ��
    private float stateTimer;

    public WindState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //��ʼ��ʱ��
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //�ٻ��ͷ�
            atkTimer = 0;
            GameObject obj = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/Wind");
            obj.transform.position = manager.transform.position;
            GameObject.Destroy(obj.gameObject, 1f);
            //ÿ�λָ�20������ֵ
            parameter.health += 15;
        }

        if (stateTimer >= parameter.stateCD)
        {
            //���л�״̬��
            manager.TransitionState(StateType_Kun.Idle);
        }
    }
}

//������̬
public class IceState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //��������CD
    private float skillCD = 3;
    private float atkTimer;
    //״̬������ʱ��
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
        //��ʼ��ʱ��
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {
        //״̬�����ָ���ҵ��ƶ��ٶ�
        if (parameter.player != null)
            parameter.player.GetComponent<Player>().speed = parameter.player.GetComponent<Player>().baseSpeed;
        GameObject.Destroy(obj.gameObject);
    }

    public void OnUpdate()
    {
        //����
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);
        //��˪����Buffȫ�ּ���
        if (parameter.player != null)
            parameter.player.GetComponent<Player>().speed = 2;

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //�ٻ���������
            atkTimer = 0;
            GameObject ice = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/Ice2");
            if (parameter.player != null)
                ice.transform.position = parameter.player.transform.position;
            else
                //������
                ice.transform.position = manager.transform.position;
            GameObject.Destroy(ice.gameObject, 1.5f);
        }

        if (stateTimer >= parameter.stateCD)
        {
            //���л�״̬��
            manager.TransitionState(StateType_Kun.Idle);
        }
    }
}

//ˮ��״̬
public class WaterState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //���׹���CD
    private float skillCD = 0.9f;
    private float atkTimer;
    //״̬������ʱ��
    private float stateTimer;

    public WaterState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //��ʼ��ʱ��
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //���Խ�������(�����ο��Կ���Э��)
            atkTimer = 0;
            MonoManager.Instance.StartCoroutine(IE_Water());
        }

        if (stateTimer >= parameter.stateCD)
        {
            //���л�״̬��
            manager.TransitionState(StateType_Kun.Idle);
        }
    }

    IEnumerator IE_Water()
    {
        float randX, randY;
        for (int i = 0; i < 3; i++)
        {
            //������ˮ����λ��
            randX = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            randY = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            //��������
            GameObject obj = PoolManager.Instance.GetElement("Fire/Kun/Water");
            Vector3 self = manager.transform.position;
            obj.transform.position = new Vector3(self.x + randX, self.y + randY, 0);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}

//����״̬
public class BurningState_Kun : IState
{
    //״̬��
    private FSM_Kun manager;
    private Parameter_Kun parameter;

    //���׹���CD
    private float skillCD = 0.9f;
    private float atkTimer;
    //״̬������ʱ��
    private float stateTimer;

    public BurningState_Kun(FSM_Kun manager)
    {
        this.manager = manager;
        parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Kun_Atk");
        //��ʼ��ʱ��
        atkTimer = 0.5f;
        stateTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //����
        if (parameter.health <= 0)
            manager.TransitionState(StateType_Kun.Death);

        atkTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (atkTimer >= skillCD)
        {
            //���Խ�������(�����ο��Կ���Э��)
            atkTimer = 0;
            MonoManager.Instance.StartCoroutine(IE_Burning());
        }

        if (stateTimer >= parameter.stateCD)
        {
            //���л�״̬��
            manager.TransitionState(StateType_Kun.Idle);
        }
    }

    IEnumerator IE_Burning()
    {
        float randX, randY;
        for (int i = 0; i < 3; i++)
        {
            //������ˮ����λ��
            randX = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            randY = Random.Range(-parameter.Atk1Range, parameter.Atk1Range);
            //��������
            GameObject obj = PoolManager.Instance.GetElement("Fire/Kun/Burning");
            Vector3 self = manager.transform.position;
            obj.transform.position = new Vector3(self.x + randX, self.y + randY, 0);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}

