using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    public void MoveUp() {
        transform.position += new Vector3(0, 0.1f, 0);
    }

    public void MoveDown() {
        transform.position += new Vector3(0, -0.1f, 0);
    }
}
