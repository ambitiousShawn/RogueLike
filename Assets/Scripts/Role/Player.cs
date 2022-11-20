using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //刚体组件
    private Rigidbody2D rigidbody;
    //动画组件
    private Animator anim;

    //玩家数据
    private PlayerInfo info;

    //移动数值
    private float speed;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        info = DataManager.Instance.playerInfos[2];
        speed = info.MoveSpeed;
    }

    void FixedUpdate()
    {
        Move();
    }

    //移动逻辑
    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rigidbody.MovePosition(rigidbody.position + new Vector2(x,y).normalized * speed * Time.fixedDeltaTime);
        //走路动画
        anim.SetFloat("speed", new Vector2(x, y).magnitude);

        //转向
        if (x < 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
