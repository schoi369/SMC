using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadResult : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene("GoodEnd", LoadSceneMode.Single);
        }
    }
}
