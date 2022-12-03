using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DelayPut", 1f);
    }

    //�ӳ�����
    private void DelayPut()
    {
        PoolManager.Instance.SetElement("Fire/Kun/Water", gameObject);
    }

    //�����¼�
    public void Water1()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 1f, 1 << LayerMask.NameToLayer("Player"));

        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(5);
        }
    }
}
