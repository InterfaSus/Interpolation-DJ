using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    
    public GameObject slidersL;
    public GameObject slidersR;

    List<Slider> sliders;
    int[] order;

    void Awake() {

        sliders = new List<Slider>();
        sliders.AddRange(slidersL.GetComponentsInChildren<Slider>());
        sliders.AddRange(slidersR.GetComponentsInChildren<Slider>());

        // // Rearragne list in the order 0, 5, 1, 4, 2, 3
        // Slider temp = sliders[1];
        // sliders[1] = sliders[5];
        // sliders[5] = temp;
        // temp = sliders[2];
        // sliders[2] = sliders[4];
        // sliders[4] = temp;

    }

    public void StartLevel(int n) {

        List<int> result = new List<int>();

        for (int i = 0; i < (n + 1) / 2; i++) {
            result.Add(i);
        }
        for (int i = 6 - n / 2; i < 6; i++) {
            result.Add(i);
        }

        order = result.ToArray();
    }

    public void UpdateSliders(Vector3[] points) {

        for (int i = 0; i < points.Length; i++) {
            sliders[order[i]].value = 1.0f / (1.0f + Mathf.Exp(-points[i].y));
        }
    }
}
