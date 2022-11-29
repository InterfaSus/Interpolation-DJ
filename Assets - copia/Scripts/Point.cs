using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    float yMoveLimit;

    void Start() {

        yMoveLimit = FindObjectOfType<PointsController>().yMoveLimit;
    }

    public void MoveUp() {

        float newY = Mathf.Min(transform.position.y + 0.1f, yMoveLimit);
        transform.position = new Vector3(transform.position.x, newY, 0);
    }

    public void MoveDown() {

        float newY = Mathf.Max(transform.position.y - 0.1f, -yMoveLimit);
        transform.position = new Vector3(transform.position.x, newY, 0);
    }
}
