using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    炸弹脚本：
        拾取前，出现在地图上，当玩家碰到物体时，摧毁自身，并将玩家身上炸弹数加一。
        拾取后，身上炸弹数减一，在玩家处释放炸弹（排除玩家与炸弹碰撞体），一定秒数后爆炸，对周围敌人和玩家造成伤害，并摧毁一些地形。
        拾取前作为地图的预设出现在地图上
 */
public class Bomb_Unpicked : MonoBehaviour
{
    private GamePanel gamePanel;

    #region 碰撞器相关
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //拾取逻辑
        gamePanel = GameManager.Instance.playerObj.GetComponent<PlayerInteraction>().gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            gamePanel.UpdateCollections(1);
            Destroy(gameObject);
        }
       
    }
    #endregion

    #region 动画事件
    public void Boom()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 2f);

        foreach (Collider2D c in coll)
        {
            if (c.CompareTag("Enemy") )
            {
                FSM fsm = c.GetComponent<FSM>();
                fsm.parameter.getHit = true;
                c.GetComponent<FSM>().Hit(5);
            }
            if (c.CompareTag("Player"))
            {
                c.GetComponent<PlayerInteraction>().Wound(5);
            }
            //TODO:摧毁部分地形
            if (c.CompareTag("Explode"))
            {
                Destroy(c.gameObject);
            }

            Destroy(gameObject, 0.3f);
        }
    }
    #endregion
}
