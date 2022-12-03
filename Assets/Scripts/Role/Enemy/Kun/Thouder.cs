using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thouder : MonoBehaviour
{

    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DelayPut", 0.8f);
    }

    //延迟销毁
    private void DelayPut()
    {
        PoolManager.Instance.SetElement("Fire/Kun/Thouder",gameObject);
    }

    //动画事件
    public void Thouder1()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 1f, 1 << LayerMask.NameToLayer("Player"));

        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(6);
        }
    }
}
