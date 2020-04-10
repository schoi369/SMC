using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript Instance { get; private set; }

    public float startTime;
    public float t = 0;
    public string minutes;
    public string seconds;
    public float leftHealth;
    public int highestAlertPercentage;
    public int totalScore = 0;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        startTime = Time.time;
    }

    void Update() {
        t = Time.time - startTime;
    }

    public string timeToString() {
        minutes = ((int) t / 60).ToString();
        seconds = (t % 60).ToString("f0");
        return "Elapsed Time: " + minutes + ":" + seconds;
    }

    void calculateTimeScore() {
        
    }

    public string healthToString() {
        return "Remaining Chocolate Health: " + leftHealth.ToString();
    }

    void calculateHealthScore() {
        if (leftHealth >= 90) {
            totalScore += 13000;
        } else if (leftHealth < 90 && leftHealth >= 70) {
            totalScore += 9000;
        } else if (leftHealth < 70 && leftHealth >= 50) {
            totalScore += 7000;
        } else if (leftHealth < 50 && leftHealth >= 30) {
            totalScore += 5000;
        } else if (leftHealth < 30) {
            totalScore += 3000;
        }
    }

    public string highestAlertToString() {
        return "Highest Alert % Reached: " + highestAlertPercentage.ToString() + "%";
    }

}
