using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    public SpriteRenderer circle;
    public bool isMobile;
    public float toggleError = 0.5f;
    public float speed = 1.5f;
    public Animation turnOn;
    public Animation turnOff;

    float yMoveLimit;
    GraphPolinomial graph;
    bool isClose = false;

    void Start() {

        yMoveLimit = FindObjectOfType<PointsController>().yMoveLimit;
        graph = FindObjectOfType<GraphPolinomial>();
    }

    void Update() {

        // Spin the point
        transform.Rotate(0, 0, 100 * Time.deltaTime);

        if (!isMobile) 
            TogglePoint(Mathf.Abs(graph.CalculatePoints(transform.position.x).y - transform.position.y) < toggleError);
    }

    public void TogglePoint(bool toggle) {

        circle.color = toggle ? Color.green : Color.red;

        if (!isClose && toggle) turnOn.Play("RingOut");
        else if (isClose && !toggle) turnOff.Play("TurnOff");

        isClose = toggle;
    }

    public void MoveUp() {

        float newY = Mathf.Min(transform.position.y + 0.1f * speed, yMoveLimit);
        transform.position = new Vector3(transform.position.x, newY, 0);
    }

    public void MoveDown() {

        float newY = Mathf.Max(transform.position.y - 0.1f * speed, -yMoveLimit);
        transform.position = new Vector3(transform.position.x, newY, 0);
    }

    public void SetPassing(bool passing) {

        circle.color = passing ? Color.green : Color.red;
    }
}
