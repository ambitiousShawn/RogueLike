using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private static CameraMove instance;

    public static CameraMove Instance
    {
        get { return instance; }
    }

    //������ƶ����ٶ�
    public float speed = 1f;
    //����ƶ�Ŀ���
    private Vector3 targetPos;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        MoveToTarget();
    }

    //����ƶ���Ŀ���
    private void MoveToTarget()
    {
        if (targetPos.x != transform.position.x || targetPos.y != transform.position.y)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z),speed * Time.deltaTime);
    }

    //�ⲿ�޸����Ŀ���
    public void ChangeTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
