using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{

    //方向枚举
    private enum Direction { up, down, left, right }
    private Direction direction;

    #region 房间生成信息配置
    //生成数量
    public int roomNum = 10;
    //起始颜色与结束颜色
    public Color startColor, endColor;
    //起始位置
    private Transform generatorPoint;
    //每个房间的位置偏移
    public float XOffset, YOffset;
    //装预设的父物体
    public Transform PutItemsInRoom;
    //当前预设个数
    [Header("每添加一个预设将其 + 1")]
    public int itemsNum;
    //当前关卡是否已经生成某预设
    private bool[,] currItem;
    private int randType, randNum;

    //存储房间的列表
    private List<Room> rooms = new List<Room>();
    //房间图层
    public LayerMask roomLayer;
    //Boss房对象
    [HideInInspector]
    public GameObject boss;
    #endregion

    void Start()
    {
        //生成位置初始化
        generatorPoint = transform;
        //位置偏移量初始化
        XOffset = 16;
        YOffset = 12;

        //初始化哈希表
        currItem = new bool[itemsNum + 1, itemsNum + 1];

        //动态生成多个房间
        for (int i = 0; i < roomNum; i++)
        {
            /*
                此处使用Teacher Tang的框架会出问题，无法在加载资源完成后对位置信息进行赋值。会导致出生点的错乱。
            */
            GameObject obj = Instantiate(Resources.Load<GameObject>("Room/BasicRoom"), generatorPoint.position, Quaternion.identity);

            //设置位置信息
            obj.transform.SetParent(generatorPoint.parent);
            //添加进列表
            rooms.Add(obj.GetComponent<Room>());

            //TODO:在房间图层上生成随机预设地形
            RandItem();

            while (i != 0)
            {
                //print(randType + " " + randNum);
                if (!currItem[randType, randNum])
                {
                    Instantiate(Resources.Load<GameObject>("Items/" + randType + "/Items" + randNum), generatorPoint.position, Quaternion.identity, PutItemsInRoom);
                    currItem[randType, randNum] = true;
                    break;
                }
                else
                {
                    RandItem();
                }
            }
                
            //改变生成点位置
            ChangePointPos();
        }
        //修改起始点和终点的颜色
        //rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        SpriteRenderer[] sr = rooms[0].GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sr)
            if (!s.tag.StartsWith("Door_"))
            {
                s.color = startColor;
            }

        //定义变量存储结束位置
        Transform endRoom = rooms[0].transform;
        foreach (var room in rooms)
        {
            //当前房间的位置信息
            Vector2 pos = room.transform.position;
            if (Mathf.Abs(pos.x /  XOffset) + Mathf.Abs(pos.y / YOffset) > Mathf.Abs(endRoom.position.x / XOffset) + Mathf.Abs(endRoom.position.y / YOffset))
            {
                //计算出最远位置，然后返回
                endRoom = room.transform;
            }

            //为当前房间设置门
            SetupDoor(room);
        }
        //重置Boss房生成点
        if (!endRoom.GetComponent<Room>().roomLeft)
            generatorPoint.position = endRoom.position + new Vector3(-XOffset, 0,0);
        else if (!endRoom.GetComponent<Room>().roomRight)
            generatorPoint.position = endRoom.position + new Vector3(XOffset, 0, 0);
        else if (!endRoom.GetComponent<Room>().roomUp)
            generatorPoint.position = endRoom.position + new Vector3(0, YOffset, 0);
        else if (!endRoom.GetComponent<Room>().roomUp)
            generatorPoint.position = endRoom.position + new Vector3(0, -YOffset, 0);

        //================Boss房生成逻辑===================
        boss = Instantiate(Resources.Load<GameObject>("Room/BasicRoom"), generatorPoint.position, Quaternion.identity);
        //设置位置信息
        boss.transform.SetParent(generatorPoint.parent);
        //添加进列表
        rooms.Add(boss.GetComponent<Room>());
        //设置Boss房和末尾房的房间门的显隐
        SetupDoor(boss.GetComponent<Room>());
        SetupDoor(endRoom.GetComponent<Room>());
        
        foreach (var s in endRoom.GetComponentsInChildren<SpriteRenderer>())
            if (s.tag.StartsWith("Door_"))
                s.color = Color.red;
        //TODO:生成Boss房间的预设
        LevelManager.Instance.boss = Instantiate(Resources.Load<GameObject>("Items/4" + "/Cyclops"), boss.transform.position, Quaternion.identity, PutItemsInRoom);
        LevelManager.Instance.boss.name = "Cyclops";
    }

    //生成随机预设的方法逻辑
    private void RandItem()
    {
        //生成一个随机数
        //预设类型
        randType = Random.Range(1, 101);
        //print(randType);
        if (randType % 10 == 0)
        {
            //生成成就房间的概率控制在0.1
            randType = 1;
        }
        else if (randType % 2 == 1 && randType <= 40)
        {
            //生成无怪物有道具房间概率控制在0.2
            randType = 2;
        }
        else
        {
            //生成有怪物房间概率控制在0.7
            randType = 3;
        }
        //对应类型的预设号
        randNum = Random.Range(1, itemsNum + 1);
    }

    void Update()
    {
        if (boss.GetComponent<Room>().IsArrived)
            LevelManager.Instance.isArrive = true;
    }

    //随机生成下一个房间的位置(注意避免重复)
    private void ChangePointPos()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);
            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += Vector3.up * YOffset;
                    break;
                case Direction.down:
                    generatorPoint.position += Vector3.down * YOffset;
                    break;
                case Direction.left:
                    generatorPoint.position += Vector3.left * XOffset;
                    break;
                case Direction.right:
                    generatorPoint.position += Vector3.right * XOffset;
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.5f, roomLayer));
    }

    //根据上下左右是否有房间修改是否有门的布尔变量
    private void SetupDoor(Room currRoom)
    {
        Vector3 roomPos = currRoom.transform.position;

        bool roomUp = Physics2D.OverlapCircle(roomPos + Vector3.up * YOffset, 0.2f, roomLayer);
        bool roomDown = Physics2D.OverlapCircle(roomPos + Vector3.down * YOffset, 0.2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPos + Vector3.left * XOffset, 0.2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPos + Vector3.right * XOffset, 0.2f, roomLayer);
        //更新到房间属性
        currRoom.UpdateDoor(roomLeft, roomRight, roomUp, roomDown);
    }
}
