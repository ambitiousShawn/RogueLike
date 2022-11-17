using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�������
    private Rigidbody2D rigidbody;
    //�������
    private Animator anim;

    //�ƶ���ֵ
    public float speed = 5;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
    }

    //�ƶ��߼�
    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rigidbody.MovePosition(rigidbody.position + new Vector2(x,y).normalized * speed * Time.fixedDeltaTime);
        //��·����
        anim.SetFloat("speed", new Vector2(x, y).magnitude);

        //ת��
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (x < 0)
        {
            sr.flipX = true;
        }
        else if (x > 0)
        {
            sr.flipX = false;
        }
    }
}
