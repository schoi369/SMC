using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonsManager : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("_Level1");
    }

    public void LoadCredits() {
        SceneManager.LoadScene("_Credits");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
