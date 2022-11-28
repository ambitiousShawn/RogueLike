using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�õ�����ƫ����
    private float XOffset, YOffset;
    //�������
    private Animator anim;
    //���������
    private Transform firePoint;

    //�ϵ�ģʽ(�ܻ����л���״̬1s)��
    private bool isGod;

    #region ��ɫ��ֵ(����ֵ)
    //�������
    [HideInInspector]
    public PlayerInfo playerInfo;
    //��������
    private WeaponInfo weaponInfo;

    //������ȴ
    private float currCD;
    //��ǰѪ��
    [HideInInspector]
    public int currHealth;
    //��ǰ������
    private int currAtk;
    //��ǰ�����ӵ�����
    private int currBullet;

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
        /* UIManager.Instance.ShowPanel<GamePanel>("GamePanel", (panel) =>
         {
             gamePanel = panel;
         });*/
    }

    void Start()
    {
        
        //�����ֵ
        anim = GetComponent<Animator>();

        XOffset = 16;
        YOffset = 12;

        //Bug��¼������gamePanel���첽���أ�������Ҫһ֡ʱ����أ��޷�����Start���渳ֵ
        //gamePanel = GameManager.Instance.gamePanel;
        //print(gamePanel);

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
        //�����ӵ�������ʼ��
        currBullet = weaponInfo.BulletNum;
    }

    void Update()
    {
        if (GameManager.Instance.isDead)
        {
            anim.SetBool("isDead", true);
            InputManager.Instance.SwitchState(false);
        }
        CoolDown();
    }

    //������ȴʱ�䷽��
    private void CoolDown()
    {
        currCD += Time.deltaTime;
        if (currCD >= 1.5)
        {
            currCD = 1.5f;
        }

        //�ػ���������
        if (Jtrigger)
        {
            if (isUp)
            {
                currTimer += Time.deltaTime * 0.8f;
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
        if (GameManager.Instance.gamePanel != null)
            GameManager.Instance.gamePanel.UpdateBarState(currTimer , HitTimer);
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
            GameManager.Instance.gamePanel.SwitchBarState(true);
            Jtrigger = true;
        }

        //��ɫ���
        if (keycode == KeyCode.K && currCD >= weaponInfo.AtkCD)
        {
            //�ӵ������߼�
            currBullet--;
            if (currBullet < 0)
            {
                currBullet = 0;
                //TODO:�����ɲ��ſ�����Ч
            }
            else
            {
                currCD = 0;
                anim.SetTrigger("Shoot");
            }
            GameManager.Instance.gamePanel.SwitchWeapon(currBullet, weaponInfo.BulletNum);
        }

        //��ը��
        if (keycode == KeyCode.E && currCD >= playerInfo.AtkCD)
        {
            if (DataManager.Instance.bomb > 0)
            {
                //�����λ������һöը��������Ĭ�϶���
                GameObject bomb = ResourcesManager.Instance.Load<GameObject>("Collections/Bomb");
                bomb.transform.position = transform.position;
                bomb.GetComponent<Animator>().enabled = true;
                bomb.GetComponent<Collider2D>().enabled = false;
                //������������һ
                GameManager.Instance.gamePanel.UpdateCollections(-1);
            }
            
        }

        //�Լ���
        if (keycode == KeyCode.R && currHealth < playerInfo.Health && DataManager.Instance.chicken > 0)
        {
            //���������1
            GameManager.Instance.gamePanel.UpdateCollections(0, 0, 0, -1);

            //��Ѫ�߼�
            currHealth += 3;
            if (currHealth > playerInfo.Health)
            {
                currHealth = playerInfo.Health;
            }
            GameManager.Instance.gamePanel.UpdateBloodBar(currHealth, playerInfo.Health);
        }

        //�����������
        if (keycode == KeyCode.Escape)
        {
            if (UIManager.Instance.GetPanel<InstructionPanel>("InstructionPanel") != null)
            {
                UIManager.Instance.HidePanel("InstructionPanel");
                UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
            }
            else if (UIManager.Instance.GetPanel<SettingPanel>("SettingPanel") != null)
                UIManager.Instance.HidePanel("SettingPanel");
            else
                UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
        }

        //Test:�л�������������ԣ�������Ҫɾ��
        if (keycode == KeyCode.Alpha1)
        {
            GameManager.Instance.weaponNum = 0;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            currBullet = weaponInfo.BulletNum;
            GameManager.Instance.gamePanel.SwitchWeapon(currBullet, weaponInfo.BulletNum);
        }
        else if (keycode == KeyCode.Alpha2)
        {
            GameManager.Instance.weaponNum = 1;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            currBullet = weaponInfo.BulletNum;
            GameManager.Instance.gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum);
        }
        else if (keycode == KeyCode.Alpha3)
        {
            GameManager.Instance.weaponNum = 2;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            currBullet = weaponInfo.BulletNum;
            GameManager.Instance.gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum);
        }
        else if (keycode == KeyCode.Alpha4)
        {
            GameManager.Instance.weaponNum = 3;
            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            currBullet = weaponInfo.BulletNum;
            GameManager.Instance.gamePanel.SwitchWeapon(weaponInfo.BulletNum, weaponInfo.BulletNum);
        }
    }

    //������̧��
    private void CheckKeyUp(KeyCode keycode)
    {
        if (keycode == KeyCode.J)
        {
            anim.SetBool("isCombo", false);
            //���ؽ�����
            GameManager.Instance.gamePanel.SwitchBarState(false);
            Jtrigger = false;
        }
    }

    //���˺���
    public void Wound(int damage)
    {
        if (isGod) return;

        currHealth -= damage;
        if (currHealth <= 0)
        {
            currHealth = 0;
            GameManager.Instance.isDead = true;
        }
        GameManager.Instance.gamePanel.UpdateBloodBar(currHealth, playerInfo.Health);

        //�ܻ����
        GetComponent<SpriteRenderer>().color = Color.red;
        isGod = true;
        Invoke("color", 1f);
    }

    private void color()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        isGod = false;
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

        //��������ʱ��ʰȡ�߼�
        if (collision.CompareTag("Weapon"))
        {
            //�������һ���������
            GameManager.Instance.weaponNum = Random.Range(1, 4);

            weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
            currBullet = weaponInfo.BulletNum;
            GameManager.Instance.gamePanel.SwitchWeapon(currBullet, weaponInfo.BulletNum);

            //�л�����������ͼ
            collision.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.Load<Sprite>("Room/����");
            Destroy(collision.gameObject,1f);
            
        }

        //����Buffʱ��ʰȡ���ӳɵ�������
        if (collision.CompareTag("Buff"))
        {
            //�������һ��Buff���
            int buffNum = Random.Range(1, 4);

            switch (buffNum)
            {
                //TODO:�����߼�
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            //�л�����������ͼ
            collision.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.Load<Sprite>("Room/����");
            Destroy(collision.gameObject, 1f);
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
                FSM_Boss fsm_boss = c.GetComponent<FSM_Boss>();
                if (fsm != null)
                {
                    fsm.parameter.getHit = true;
                    //�˺�ֵ���ڶ���
                    fsm.Hit(playerInfo.BaseAtk);
                }
                else
                {
                    fsm_boss.parameter.getHit = true;
                    fsm_boss.Hit(playerInfo.BaseAtk);
                }
               
                
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
                FSM_Boss fsm_boss = c.GetComponent<FSM_Boss>();
                if (fsm != null)
                {
                    fsm.parameter.getHit = true;
                    //�˺�ֵ���ڶ���
                    fsm.Hit(playerInfo.BaseAtk * 3);
                }
                else
                {
                    fsm_boss.parameter.getHit = true;
                    fsm_boss.Hit(playerInfo.BaseAtk * 3);
                }
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
                FSM_Boss fsm_boss = c.GetComponent<FSM_Boss>();
                if (fsm != null)
                {
                    fsm.parameter.getHit = true;
                    //�˺�ֵ���ڶ���
                    fsm.Hit(playerInfo.BaseAtk * 3);
                }
                else
                {
                    fsm_boss.parameter.getHit = true;
                    fsm_boss.Hit(playerInfo.BaseAtk * 3);
                }
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