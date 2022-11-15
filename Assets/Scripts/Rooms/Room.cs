using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Transform doorRight, doorLeft, doorUp, doorDown;

    //对一个房间，生成布尔值查看是否存在
    private bool roomLeft, roomRight, roomUp, roomDown;

    void Start()
    {
        //门的赋值
        doorRight = transform.Find("Door_right");
        doorLeft = transform.Find("Door_left");
        doorUp = transform.Find("Door_up");
        doorDown = transform.Find("Door_down");

        //门的初始化
        doorRight.gameObject.SetActive(roomRight);
        doorLeft.gameObject.SetActive(roomLeft);
        doorUp.gameObject.SetActive(roomUp);
        doorDown.gameObject.SetActive(roomDown);
    }

    //外部修改门的状态的方法
    public void UpdateDoor(bool left,bool right,bool up,bool down)
    {
        roomLeft = left;
        roomRight = right;
        roomUp = up;
        roomDown = down;
    }
}
