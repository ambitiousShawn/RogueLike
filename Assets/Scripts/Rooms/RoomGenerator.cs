using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{

    //����ö��
    private enum Direction { up, down, left, right }
    private Direction direction;

    #region ����������Ϣ����
    //��������
    public int roomNum = 10;
    //��ʼ��ɫ�������ɫ
    public Color startColor, endColor;
    //��ʼλ��
    private Transform generatorPoint;
    //ÿ�������λ��ƫ��
    public float XOffset, YOffset;

    //�洢������б�
    private List<Room> rooms = new List<Room>();
    //����ͼ��
    public LayerMask roomLayer;
    #endregion

    void Start()
    {
        //����λ�ó�ʼ��
        generatorPoint = transform;
        //λ��ƫ������ʼ��
        XOffset = 16;
        YOffset = 12;

        //��̬���ɶ������
        for (int i = 0; i < roomNum; i++)
        {
            /*
                �˴�ʹ��Teacher Tang�Ŀ�ܻ�����⣬�޷��ڼ�����Դ��ɺ��λ����Ϣ���и�ֵ���ᵼ�³�����Ĵ��ҡ�
            */
            GameObject obj = Instantiate(Resources.Load<GameObject>("Room/BasicRoom"), generatorPoint.position, Quaternion.identity);

            //����λ����Ϣ
            obj.transform.SetParent(generatorPoint.parent);
            //��ӽ��б�
            rooms.Add(obj.GetComponent<Room>());
            //�ı����ɵ�λ��
            ChangePointPos();
        }
        //�޸���ʼ����յ����ɫ
        //rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        SpriteRenderer[] sr = rooms[0].GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sr)
            if (!s.tag.StartsWith ("Door_"))
                s.color = startColor;
        

        //��������洢����λ��
        Transform endRoom = rooms[0].transform;
        foreach (var room in rooms)
        {
            //��ǰ�����λ����Ϣ
            Vector2 pos = room.transform.position;
            if (Mathf.Abs(pos.x / XOffset) + Mathf.Abs(pos.y / YOffset) > Mathf.Abs(endRoom.position.x / XOffset) + Mathf.Abs(endRoom.position.y / YOffset))
            {
                //�������Զλ�ã�Ȼ�󷵻�
                endRoom = room.transform;
            }

            //Ϊ��ǰ����������
            SetupDoor(room);
        }
        //endRoom.GetComponent<SpriteRenderer>().color = endColor;
        sr = endRoom.GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sr)
            if (!s.tag.StartsWith("Door_"))
                s.color = endColor;
    }

    void Update()
    {

    }

    //���������һ�������λ��(ע������ظ�)
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

    //�������������Ƿ��з����޸��Ƿ����ŵĲ�������
    private void SetupDoor(Room currRoom)
    {
        Vector3 roomPos = currRoom.transform.position;

        bool roomUp = Physics2D.OverlapCircle(roomPos + Vector3.up * YOffset, 0.2f, roomLayer);
        bool roomDown = Physics2D.OverlapCircle(roomPos + Vector3.down * YOffset, 0.2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPos + Vector3.left * XOffset, 0.2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPos + Vector3.right * XOffset, 0.2f, roomLayer);
        //���µ���������
        currRoom.UpdateDoor(roomLeft, roomRight, roomUp, roomDown);
    }
}
