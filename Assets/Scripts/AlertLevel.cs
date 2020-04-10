using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertLevel : MonoBehaviour
{ 
    public static int currentAlertPercentage;
    private static bool alertLevelInitialized = false;

    public int minAlertPercentage = 0;
    public int maxAlertPercentage = 100;

    public Text alertLevelText;

    private int detectionCounter;

    public int highestAlertPercentage = 0;

    // Start is called before the first frame update
    void Start()
    {
        detectionCounter = 0;
        if (!alertLevelInitialized)
        {
            currentAlertPercentage = minAlertPercentage;
            alertLevelInitialized = true;
        }
        updateText();
        InvokeRepeating("decreaseAlertPercentage", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     increaseAlertPercentage(10);
        // }
        //updateText();
        if (currentAlertPercentage > highestAlertPercentage) {
            highestAlertPercentage = currentAlertPercentage;
            ScoreManagerScript.Instance.highestAlertPercentage = highestAlertPercentage;
        }
    }

    private void decreaseAlertPercentage() {
        if (currentAlertPercentage > minAlertPercentage && detectionCounter == 0) {
            currentAlertPercentage = currentAlertPercentage - 1;
            updateText();
        }
    }

    // testing purpose
    public void increaseAlertPercentage(int amount) {
        if (currentAlertPercentage < maxAlertPercentage) {
            currentAlertPercentage = Mathf.Min(currentAlertPercentage + amount, 100);
            updateText();
        }
    }

    public void incDetectionCount()
    {
        detectionCounter++;
    }

    public void decDetectionCount()
    {
        detectionCounter--;
    }

    private void updateText()
    {
        alertLevelText.text = currentAlertPercentage.ToString() + "%";
    }
}
