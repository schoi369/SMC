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
    public bool isInSightOrVision;

    public Text alertLevelText;

    // Start is called before the first frame update
    void Start()
    {
        isInSightOrVision = false;
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
        updateText();
    }

    public void decreaseAlertPercentage() {
        if (currentAlertPercentage > minAlertPercentage && !isInSightOrVision) {
            currentAlertPercentage = currentAlertPercentage - 1;
        }
    }

    // testing purpose
    public void increaseAlertPercentage(int amount) {
        if (currentAlertPercentage < maxAlertPercentage) {
            currentAlertPercentage = currentAlertPercentage + amount;
        }
    }

    void updateText()
    {
        alertLevelText.text = currentAlertPercentage.ToString() + "%";
    }
}
