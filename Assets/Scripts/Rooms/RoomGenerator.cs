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
    public int roomNum = 10 ;
    //��ʼ��ɫ�������ɫ
    public Color startColor, endColor;
    //��ʼλ��
    private Transform generatorPoint;
    //ÿ�������λ��ƫ��
    private float XOffset, YOffset;

    //�洢������б�
    private List<GameObject> rooms = new List<GameObject>();
    //����ͼ��
    public LayerMask roomLayer;
    #endregion

    void Start()
    {
        //����λ�ó�ʼ��
        generatorPoint = transform;
        //λ��ƫ������ʼ��
        XOffset = 18;
        YOffset = 9;

        //��̬���ɶ������
        for (int i = 0;i < roomNum; i++)
        {
            /*
                �˴�ʹ��Teacher Tang�Ŀ�ܻ�����⣬�޷��ڼ�����Դ��ɺ��λ����Ϣ���и�ֵ���ᵼ�³�����Ĵ��ҡ�
            */
            GameObject obj = Instantiate(Resources.Load<GameObject>("Room/BasicRoom"), generatorPoint.position, Quaternion.identity);

            //����λ����Ϣ
            obj.transform.SetParent(generatorPoint.parent);
            //��ӽ��б�
            rooms.Add(obj);
            //�ı����ɵ�λ��
            ChangePointPos();
        }
        //�޸���ʼ����յ����ɫ
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        rooms[roomNum - 1].GetComponent<SpriteRenderer>().color = endColor;
    }

    void Update()
    {
        
    }

    //���������һ�������λ��
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
