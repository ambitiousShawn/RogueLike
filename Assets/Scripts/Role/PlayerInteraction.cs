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
    //攻击冷却
    public float atkCD = 0.4f;
    private float currCD;

    //重击蓄力时长
    public float HitTimer = 1;
    private float currTimer = 0;
    private bool Jtrigger = false;

    //近战轻击范围
    private Transform atk1Point;
    private float atk1Range;
    //近战重击范围
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
        //组件赋值
        anim = GetComponent<Animator>();
        gamePanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();

        XOffset = 16;
        YOffset = 12;
        
        currCD = atkCD;

        //找到开火点
        firePoint = transform.Find("firePoint");
        Init();
    }

    //读表给角色赋值
    void Init()
    {
        atk1Point = transform.Find("Atk1Range");
        //Test:读表
        atk1Range = 0.5f;
        atk2Range = 0.7f;
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
            currTimer += Time.deltaTime;
        else
            currTimer = 0;

        if (currTimer >= 1)
            anim.SetBool("isCombo", true);
        //更新蓄力时间UI
        gamePanel.UpdateBarState(currTimer , HitTimer);
    }

    //检测键盘按下
    private void CheckKeyDown(KeyCode keycode)
    {
        if (keycode == KeyCode.M)
        {
            if (!UIManager.Instance.panelDic.ContainsKey("MiniMapPanel"))
                UIManager.Instance.ShowPanel<BasePanel>("MiniMapPanel");
            else
                UIManager.Instance.HidePanel("MiniMapPanel");
        }

        //角色攻击
        if (keycode == KeyCode.J && currCD >= atkCD)
        {
            
            currCD = 0;
            anim.SetTrigger("Atk1");
            //显示进度条
            gamePanel.SwitchBarState(true);
            Jtrigger = true;
        }

        if (keycode == KeyCode.K && currCD >= atkCD)
        {
            currCD = 0;
            anim.SetTrigger("Shoot");
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
        print(GameManager.Instance.isSucceed);
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
    }
    #endregion


    #region 动画事件
    public void Fire()
    {
        //开火时创建出子弹(后续改为读表数据)
        /*ResourcesManager.Instance.LoadAsync<GameObject>("Fire/火焰子弹",(obj) => 
        {
            //创建出来设置位置，旋转以及父物体
            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.identity;
        });*/

        PoolManager.Instance.GetElement("Fire/火焰子弹", firePoint);
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
                //Test:伤害值后期读表
                fsm.Hit(2);
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
                //Test:伤害值后期读表
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