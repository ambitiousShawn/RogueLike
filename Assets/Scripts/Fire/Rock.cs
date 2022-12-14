using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    //飞行方向
    private Vector3 dir;
    //飞行速度
    private float speed = 8 ;
    private GameObject currPlayerPos;
    private Transform throwPoint;
    private void OnEnable()
    {
        currPlayerPos = LevelManager.Instance.playerObj;
        //投掷点
        print(LevelManager.Instance.boss);
        throwPoint = LevelManager.Instance.boss.transform.Find("ThrowPoint");
        transform.position = throwPoint.position;
        dir = (currPlayerPos.transform.position - throwPoint.position).normalized;
        Invoke("delayPut", 3);
    }

    void Update()
    {
        //投掷
        transform.Translate(dir * speed * Time.deltaTime,Space.World);
        //旋转
        transform.Rotate(new Vector3(0, 0, 6),Space.Self);
    }

    private void delayPut()
    {
        PoolManager.Instance.SetElement("Fire/Rock",gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteraction>().Wound(4);
        }
        else if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.name != "Cyclops") 
        {
            collision.gameObject.GetComponent<FSM>().Hit(4);
        }
    }
}
