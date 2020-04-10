﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodEndScript : MonoBehaviour
{
    private GameObject go;
    public void LoadMainMenuFromResult() {
        go = GameObject.Find("ScoreManager");
        Destroy(go);
        BasicMovement.currentHealth = 100;
        AlertLevel.currentAlertPercentage = 0;
        SceneManager.LoadScene("Main Menu");
    }
}