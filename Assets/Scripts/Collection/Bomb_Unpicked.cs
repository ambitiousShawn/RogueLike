using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ը���ű���
        ʰȡǰ�������ڵ�ͼ�ϣ��������������ʱ���ݻ����������������ը������һ��
        ʰȡ������ը������һ������Ҵ��ͷ�ը�����ų������ը����ײ�壩��һ��������ը������Χ���˺��������˺������ݻ�һЩ���Ρ�
        ʰȡǰ��Ϊ��ͼ��Ԥ������ڵ�ͼ��
 */
public class Bomb_Unpicked : MonoBehaviour
{
    private GamePanel gamePanel;

    #region ��ײ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //ʰȡ�߼�
        gamePanel = GameManager.Instance.playerObj.GetComponent<PlayerInteraction>().gamePanel;
        if (collision.gameObject.CompareTag("Player"))
        {
            gamePanel.UpdateCollections(1);
            Destroy(gameObject);
        }
       
    }
    #endregion

    #region �����¼�
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
            //TODO:�ݻٲ��ֵ���
            if (c.CompareTag("Explode"))
            {
                Destroy(c.gameObject);
            }

            Destroy(gameObject, 0.3f);
        }
    }
    #endregion
}
