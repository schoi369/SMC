using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreScript : MonoBehaviour
{
    public Text timeText;
    public Text healthText;
    public Text highestAlertLevel;
    public Text TotalScore;
    public Text Rank;

    void Start()
    {
        timeText.text = ScoreManagerScript.Instance.timeToString();
        healthText.text = ScoreManagerScript.Instance.healthToString();
        highestAlertLevel.text = ScoreManagerScript.Instance.highestAlertToString();
    }
}
