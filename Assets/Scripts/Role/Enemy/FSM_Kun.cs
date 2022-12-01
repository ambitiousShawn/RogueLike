using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType_Kun
{
    Idle, Death, Wind, Fire, Ice, Water, Thunder, Rock, Grass
}

[Serializable]
public class Parameter_Kun
{
    public int health;
    public int maxHealth;
    public Animator anim;
    //���Ŀ��
    public Transform player;
    //������ز���
    public float Atk1Range;
    public float Atk2Range;
    public int baseDamage;
    //��ȴ���
    public float atkCD;
    //�Ƿ񱻹���
    public bool getHit;
    public SpriteRenderer shader;

    //��ȴ��ʱ��
    public float currAtkCD;
}

public class FSM_Kun : MonoBehaviour
{
    //��ǰ״̬
    private IState currState;
    //�洢״̬��Ӧ��ϵ
    private Dictionary<StateType_Kun, IState> states_kun = new Dictionary<StateType_Kun, IState>();
    //����
    public Parameter_Kun parameter;

    //Boss�Ƿ�����
    private bool isDie;

    void Start()
    {
        states_kun.Add(StateType_Kun.Idle, new IdleState_Kun(this));
        states_kun.Add(StateType_Kun.Death, new DeathState_Kun(this));

        //���ó�ʼ״̬ΪIdle
        TransitionState(StateType_Kun.Idle);
    }

    void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.N))
        {
            TransitionState(StateType_Kun.Death);
        }
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
        if (parameter.currAtkCD >= parameter.atkCD)
            parameter.currAtkCD = parameter.atkCD;
    }

    //ת��״̬�߼�
    public void TransitionState(StateType_Kun type)
    {
        //ת��״̬֮ǰ��ִ�е�ǰ״̬�˳��߼�
        if (currState != null)
            currState.OnExit();

        //���ĵ�ǰ״̬
        currState = states_kun[type];
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
    public void KunDeath()
    {
        Destroy(gameObject);
        //���ɱ�ը
        GameObject obj = ResourcesManager.Instance.Load<GameObject>("Fire/Kun/KunDeath");
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        Destroy(obj, 1f);
    }
    #endregion
}
