using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Load());
        }
    }

    //�л�����Э��
    IEnumerator Load()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        else
            yield return SceneManager.LoadSceneAsync(0);
    }
}
