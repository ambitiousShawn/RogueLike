using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    //¶¯»­ÊÂ¼þ£ºËé±ùÉËº¦
    public void IceDamage()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));

        foreach (Collider2D c in coll)
        {
            c.GetComponent<PlayerInteraction>().Wound(4);
        }
    }
}
