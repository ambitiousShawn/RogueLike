using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GamePanel gamePanel;

    #region Åö×²Æ÷Ïà¹Ø
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Ê°È¡Âß¼­
        gamePanel = GameManager.Instance.playerObj.GetComponent<PlayerInteraction>().gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            gamePanel.UpdateCollections(0,1);
            Destroy(gameObject);
        }

    }
    #endregion
}
