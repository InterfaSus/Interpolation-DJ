using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   

    public GameObject endLevelText;
    public GameObject pressEnterText;

    public bool LevelEnded { get; private set; } = false;
    int currentPoints = 2;
    PointsController pointsController;

    void Start() {
        
        pointsController = FindObjectOfType<PointsController>();

        pointsController.SpawnPoints(currentPoints);
    }

    void Update() {

        if (LevelEnded && Input.GetKeyDown(KeyCode.Space)) {
            
            endLevelText.SetActive(false);
            pressEnterText.SetActive(true);
            LevelEnded = false;

            pointsController.DespawnPoints();
            currentPoints = Mathf.Min(currentPoints + 1, 6);
            pointsController.SpawnPoints(currentPoints);
            GetComponent<TimerManager>().StartTimer();
        }
    }

    public void NextLevel() {
        
        GetComponent<TimerManager>().StopTimer();

        var pointsController = FindObjectOfType<PointsController>();
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
        // Score is arc cotangent of (error - 3)
        float score = Mathf.Atan(1 / (error - 3)) + 1;
        if (error < 3) score += Mathf.PI;

        GetComponent<ScoreManager>().AddScore((int)(score * Mathf.Pow(currentPoints, 2) * 100));

        endLevelText.SetActive(true);
        pressEnterText.SetActive(false);
        LevelEnded = true;
    }
}
