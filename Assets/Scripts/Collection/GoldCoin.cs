using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    private GamePanel gamePanel;

    #region ��ײ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //ʰȡ�߼�
        gamePanel = LevelManager.Instance.gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.name.StartsWith("oneGoldCoin"))
                gamePanel.UpdateCollections(0, 0, 1);
            else
                gamePanel.UpdateCollections(0,0,3);
            Destroy(gameObject);
        }

    }
    #endregion
}