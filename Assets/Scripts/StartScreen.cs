using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    void OnGUI() //attached to listener game object
    {
        if (Input.anyKeyDown)
		{
            SceneManager.LoadScene("BasicMovementandAnim");
		}
    }
}
