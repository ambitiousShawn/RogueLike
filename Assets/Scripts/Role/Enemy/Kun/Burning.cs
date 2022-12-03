using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DelayPut", 1f);
    }

    //延迟销毁
    private void DelayPut()
    {
        PoolManager.Instance.SetElement("Fire/Kun/Burning", gameObject);
    }

    //动画事件
    public void Burning1()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 1f, 1 << LayerMask.NameToLayer("Player"));

        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(5);
        }
    }
}
