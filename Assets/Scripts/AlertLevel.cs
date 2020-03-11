using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertLevel : MonoBehaviour
{ 
    public int currentAlertPercentage;
    public int minAlertPercentage = 0;
    public int maxAlertPercentage = 100;
    public bool isDetected;    

    public Text alertLevelText;

    private int detectionCounter;

    // Start is called before the first frame update
    void Start()
    {
        detectionCounter = 0;
        currentAlertPercentage = minAlertPercentage;

        InvokeRepeating("decreaseAlertPercentage", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            increaseAlertPercentage(10);
        }
        //updateText();
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
