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

    //�ӳ�����
    private void DelayPut()
    {
        PoolManager.Instance.SetElement("Fire/Kun/Burning", gameObject);
    }

    //�����¼�
    public void Burning1()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 1f, 1 << LayerMask.NameToLayer("Player"));

        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(5);
        }
    }
}
