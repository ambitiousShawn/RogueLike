using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Boss��Ϊ����״̬��
 */

//Boss������״̬:
/*
    �л���ϵ��
        1.Ĭ�ϲ���Idle������������߽�Boss���䣬�л�ΪChase״̬
        2.����������Ҿ���ﵽԶ�̹�������ʱ���л�Atk2���й���
        3.����������Ҿ���ﵽ��ս��������ʱ���л�Atk1���й�����Զ��ս���ü�ʱ����
        4.�������ü��ܼ�ʱ��������ʱ��ʱ�䵽��ָ��ʱ��ʱ��Boss�ͷŴ��С�
        5.Boss�ͷ���һ�δ���ʱ�������̽�������ģʽFatigue��
        6.������׷��״̬ʱ���ܻ��������л�ΪHit״̬��
        7.������Hit״̬��������ֵ����Ϊ0ʱ���л�ΪDeath״̬��
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
    //���Ŀ��
    public Transform player;
    //������ز���
    public float Atk1Range;
    public float Atk2Range;
    public int baseDamage;
    //��ȴ���
    public float atkCD;
    public float skillCD;
    public float fatigueTime;
    //�Ƿ񱻹���
    public bool getHit;
    //�ܻ�������
    public SpriteRenderer shader;

    //��ȴ��ʱ��
    public float currAtkCD;
    public float currSkillCD;
}

public class FSM_Boss : MonoBehaviour
{
    //��ǰ״̬
    private IState currState;
    //�洢״̬��Ӧ��ϵ
    private Dictionary<StateType_Boss, IState> states_boss = new Dictionary<StateType_Boss, IState>();
    //����
    public Parameter_Boss parameter;

    //���۾�������
    public Transform laserPoint;
    public Transform ThrowPoint;

    //Boss�Ƿ�����
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

        //���ó�ʼ״̬ΪIdle
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
                //���Boss���������������ţ�������Ҵ�������һ��
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

    //��ȴ�߼�
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

    //ת��״̬�߼�
    public void TransitionState(StateType_Boss type)
    {
        //ת��״̬֮ǰ��ִ�е�ǰ״̬�˳��߼�
        if (currState != null)
            currState.OnExit();

        //���ĵ�ǰ״̬
        currState = states_boss[type];
        //ִ����״̬�����߼�
        currState.OnEnter();

    }

    //�ı���˳���
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

    //���������߼�(����ҵ���)
    public void Hit(int damage)
    {
        parameter.health -= damage;
        print("��ǰBossʣ��Ѫ�� " + parameter.health);
    }

    #region �����¼�
    //���۾��˼�����
    public void Laser()
    {
        GameObject laser = PoolManager.Instance.GetElement("Fire/Laser");
        laser.transform.position = laserPoint.position;
        if (transform.localScale.x > 0)
            laser.transform.localScale = new Vector3(1, 1, 1);
        else
            laser.transform.localScale = new Vector3(-1, 1, 1);
    }

    //���۾���Ͷ��
    public void Throw()
    {
        PoolManager.Instance.GetElement("Fire/Rock");
    }

    //�һ������ͷ�
    public void FireTrial()
    {
        GameObject fireTrial = PoolManager.Instance.GetElement("Fire/FireTrial");
        fireTrial.transform.position = transform.position + Vector3.up;
    }
    #endregion

    #region �����߼�
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
