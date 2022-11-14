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
    public int roomNum = 10 ;
    //起始颜色与结束颜色
    public Color startColor, endColor;
    //起始位置
    private Transform generatorPoint;
    //每个房间的位置偏移
    private float XOffset, YOffset;

    //存储房间的列表
    private List<GameObject> rooms = new List<GameObject>();
    //房间图层
    public LayerMask roomLayer;
    #endregion

    void Start()
    {
        //生成位置初始化
        generatorPoint = transform;
        //位置偏移量初始化
        XOffset = 18;
        YOffset = 9;

        //动态生成多个房间
        for (int i = 0;i < roomNum; i++)
        {
            /*
                此处使用Teacher Tang的框架会出问题，无法在加载资源完成后对位置信息进行赋值。会导致出生点的错乱。
            */
            GameObject obj = Instantiate(Resources.Load<GameObject>("Room/BasicRoom"), generatorPoint.position, Quaternion.identity);

            //设置位置信息
            obj.transform.SetParent(generatorPoint.parent);
            //添加进列表
            rooms.Add(obj);
            //改变生成点位置
            ChangePointPos();
        }
        //修改起始点和终点的颜色
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        rooms[roomNum - 1].GetComponent<SpriteRenderer>().color = endColor;
    }

    void Update()
    {
        
    }

    //随机生成下一个房间的位置
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
        } while (Physics2D.OverlapCircle(generatorPoint.position,0.5f,roomLayer));
    }
}
