using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Transform doorRight, doorLeft, doorUp, doorDown;

    //对一个房间，生成布尔值查看是否存在
    [HideInInspector]
    public bool roomLeft, roomRight, roomUp, roomDown;

    private GameObject mini;
    //该房间是否被探索过
    private bool isArrived;
    //当前房间绑定的预设(仅仅适用有怪物的房间预设)
    public int currItems = 0;

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

        mini = transform.Find("Minimap").gameObject;
        IsArrived = false;
        if (GetComponent<SpriteRenderer>().color.r != 0.57f &&
            GetComponent<SpriteRenderer>().color.r != 1)
        {
            IsArrived = true;
        }
            
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
