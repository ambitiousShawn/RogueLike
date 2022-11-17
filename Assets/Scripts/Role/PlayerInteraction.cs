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

    #region ��ɫ��ֵ
    //������ȴ
    public float atkCD = 0.8f;
    private float currCD;

    //�ػ�����ʱ��
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
        //�����ֵ
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
        //�õ���ǰ��������
        Vector3 currRoom = collision.transform.parent.position;
        if (collision.tag.StartsWith("Door_"))
        {

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

}