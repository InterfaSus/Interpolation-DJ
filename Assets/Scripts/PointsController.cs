using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointsController : MonoBehaviour
{

    public GameObject mobilePointPrefab;
    public GameObject staticPointPrefab;
    public float distance;
    public float yMoveLimit = 4.5f;

    public Vector3[] MobilePoints { get => mobilePoints.Select(point => point.transform.position).ToArray(); }
    public Vector3[] StaticPoints { get => staticPoints.Select(point => point.transform.position).ToArray(); }

    GameManager manager;
    SlidersController slidersController;
    GameObject[] mobilePoints;
    GameObject[] staticPoints;
    KeyCode[] generalUpKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.U, KeyCode.I, KeyCode.O};
    KeyCode[] generalDownKeys = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K, KeyCode.L};
    KeyCode[] validUpKeys;
    KeyCode[] validDownKeys;
    int[] keysOrder = new int[] { 3, 2, 4, 1 };

    void Start() {

        manager = FindObjectOfType<GameManager>();
        slidersController = FindObjectOfType<SlidersController>();
    }

    public void SpawnPoints(int numPoints) {

        mobilePoints = new GameObject[numPoints];
        staticPoints = new GameObject[numPoints - 1];

        // Spawn points in a row centered on the screen with the distance between them being proportionally inversely related to the number of points
        float distanceBetweenPoints = distance / (numPoints - 1);
        for (int i = 0; i < numPoints; i++) {

            float y = UnityEngine.Random.Range(-yMoveLimit, yMoveLimit);
            mobilePoints[i] = Instantiate(mobilePointPrefab, new Vector3(i * distanceBetweenPoints - distance / 2, y, 0), Quaternion.identity, transform);
        }

        GetComponentInChildren<GraphPolinomial>().points = mobilePoints.Select(point => point.transform.position).ToArray();

        // Spawn static points between the mobile points
        for (int i = 0; i < numPoints - 1; i++) {

            float y = UnityEngine.Random.Range(-yMoveLimit * 7.0f / 9.0f, yMoveLimit * 7.0f / 9.0f);
            staticPoints[i] = Instantiate(staticPointPrefab, new Vector3((i + 0.5f) * distanceBetweenPoints - distance / 2, y, 0), Quaternion.identity, transform);
        }

        validUpKeys = FilterKeys(generalUpKeys, numPoints);
        validDownKeys = FilterKeys(generalDownKeys, numPoints);

        slidersController.StartLevel(numPoints);
        slidersController.UpdateSliders(MobilePoints);
    }

    private KeyCode[] FilterKeys(KeyCode[] keys, int n) {

        List<KeyCode> result = new List<KeyCode>();
        List<int> order = new List<int>();

        for (int i = 0; i < (n + 1) / 2; i++) {
            result.Add(keys[i]);
        }
        for (int i = 6 - n / 2; i < 6; i++) {
            result.Add(keys[i]);
        }

        return result.ToArray();
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
        
        // If Enter is pressed, next level
        if (Input.GetKeyDown(KeyCode.Return)) {
            manager.NextLevel();
        }

        if (manager.GameEnded) return;
        bool movement = false;
        for (int i = 0; i < mobilePoints.Length; i++) {
            if (Input.GetKey(validUpKeys[i])) {
                movement = true;
                mobilePoints[i].GetComponent<Point>().MoveUp();
            }
            if (Input.GetKey(validDownKeys[i])) {
                movement = true;
                mobilePoints[i].GetComponent<Point>().MoveDown();
            }
        }

        if (movement) {
            GetComponentInChildren<GraphPolinomial>().points = MobilePoints;
            slidersController.UpdateSliders(MobilePoints);
        }
    }
}
