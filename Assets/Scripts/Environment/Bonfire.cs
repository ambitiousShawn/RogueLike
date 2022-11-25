using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private Transform bonfire;

    [HideInInspector]
    public bool Ignite;

    void Start()
    {
        bonfire = transform.Find("fire");
        bonfire.gameObject.SetActive(false);
        Ignite = false;
    }

    #region ���������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fire/PowerfulFire" && !Ignite)
        {
            bonfire.gameObject.SetActive(true);
            //����һö���
            ResourcesManager.Instance.Load<GameObject>("Collections/oneGoldCoin").transform.position = transform.position + Vector3.right;
            Ignite = true;
        }
    }
    #endregion
}
