using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GamePanel gamePanel;

    #region ��ײ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //ʰȡ�߼�
        gamePanel = GameManager.Instance.playerObj.GetComponent<PlayerInteraction>().gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            gamePanel.UpdateCollections(0,1);
            Destroy(gameObject);
        }

    }
    #endregion
}
