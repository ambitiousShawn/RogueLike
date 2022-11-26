using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrial : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("delayPut", 0.8f);
    }

    private void delayPut()
    {
        PoolManager.Instance.SetElement("Fire/FireTrial",gameObject);
    }

    #region 动画事件
    //烈火审判一，二段
    public void FireTrial1()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 2f, 1 << LayerMask.NameToLayer("Player"));
        
        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(6);
        }
    }
    public void FireTrial2()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 3.5f, 1 << LayerMask.NameToLayer("Player"));

        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(6);
        }
    }
    #endregion
}
