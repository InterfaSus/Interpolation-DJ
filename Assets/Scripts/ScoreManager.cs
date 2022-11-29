using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TMP_InputField nameInput;
    public GameObject scoresObject;
    public GameObject scoresObjectList;

    int score = 0;
    List<Tuple<string, int>> scores;

    void Start() {

        scores = new List<Tuple<string, int>>();
        ResetScore();
    }

    public void AddScore(int points) {
        
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void ResetScore() {
        
        score = 0;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    // Load scores from player prefs
    public void LoadScores() {
        
        scores.Clear();
        int count = PlayerPrefs.GetInt("scoreCount", 0);

        for (int i = 0; i < count; i++) {
            
            string name = PlayerPrefs.GetString("ScoreName" + i, "");
            int score = PlayerPrefs.GetInt("Score" + i, 0);
            scores.Add(new Tuple<string, int>(name, score));
        }
    }

    public bool CheckIfHighScore() {
        
        LoadScores();

        int pos = 1;
        foreach (var item in scores) {
            if (score > item.Item2) {
                return true;
            }
            pos++;
        }
        return pos <= 5;
    }

    public void SaveScore() {
        
        name = nameInput.text;
        if (name == "") name = "Anon";

        scores.Add(new Tuple<string, int>(name, score));
        scores.Sort((x, y) => y.Item2.CompareTo(x.Item2));

        // If there are more than 5 scores, remove the last one
        if (scores.Count > 5) {
            scores.RemoveAt(scores.Count - 1);
        }

        PlayerPrefs.SetInt("scoreCount", scores.Count);
        for (int i = 0; i < scores.Count; i++) {
            
            PlayerPrefs.SetString("ScoreName" + i, scores[i].Item1);
            PlayerPrefs.SetInt("Score" + i, scores[i].Item2);
        }
    }

    public void ShowScoreList() {

        LoadScores();

        string text = "";
        int i = 0;
        foreach (var score in scores) {

            text += $"{i + 1}. {score.Item1} - {score.Item2}\n";
            i++;
        }
        
        scoresObject.SetActive(true);
        scoresObjectList.GetComponent<TextMeshProUGUI>().text = text;
    }
    public void ClearScores() {

        PlayerPrefs.DeleteAll();
    }
}
