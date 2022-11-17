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

    #region 角色数值
    //攻击冷却
    public float atkCD = 0.8f;
    private float currCD;

    //重击蓄力时长
    public float HitTimer = 1;
    private float currTimer = 0;
    private bool Jtrigger = false;
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
        //拿到当前房间坐标
        Vector3 currRoom = collision.transform.parent.position;
        if (collision.tag.StartsWith("Door_"))
        {

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

}