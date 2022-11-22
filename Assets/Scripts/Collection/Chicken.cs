using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private GamePanel gamePanel;

    #region 碰撞器相关
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //拾取逻辑
        gamePanel = GameManager.Instance.playerObj.GetComponent<PlayerInteraction>().gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            //拿到玩家交互脚本
            PlayerInteraction pi = collision.gameObject.GetComponent<PlayerInteraction>();
            if (pi.currHealth < pi.playerInfo.Health)
            {
                //当残血时，吃鸡腿直接回血
                pi.currHealth += 3;
                if (pi.currHealth > pi.playerInfo.Health)
                {
                    pi.currHealth = pi.playerInfo.Health;
                }
                gamePanel.UpdateBloodBar(pi.currHealth, pi.playerInfo.Health);
            }
            else
            {
                //当不是满血时，吃鸡腿进入收集物数量
                gamePanel.UpdateCollections(0, 0, 0, 1);
            }
        }

    }
    #endregion
}
