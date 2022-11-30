using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEyes : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("delayPut", 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.playerObj.GetComponent<PlayerInteraction>().Wound(4);
            
        }
    }

    public void delayPut()
    {
        PoolManager.Instance.SetElement("Fire/Laser",gameObject);
    }
}
