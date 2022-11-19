using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�õ�����ƫ����
    private float XOffset, YOffset;
    //�������
    private Animator anim;
    //����UI
    private GamePanel gamePanel;
    //���������
    private Transform firePoint;


    #region ��ɫ��ֵ(����ֵ)
    //������ȴ
    public float atkCD = 0.4f;
    private float currCD;

    //�ػ�����ʱ��
    public float HitTimer = 1;
    private float currTimer = 0;
    private bool Jtrigger = false;

    //��ս�����Χ
    private Transform atk1Point;
    private float atk1Range;
    //��ս�ػ���Χ
    private float atk2Range;
    #endregion

    private void Awake()
    {
        InputManager.Instance.SwitchState(true);
        EventManager.Instance.AddEventListener<KeyCode>("KeyDown", CheckKeyDown);
        EventManager.Instance.AddEventListener<KeyCode>("KeyUp", CheckKeyUp);

        //Test:
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel");
    }

    void Start()
    {
        //�����ֵ
        anim = GetComponent<Animator>();
        gamePanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();

        XOffset = 16;
        YOffset = 12;
        
        currCD = atkCD;

        //�ҵ������
        firePoint = transform.Find("firePoint");
        Init();
    }

    //�������ɫ��ֵ
    void Init()
    {
        atk1Point = transform.Find("Atk1Range");
        //Test:����
        atk1Range = 0.5f;
        atk2Range = 0.7f;
    }

    void Update()
    {
        CoolDown();
    }

    //������ȴʱ�䷽��
    private void CoolDown()
    {
        currCD += Time.deltaTime;
        if (currCD >= 1)
        {
            currCD = 1;
        }

        //�ػ���������
        if (Jtrigger)
            currTimer += Time.deltaTime;
        else
            currTimer = 0;

        if (currTimer >= 1)
            anim.SetBool("isCombo", true);
        //��������ʱ��UI
        gamePanel.UpdateBarState(currTimer , HitTimer);
    }

    //�����̰���
    private void CheckKeyDown(KeyCode keycode)
    {
        if (keycode == KeyCode.M)
        {
            if (!UIManager.Instance.panelDic.ContainsKey("MiniMapPanel"))
                UIManager.Instance.ShowPanel<BasePanel>("MiniMapPanel");
            else
                UIManager.Instance.HidePanel("MiniMapPanel");
        }

        //��ɫ����
        if (keycode == KeyCode.J && currCD >= atkCD)
        {
            
            currCD = 0;
            anim.SetTrigger("Atk1");
            //��ʾ������
            gamePanel.SwitchBarState(true);
            Jtrigger = true;
        }

        if (keycode == KeyCode.K && currCD >= atkCD)
        {
            currCD = 0;
            anim.SetTrigger("Shoot");
        }

    }

    //������̧��
    private void CheckKeyUp(KeyCode keycode)
    {
        if (keycode == KeyCode.J)
        {
            anim.SetBool("isCombo", false);
            //���ؽ�����
            gamePanel.SwitchBarState(false);
            Jtrigger = false;
        }
    }

    #region ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(GameManager.Instance.isSucceed);
        if (collision.tag.StartsWith("Door_") && GameManager.Instance.isSucceed)
        {
            
            //�õ���ǰ��������
            Vector3 currRoom = collision.transform.parent.position;
            //��÷���ƫ����
            if (collision.tag == "Door_right")
            {
                CameraMove.Instance.ChangeTargetPos(new Vector3(currRoom.x + XOffset, currRoom.y, -10));
                //�ƶ����
                transform.position = new Vector3(transform.position.x + 6, transform.position.y, transform.position.z);
            }
            else if (collision.tag == "Door_left")
            {
                CameraMove.Instance.ChangeTargetPos(new Vector3(currRoom.x - XOffset, currRoom.y, -10));
                transform.position = new Vector3(transform.position.x - 6, transform.position.y, transform.position.z);
            }
            else if (collision.tag == "Door_up")
            {
                CameraMove.Instance.ChangeTargetPos(new Vector3(currRoom.x, currRoom.y + YOffset, -10));
                transform.position = new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);
            }
            else if (collision.tag == "Door_down")
            {
                CameraMove.Instance.ChangeTargetPos(new Vector3(currRoom.x, currRoom.y - YOffset, -10));
                transform.position = new Vector3(transform.position.x, transform.position.y - 6, transform.position.z);
            }

        }
    }
    #endregion


    #region �����¼�
    public void Fire()
    {
        //����ʱ�������ӵ�(������Ϊ��������)
        /*ResourcesManager.Instance.LoadAsync<GameObject>("Fire/�����ӵ�",(obj) => 
        {
            //������������λ�ã���ת�Լ�������
            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.identity;
        });*/

        PoolManager.Instance.GetElement("Fire/�����ӵ�", firePoint);
    }

    public void Atk1()
    {
        //���Ž�ս�������ʱ����ⷶΧ���Ƿ���ڵ��˱�ǩ����������õ��������߼�
        Collider2D[] coll = Physics2D.OverlapCircleAll(atk1Point.position, atk1Range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D c in coll)
        {
            //Bug��¼������������������ײ������Ҫ�ų����Ǵ��������Ǹ������⣬����Ҳ���ڴ�������������ͼ���Ϊ��Enemy���ɽ����
            if (c.isTrigger)
            {
                //ת�����˵�����״̬
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                //Test:�˺�ֵ���ڶ���
                fsm.Hit(2);
            }
        }
    }

    public void Atk2()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(atk1Point.position, atk2Range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D c in coll)
        {
            //Bug��¼������������������ײ������Ҫ�ų����Ǵ��������Ǹ������⣬����Ҳ���ڴ�������������ͼ���Ϊ��Enemy���ɽ����
            if (c.isTrigger)
            {
                //ת�����˵�����״̬
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                //Test:�˺�ֵ���ڶ���
                fsm.Hit(5);
            }
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(atk1Point.position, atk1Range);
    }
}