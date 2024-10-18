using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;  // UI element to display score
    private int totalScore = 0;

    public void UpdateScore(int score)
    {
        totalScore += score;
        scoreText.text = "Score: " + totalScore;
        Debug.Log("Total Score: " + totalScore);
    }
}

