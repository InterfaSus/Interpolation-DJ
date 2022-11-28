using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{   

    public GameObject endLevelText;
    public GameObject pressEnterText;
    public GameObject enterName;
    public GameObject scoresObject;
    public GameObject scoresObjectList;

    public bool LevelEnded { get; private set; } = false;
    public bool RoundEnded { get; private set; } = false;

    int currentPoints = 2;
    int currentLevel = 1;

    PointsController pointsController;

    void Start() {
        
        pointsController = FindObjectOfType<PointsController>();

        pointsController.SpawnPoints(currentPoints);
    }

    void Update() {

        if (RoundEnded) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                EnterScore();
            }
            return;
        }
        if (LevelEnded && Input.GetKeyDown(KeyCode.Space)) {
            
            endLevelText.SetActive(false);

            currentLevel++;
            if (currentLevel == 6) {

                FinishRound();
                return;
            }

            LevelEnded = false;
            pressEnterText.SetActive(true);

            pointsController.DespawnPoints();
            currentPoints = Mathf.Min(currentPoints + 1, 6);
            pointsController.SpawnPoints(currentPoints);
            GetComponent<TimerManager>().StartTimer();
        }

        // OJO TEMPORAL
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<ScoreManager>().ClearScores();
        }
    }

    public void NextLevel() {
        
        if (LevelEnded) return;

        GetComponent<TimerManager>().StopTimer();

        var graphPolinomial = FindObjectOfType<GraphPolinomial>();

        Vector3[] mobilePoints = pointsController.MobilePoints;
        Vector3[] staticPoints = pointsController.StaticPoints;

        float[] polinomial = graphPolinomial.GetPolinomial(mobilePoints);

        // Calculate mean square error
        float error = 0;
        foreach (var point in staticPoints) {
            error += Mathf.Pow(point.y - graphPolinomial.CalculatePoints(point.x, polinomial).y, 2);
        }
        error /= staticPoints.Length;
        // Score is arc cotangent of (error - 3) + 1
        float score = Mathf.Atan(1 / (error - 3)) + 1;
        if (error < 3) score += Mathf.PI;

        GetComponent<ScoreManager>().AddScore((int)(score * Mathf.Pow(currentPoints, 2) * 100));

        endLevelText.SetActive(true);
        pressEnterText.SetActive(false);
        LevelEnded = true;
    }

    void FinishRound() {

        RoundEnded = true;
        var scoreManager = GetComponent<ScoreManager>();

        if (scoreManager.CheckIfHighScore()) {
            enterName.SetActive(true);
            enterName.GetComponentInChildren<TMP_InputField>().Select();
        }
        else ShowScoreList();
    }

    public void EnterScore() {
            
        GetComponent<ScoreManager>().SaveScore();
        enterName.SetActive(false);
        ShowScoreList();
    }

    void ShowScoreList() {
        
        var scoreManager = GetComponent<ScoreManager>();
        var scoreList = scoreManager.Scores;

        string text = "";
        int i = 0;
        foreach (var score in scoreList) {

            text += $"{i + 1}. {score.Item1} - {score.Item2}\n";
            i++;
        }
        
        scoresObject.SetActive(true);
        scoresObjectList.GetComponent<TextMeshProUGUI>().text = text;
    }
}
