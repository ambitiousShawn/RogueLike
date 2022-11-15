using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Transform doorRight, doorLeft, doorUp, doorDown;

    //��һ�����䣬���ɲ���ֵ�鿴�Ƿ����
    private bool roomLeft, roomRight, roomUp, roomDown;

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
