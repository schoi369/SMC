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
    public string rank;

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
        if (t <= 60) {
            totalScore += 13000;
        } else if (t > 60 && t <= 120) {
            totalScore += 9000;
        } else if (t > 120 && t <= 180) {
            totalScore += 7000;
        } else if (t > 180 && t <= 240) {
            totalScore += 5000;
        } else if (t > 240) {
            totalScore += 3000;
        }
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

    void calculateAlertScore() {
        if (highestAlertPercentage <= 20) {
            totalScore += 13000;
        } else if (highestAlertPercentage > 20 && highestAlertPercentage <= 40) {
            totalScore += 9000;
        } else if (highestAlertPercentage > 40 && highestAlertPercentage <= 60) {
            totalScore += 7000;
        } else if (highestAlertPercentage > 60 && highestAlertPercentage <= 80) {
            totalScore += 5000;
        } else if (highestAlertPercentage > 80) {
            totalScore += 3000;
        }
    }

    public int calculateTotalScore() {
        calculateTimeScore();
        calculateHealthScore();
        calculateAlertScore();
        return totalScore;
    }

    public string calculateRank() {
        if (totalScore >= 35000) {
            rank = "S";
        } else if (totalScore < 35000 && totalScore >= 25000) {
            rank = "A";
        } else if (totalScore < 25000 && totalScore >= 19000) {
            rank = "B";
        } else if (totalScore < 19000 && totalScore >= 13000) {
            rank = "C";
        } else if (totalScore < 13000) {
            rank = "D";
        }
        return rank;
    }

}
