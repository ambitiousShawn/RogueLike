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
    //�������
    private PlayerInfo playerInfo;
    //��������
    private WeaponInfo weaponInfo;

    //������ȴ
    private float currCD;
    //��ǰѪ��
    private int currHealth;
    //��ǰ������
    private int currAtk;

    //�ػ�����ʱ��
    public float HitTimer = 1;
    private float currTimer = 0;
    private bool Jtrigger = false;
    private bool isUp = true;

    //��ս�����Χ
    private Transform atk1Point;
    private float atk1Range;
    //��ս�ػ���Χ
    private float atk2Range;
    //��ʦ�ػ���Χ
    private float atk3Range;
    #endregion

    private GameObject Obj;
    //�洢��Ч���б�
    private List<GameObject> list = new List<GameObject>();

    private void Awake()
    {
        InputManager.Instance.SwitchState(true);
        EventManager.Instance.AddEventListener<KeyCode>("KeyDown", CheckKeyDown);
        EventManager.Instance.AddEventListener<KeyCode>("KeyUp", CheckKeyUp);    
    }

    void Start()
    {
        //�����ֵ
        anim = GetComponent<Animator>();

        UIManager.Instance.ShowPanel<GamePanel>("GamePanel", (panel) =>
        {
            gamePanel = panel;
        });

        XOffset = 16;
        YOffset = 12;

        //�ҵ������
        firePoint = transform.Find("firePoint");
        Init();
    }

    //�������ɫ��ֵ
    void Init()
    {
        atk1Point = transform.Find("Atk1Range");
        //����ֵ
        //����������Ҫ�ֶ��������Ͳ��ö�����
        atk1Range = 0.5f;
        atk2Range = 0.7f;
        atk3Range = 3f;

        playerInfo = DataManager.Instance.playerInfos[GameManager.Instance.num];
        weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
        //cd��������ʼ��
        currCD = playerInfo.AtkCD;
        //Ѫ����ʼ��
        currHealth = playerInfo.Health;
        //��������ʼ��
        currAtk = playerInfo.BaseAtk;
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
        {
            if (isUp)
            {
                currTimer += Time.deltaTime * 0.5f;
                if (currTimer >= 1)
                {
                    anim.SetBool("isCombo", true);
                    isUp = false;
                }
            }
            else
            {
                currTimer -= Time.deltaTime * 0.5f;
                if (currTimer <= 0)
                {
                    anim.SetBool("isCombo", false);
                    isUp = true;
                }
            }
            
        } 
        else
            currTimer = 0;

        
        //��������ʱ��UI
        //Bug��¼�����ӷǿ��жϻᱨ��ָ��(�첽���ص��Ⱥ���)
        if (gamePanel != null)
            gamePanel.UpdateBarState(currTimer , HitTimer);
    }

    //�����̰���
    private void CheckKeyDown(KeyCode keycode)
    {
        //���� / �ر�С��ͼ
        if (keycode == KeyCode.M)
        {
            if (!UIManager.Instance.panelDic.ContainsKey("MiniMapPanel"))
                UIManager.Instance.ShowPanel<MiniMapPanel>("MiniMapPanel");
            else
                UIManager.Instance.HidePanel("MiniMapPanel");
        }

        //��ɫ����
        if (keycode == KeyCode.J && currCD >= playerInfo.AtkCD)
        {
            
            currCD = 0;
            anim.SetTrigger("Atk1");
            //��ʾ������
            gamePanel.SwitchBarState(true);
            Jtrigger = true;
        }

        //��ɫ���
        if (keycode == KeyCode.K && currCD >= weaponInfo.AtkCD)
        {
            currCD = 0;
            anim.SetTrigger("Shoot");
        }

        //�л�����
        if (keycode == KeyCode.Alpha1)
        {
            GameManager.Instance.weaponNum = 0;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum, 0);
        }
        else if (keycode == KeyCode.Alpha2)
        {
            GameManager.Instance.weaponNum = 1;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum, 1);
        }
        else if (keycode == KeyCode.Alpha3)
        {
            GameManager.Instance.weaponNum = 2;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum, 2);
        }
        else if (keycode == KeyCode.Alpha4)
        {
            GameManager.Instance.weaponNum = 3;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum, 3);
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

        //������һ������ʱ����������Ϊ��̽��
        if (collision.CompareTag("Room") && !collision.transform.GetComponent<Room>().IsArrived)
        {
            Room currRoom = collision.transform.GetComponent<Room>();
            currRoom.IsArrived = true;
        }
    }
    #endregion


    #region �����¼�
    //Ĭ���ӵ�
    public void Fire()
    {
        //����ϵͳ����ʱ���˴��ӵ�·������
        PoolManager.Instance.GetElement(weaponInfo.Resource, firePoint);
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
                //�˺�ֵ���ڶ���
                fsm.Hit(playerInfo.BaseAtk);
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
                fsm.Hit(playerInfo.BaseAtk * 3); ;
            }
        }
    }

    //��ʦ�ػ�
    public void Atk3()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, atk3Range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D c in coll)
        {
            //Bug��¼������������������ײ������Ҫ�ų����Ǵ��������Ǹ������⣬����Ҳ���ڴ�������������ͼ���Ϊ��Enemy���ɽ����
            if (c.isTrigger)
            {
                //������Ч(�û����)
                Obj = PoolManager.Instance.GetElement("Fire/����");
                Obj.transform.position = new Vector2(c.transform.position.x, c.transform.position.y + 1);
                list.Add(Obj);
                Invoke("DelayPut", 1f);
                //ת�����˵�����״̬
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                //Test:�˺�ֵ���ڶ���
                fsm.Hit(playerInfo.BaseAtk * 3); ;
            }
        }
    }

    private void DelayPut()
    {
        foreach(GameObject o in list)
            if (o != null)
                PoolManager.Instance.SetElement("Fire/����", o);
    }

    //�����ػ�
    public void Atk4()
    {
        PoolManager.Instance.GetElement("Fire/����", firePoint);
    }

    //ȭʦ�����
    public void Atk5()
    {
        PoolManager.Instance.GetElement("Fire/�����", firePoint);
    }

    //ȭʦ���
    public void Atk6()
    {
        PoolManager.Instance.GetElement("Fire/Gift", firePoint);
    }
    #endregion

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, atk3Range);
    }*/

    
}