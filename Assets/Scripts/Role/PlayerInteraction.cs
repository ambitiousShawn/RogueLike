using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�õ�����ƫ����
    private float XOffset, YOffset;

    private void Awake()
    {
        InputManager.Instance.SwitchState(true);
        EventManager.Instance.AddEventListener<KeyCode>("KeyDown", CheckKeyDown);    
    }

    void Start()
    {
        XOffset = 16;
        YOffset = 12;
    }

    //�����̰���
    private void CheckKeyDown(KeyCode keycode)
    {
        print("��������");
        if (keycode == KeyCode.M)
        {
            if (!UIManager.Instance.panelDic.ContainsKey("MiniMapPanel"))
                UIManager.Instance.ShowPanel<BasePanel>("MiniMapPanel");
            else
                UIManager.Instance.HidePanel("MiniMapPanel");
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
                CameraMove.Instance.ChangeTargetPos(new Vector3(currRoom.x + XOffset,currRoom.y,-10));
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
