using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    
    public GameObject slidersL;
    public GameObject slidersR;

    List<Slider> sliders;

    void Start() {

        sliders = new List<Slider>();
        sliders.AddRange(slidersL.GetComponentsInChildren<Slider>());
        sliders.AddRange(slidersR.GetComponentsInChildren<Slider>());
    }

    public void UpdateSliders(Vector3[] points) {

        for (int i = 0; i < points.Length; i++) {
            sliders[i].value = 1.0f / (1.0f + Mathf.Exp(-points[i].y));
        }
    }
}
