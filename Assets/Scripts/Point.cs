using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    public SpriteRenderer circle;
    public bool isMobile;
    public float toggleError = 0.5f;

    float yMoveLimit;
    GraphPolinomial graph;

    void Start() {

        yMoveLimit = FindObjectOfType<PointsController>().yMoveLimit;
        graph = FindObjectOfType<GraphPolinomial>();
    }

    void Update() {

        if (!isMobile) circle.color = Mathf.Abs(graph.CalculatePoints(transform.position.x).y - transform.position.y) < toggleError
            ? Color.green : Color.red;
    }

    public void MoveUp() {

        float newY = Mathf.Min(transform.position.y + 0.1f, yMoveLimit);
        transform.position = new Vector3(transform.position.x, newY, 0);
    }

    public void MoveDown() {

        float newY = Mathf.Max(transform.position.y - 0.1f, -yMoveLimit);
        transform.position = new Vector3(transform.position.x, newY, 0);
    }

    public void SetPassing(bool passing) {

        circle.color = passing ? Color.green : Color.red;
    }
}
