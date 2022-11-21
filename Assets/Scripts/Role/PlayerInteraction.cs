using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //拿到房间偏移量
    private float XOffset, YOffset;
    //动画组件
    private Animator anim;
    //蓄力UI
    private GamePanel gamePanel;
    //开火点坐标
    private Transform firePoint;


    #region 角色数值(读表赋值)
    //玩家数据
    private PlayerInfo playerInfo;
    //武器数据
    private WeaponInfo weaponInfo;

    //攻击冷却
    private float currCD;
    //当前血量
    private int currHealth;
    //当前攻击力
    private int currAtk;

    //重击蓄力时长
    public float HitTimer = 1;
    private float currTimer = 0;
    private bool Jtrigger = false;
    private bool isUp = true;

    //近战轻击范围
    private Transform atk1Point;
    private float atk1Range;
    //近战重击范围
    private float atk2Range;
    //法师重击范围
    private float atk3Range;
    #endregion

    private GameObject Obj;
    //存储特效的列表
    private List<GameObject> list = new List<GameObject>();

    private void Awake()
    {
        InputManager.Instance.SwitchState(true);
        EventManager.Instance.AddEventListener<KeyCode>("KeyDown", CheckKeyDown);
        EventManager.Instance.AddEventListener<KeyCode>("KeyUp", CheckKeyUp);    
    }

    void Start()
    {
        //组件赋值
        anim = GetComponent<Animator>();

        UIManager.Instance.ShowPanel<GamePanel>("GamePanel", (panel) =>
        {
            gamePanel = panel;
        });

        XOffset = 16;
        YOffset = 12;

        //找到开火点
        firePoint = transform.Find("firePoint");
        Init();
    }

    //读表给角色赋值
    void Init()
    {
        atk1Point = transform.Find("Atk1Range");
        //读表赋值
        //攻击距离需要手动测量，就不好读表了
        atk1Range = 0.5f;
        atk2Range = 0.7f;
        atk3Range = 3f;

        playerInfo = DataManager.Instance.playerInfos[GameManager.Instance.num];
        weaponInfo = DataManager.Instance.weaponInfos[GameManager.Instance.weaponNum];
        //cd计数器初始化
        currCD = playerInfo.AtkCD;
        //血量初始化
        currHealth = playerInfo.Health;
        //攻击力初始化
        currAtk = playerInfo.BaseAtk;
    }

    void Update()
    {
        CoolDown();
    }

    //计算冷却时间方法
    private void CoolDown()
    {
        currCD += Time.deltaTime;
        if (currCD >= 1)
        {
            currCD = 1;
        }

        //重击蓄力计算
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

        
        //更新蓄力时间UI
        //Bug记录：不加非空判断会报空指针(异步加载的先后性)
        if (gamePanel != null)
            gamePanel.UpdateBarState(currTimer , HitTimer);
    }

    //检测键盘按下
    private void CheckKeyDown(KeyCode keycode)
    {
        //开启 / 关闭小地图
        if (keycode == KeyCode.M)
        {
            if (!UIManager.Instance.panelDic.ContainsKey("MiniMapPanel"))
                UIManager.Instance.ShowPanel<MiniMapPanel>("MiniMapPanel");
            else
                UIManager.Instance.HidePanel("MiniMapPanel");
        }

        //角色攻击
        if (keycode == KeyCode.J && currCD >= playerInfo.AtkCD)
        {
            
            currCD = 0;
            anim.SetTrigger("Atk1");
            //显示进度条
            gamePanel.SwitchBarState(true);
            Jtrigger = true;
        }

        //角色射击
        if (keycode == KeyCode.K && currCD >= weaponInfo.AtkCD)
        {
            currCD = 0;
            anim.SetTrigger("Shoot");
        }

        //切换武器
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

    //检测键盘抬起
    private void CheckKeyUp(KeyCode keycode)
    {
        if (keycode == KeyCode.J)
        {
            anim.SetBool("isCombo", false);
            //隐藏进度条
            gamePanel.SwitchBarState(false);
            Jtrigger = false;
        }
    }

    #region 触发器相关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.StartsWith("Door_") && GameManager.Instance.isSucceed)
        {
            
            //拿到当前房间坐标
            Vector3 currRoom = collision.transform.parent.position;
            //获得房间偏移量
            if (collision.tag == "Door_right")
            {
                CameraMove.Instance.ChangeTargetPos(new Vector3(currRoom.x + XOffset, currRoom.y, -10));
                //移动玩家
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

        //进入下一个房间时，将其设置为已探索
        if (collision.CompareTag("Room") && !collision.transform.GetComponent<Room>().IsArrived)
        {
            Room currRoom = collision.transform.GetComponent<Room>();
            currRoom.IsArrived = true;
        }
    }
    #endregion


    #region 动画事件
    //默认子弹
    public void Fire()
    {
        //武器系统出来时，此处子弹路径读表
        PoolManager.Instance.GetElement(weaponInfo.Resource, firePoint);
    }

    public void Atk1()
    {
        //播放近战轻击动画时，检测范围内是否存在敌人标签，存在则调用敌人受伤逻辑
        Collider2D[] coll = Physics2D.OverlapCircleAll(atk1Point.position, atk1Range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D c in coll)
        {
            //Bug记录：怪物身上有两个碰撞器，需要排除不是触发器的那个，此外，子类也存在触发器，将子类图层改为非Enemy即可解决。
            if (c.isTrigger)
            {
                //转换敌人的受伤状态
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                //伤害值后期读表
                fsm.Hit(playerInfo.BaseAtk);
            }
        }
    }

    public void Atk2()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(atk1Point.position, atk2Range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D c in coll)
        {
            //Bug记录：怪物身上有两个碰撞器，需要排除不是触发器的那个，此外，子类也存在触发器，将子类图层改为非Enemy即可解决。
            if (c.isTrigger)
            {
                //转换敌人的受伤状态
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                fsm.Hit(playerInfo.BaseAtk * 3); ;
            }
        }
    }

    //法师重击
    public void Atk3()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, atk3Range, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D c in coll)
        {
            //Bug记录：怪物身上有两个碰撞器，需要排除不是触发器的那个，此外，子类也存在触发器，将子类图层改为非Enemy即可解决。
            if (c.isTrigger)
            {
                //闪电特效(用缓存池)
                Obj = PoolManager.Instance.GetElement("Fire/闪电");
                Obj.transform.position = new Vector2(c.transform.position.x, c.transform.position.y + 1);
                list.Add(Obj);
                Invoke("DelayPut", 1f);
                //转换敌人的受伤状态
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                //Test:伤害值后期读表
                fsm.Hit(playerInfo.BaseAtk * 3); ;
            }
        }
    }

    private void DelayPut()
    {
        foreach(GameObject o in list)
            if (o != null)
                PoolManager.Instance.SetElement("Fire/闪电", o);
    }

    //剑客重击
    public void Atk4()
    {
        PoolManager.Instance.GetElement("Fire/剑气", firePoint);
    }

    //拳师冲击波
    public void Atk5()
    {
        PoolManager.Instance.GetElement("Fire/冲击波", firePoint);
    }

    //拳师轻击
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