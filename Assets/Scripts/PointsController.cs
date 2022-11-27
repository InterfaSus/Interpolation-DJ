using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{

    public GameObject pointPrefab;
    public float distance;

    GameObject[] points;
    KeyCode[] validUpKeys;
    KeyCode[] validDownKeys;

    public void SpawnPoints(int numPoints) {

        points = new GameObject[numPoints];
        // Spawn points in a row centered on the screen with the distance between them being proportionally inversely related to the number of points
        float distanceBetweenPoints = distance / (numPoints - 1);
        for (int i = 0; i < numPoints; i++) {
            points[i] = Instantiate(pointPrefab, new Vector3(i * distanceBetweenPoints - distance / 2, 0, 0), Quaternion.identity);
        }

        validUpKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.U, KeyCode.I, KeyCode.O};
        validDownKeys = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K, KeyCode.L};
        System.Array.Resize(ref validUpKeys, points.Length);
        System.Array.Resize(ref validDownKeys, points.Length);
    }

    public void DespawnPoints() {
        // Destroy points
        for (int i = 0; i < points.Length; i++) {
            Destroy(points[i]);
        }
    }

    void Update() {
        
        for (int i = 0; i < points.Length; i++) {
            if (Input.GetKey(validUpKeys[i])) {
                points[i].GetComponent<Point>().MoveUp();
            }
            if (Input.GetKey(validDownKeys[i])) {
                points[i].GetComponent<Point>().MoveDown();
            }
        }

        // If N is pressed, go to next level
        if (Input.GetKeyDown(KeyCode.N)) {
            FindObjectOfType<GameManager>().NextLevel();
        }
    }
}
