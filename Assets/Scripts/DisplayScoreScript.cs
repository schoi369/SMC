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
    public Text Comment;

    void Start()
    {
        timeText.text = ScoreManagerScript.Instance.timeToString();
        healthText.text = ScoreManagerScript.Instance.healthToString();
        highestAlertLevel.text = ScoreManagerScript.Instance.highestAlertToString();
        TotalScore.text = ScoreManagerScript.Instance.calculateTotalScore().ToString();
        Rank.text = ScoreManagerScript.Instance.calculateRank();
        Comment.text = ScoreManagerScript.Instance.chooseComment();

    }
}
