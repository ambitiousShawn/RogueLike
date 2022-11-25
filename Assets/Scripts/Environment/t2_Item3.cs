using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t2_Item3 : MonoBehaviour
{

    public GameObject fire1, fire2, fire3;
    public GameObject stone1, stone2;

    // Update is called once per frame
    void Update()
    {
        if (fire1 != null && fire2 != null && fire3 != null)
        {
            if (fire1.GetComponent<Bonfire>().Ignite && fire2.GetComponent<Bonfire>().Ignite && fire3.GetComponent<Bonfire>().Ignite)
            {
                stone1.SetActive(false);
                stone2.SetActive(false);
            }
        }
        
    }
}
