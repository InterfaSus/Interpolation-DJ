using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   

    int currentPoints = 2;
    PointsController pointsController;

    void Start() {
        
        pointsController = FindObjectOfType<PointsController>();

        pointsController.SpawnPoints(currentPoints);
    }

    public void NextLevel() {

        GetComponent<ScoreManager>().AddScore(currentPoints * 100);

        pointsController.DespawnPoints();
        currentPoints = Mathf.Min(currentPoints + 1, 6);
        pointsController.SpawnPoints(currentPoints);
    }
}
