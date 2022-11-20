using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Transform player;

    void Update()
    {
        player = GameObject.Find("Player").transform;
        if (player != null)
        {
            //�������
            Vector3 pos = player.position;
            transform.position = new Vector3(pos.x,pos.y,-10);
        }
    }
}
