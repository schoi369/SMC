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

    public void LoadTutorial1() {
        SceneManager.LoadScene("Tutorial 1");
    }

    public void LoadTutorial2() {
        SceneManager.LoadScene("Tutorial 2");
    }

    public void LoadTutorial3() {
        SceneManager.LoadScene("Tutorial 3");
    }

    public void LoadTutorial4() {
        SceneManager.LoadScene("Tutorial 4");
    }

    public void LoadTutorial5() {
        SceneManager.LoadScene("Tutorial 5");
    }

    public void LoadTutorial6() {
        SceneManager.LoadScene("Tutorial 6");
    }
}
