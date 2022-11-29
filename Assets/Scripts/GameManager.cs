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

    public bool GameEnded { get; private set; } = false;

    int currentPoints = 2;
    int currentLevel = 1;

    PointsController pointsController;

    void Start() {
        
        // Play the song
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.clip = Globals.CurrentSong;
        audioSource.Play();

        pointsController = FindObjectOfType<PointsController>();

        pointsController.SpawnPoints(currentPoints);
    }

    void Update() {

        if (GameEnded && Input.GetKeyDown(KeyCode.Return) && enterName.activeSelf) {
            EnterScore();
            return;
        }

        // OJO TEMPORAL
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<ScoreManager>().ClearScores();
        }
    }

    public void NextLevel() {
        
        if (GameEnded) return;

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

        if (score < 1.7f) return;

        GetComponent<TimerManager>().AddTime(score);

        GetComponent<ScoreManager>().AddScore((int)(score * Mathf.Pow(currentPoints, 2) * 100));

        if (currentLevel == 1 || currentLevel % 2 == 1) currentPoints++;

        currentLevel++;
        pointsController.DespawnPoints();
        currentPoints = Mathf.Min(currentPoints, 6);
        pointsController.SpawnPoints(currentPoints);
    }

    public void FinishRound() {

        GameEnded = true;
        var scoreManager = GetComponent<ScoreManager>();

        if (scoreManager.CheckIfHighScore()) {
            enterName.SetActive(true);
            enterName.GetComponentInChildren<TMP_InputField>().Select();
        }
        else scoreManager.ShowScoreList();
    }

    public void EnterScore() {
            
        var scoreManager = GetComponent<ScoreManager>();
        scoreManager.SaveScore();
        enterName.SetActive(false);
        scoreManager.ShowScoreList();
    }
}
