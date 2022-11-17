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

    //摄像机移动的速度
    public float speed = 1f;
    //相机移动目标点
    private Vector3 targetPos;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        MoveToTarget();
    }

    //相机移动到目标点
    private void MoveToTarget()
    {
        if (targetPos.x != transform.position.x || targetPos.y != transform.position.y)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z),speed * Time.deltaTime);
    }

    //外部修改相机目标点
    public void ChangeTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
