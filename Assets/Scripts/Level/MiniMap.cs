using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private GameObject player;

    void Update()
    {
        player = LevelManager.Instance.playerObj;
        if (player != null)
        {
            //�������
            Vector3 pos = player.transform.position;
            transform.position = new Vector3(pos.x,pos.y,-10);
        }
    }
}
