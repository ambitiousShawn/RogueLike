using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Transform doorRight, doorLeft, doorUp, doorDown;

    //��һ�����䣬���ɲ���ֵ�鿴�Ƿ����
    private bool roomLeft, roomRight, roomUp, roomDown;

    private GameObject mini;
    //�÷����Ƿ�̽����
    private bool isArrived;
    public bool IsArrived
    {
        get { return isArrived; }
        set
        {
            isArrived = value;
            if (!isArrived)
            {
                if (mini != null)
                    mini.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            else
            {
                if (mini != null)
                    mini.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void Start()
    {
        //�ŵĸ�ֵ
        doorRight = transform.Find("Door_right");
        doorLeft = transform.Find("Door_left");
        doorUp = transform.Find("Door_up");
        doorDown = transform.Find("Door_down");

        //�ŵĳ�ʼ��
        doorRight.gameObject.SetActive(roomRight);
        doorLeft.gameObject.SetActive(roomLeft);
        doorUp.gameObject.SetActive(roomUp);
        doorDown.gameObject.SetActive(roomDown);

        mini = transform.Find("Minimap").gameObject;
        IsArrived = false;
        if (GetComponent<SpriteRenderer>().color.r != 0.57f)
        {
            mini.GetComponent<SpriteRenderer>().color = Color.green;
            IsArrived = true;
        }
            
    }

    //�ⲿ�޸��ŵ�״̬�ķ���
    public void UpdateDoor(bool left,bool right,bool up,bool down)
    {
        roomLeft = left;
        roomRight = right;
        roomUp = up;
        roomDown = down;
    }

    
}
