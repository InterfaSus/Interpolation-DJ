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
    KeyCode[] validUpKeys;
    KeyCode[] validDownKeys;

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

            float y = Random.Range(-4.5f, 4.5f);
            mobilePoints[i] = Instantiate(mobilePointPrefab, new Vector3(i * distanceBetweenPoints - distance / 2, y, 0), Quaternion.identity, transform);
        }

        GetComponentInChildren<GraphPolinomial>().points = mobilePoints.Select(point => point.transform.position).ToArray();

        // Spawn static points between the mobile points
        for (int i = 0; i < numPoints - 1; i++) {

            float y = Random.Range(-4.0f, 4.0f);
            staticPoints[i] = Instantiate(staticPointPrefab, new Vector3((i + 0.5f) * distanceBetweenPoints - distance / 2, y, 0), Quaternion.identity, transform);
        }

        validUpKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.U, KeyCode.I, KeyCode.O};
        validDownKeys = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K, KeyCode.L};
        System.Array.Resize(ref validUpKeys, mobilePoints.Length);
        System.Array.Resize(ref validDownKeys, mobilePoints.Length);

        slidersController.UpdateSliders(MobilePoints);
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

        if (manager.LevelEnded) return;
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
