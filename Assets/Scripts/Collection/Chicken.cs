using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private GamePanel gamePanel;

    #region ��ײ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //ʰȡ�߼�
        gamePanel = GameManager.Instance.playerObj.GetComponent<PlayerInteraction>().gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            //�õ���ҽ����ű�
            PlayerInteraction pi = collision.gameObject.GetComponent<PlayerInteraction>();
            if (pi.currHealth < pi.playerInfo.Health)
            {
                //����Ѫʱ���Լ���ֱ�ӻ�Ѫ
                pi.currHealth += 3;
                if (pi.currHealth > pi.playerInfo.Health)
                {
                    pi.currHealth = pi.playerInfo.Health;
                }
                gamePanel.UpdateBloodBar(pi.currHealth, pi.playerInfo.Health);
            }
            else
            {
                //��������Ѫʱ���Լ��Ƚ����ռ�������
                gamePanel.UpdateCollections(0, 0, 0, 1);
            }
        }

    }
    #endregion
}
