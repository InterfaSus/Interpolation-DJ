using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{

    public GameObject mobilePointPrefab;
    public GameObject staticPointPrefab;
    public float distance;
    public float yMoveLimit = 4.5f;

    GameObject[] mobilePoints;
    GameObject[] staticPoints;
    KeyCode[] validUpKeys;
    KeyCode[] validDownKeys;

    public void SpawnPoints(int numPoints) {

        mobilePoints = new GameObject[numPoints];
        staticPoints = new GameObject[numPoints];

        // Spawn points in a row centered on the screen with the distance between them being proportionally inversely related to the number of points
        float distanceBetweenPoints = distance / (numPoints - 1);
        for (int i = 0; i < numPoints; i++) {

            float y = Random.Range(-4.5f, 4.5f);
            mobilePoints[i] = Instantiate(mobilePointPrefab, new Vector3(i * distanceBetweenPoints - distance / 2, y, 0), Quaternion.identity);
        }

        // Spawn static points between the mobile points
        for (int i = 0; i < numPoints - 1; i++) {

            float y = Random.Range(-4.0f, 4.0f);
            staticPoints[i] = Instantiate(staticPointPrefab, new Vector3((i + 0.5f) * distanceBetweenPoints - distance / 2, y, 0), Quaternion.identity);
        }

        validUpKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.U, KeyCode.I, KeyCode.O};
        validDownKeys = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K, KeyCode.L};
        System.Array.Resize(ref validUpKeys, mobilePoints.Length);
        System.Array.Resize(ref validDownKeys, mobilePoints.Length);
    }

    public void DespawnPoints() {

        // Destroy points
        for (int i = 0; i < mobilePoints.Length; i++) {
            Destroy(mobilePoints[i]);
        }
        for (int i = 0; i < staticPoints.Length; i++) {
            Destroy(staticPoints[i]);
        }
    }

    void Update() {
        
        for (int i = 0; i < mobilePoints.Length; i++) {
            if (Input.GetKey(validUpKeys[i])) {
                mobilePoints[i].GetComponent<Point>().MoveUp();
            }
            if (Input.GetKey(validDownKeys[i])) {
                mobilePoints[i].GetComponent<Point>().MoveDown();
            }
        }

        // If N is pressed, go to next level
        if (Input.GetKeyDown(KeyCode.N)) {
            FindObjectOfType<GameManager>().NextLevel();
        }
    }
}
