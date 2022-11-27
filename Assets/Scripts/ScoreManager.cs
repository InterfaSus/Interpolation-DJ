using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    int score = 0;

    void Start() {
        
        ResetScore();
    }

    public void AddScore(int points) {
        
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void ResetScore() {
        
        score = 0;
        scoreText.text = "Score: " + score;
    }
}
