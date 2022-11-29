using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumController : MonoBehaviour
{
    public float scale = 100;
    public float baseHeight = 1;
    public int visible = 128;
    public float observe = 2;

    public AudioSource audioSource;
    public GameObject cubePrefab;

    public float[] signals;
    public GameObject[] objectLeft;
    public GameObject[] objectRight;
    public Vector3 localScale;
    public float[] lastScales;

    private void Start()
    {
        signals = new float[visible];
        objectLeft = new GameObject[visible];
        objectRight = new GameObject[visible];
        lastScales = new float[visible];

        for (int i = 0; i < visible; i++)
        {
            objectLeft[i] = Instantiate(cubePrefab, transform);
            objectRight[i] = Instantiate(cubePrefab, transform);
            objectLeft[i].transform.localPosition = new Vector3(-i, 0, 0);
            objectRight[i].transform.localPosition = new Vector3(i, 0, 0);
        }
    }

    private void Update()
    {
        audioSource.GetSpectrumData(signals, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < visible; i++)
        {
            float scale = signals[i] * this.scale + baseHeight;
            objectLeft[i].transform.localScale = new Vector3(1, scale, 1);
            objectRight[i].transform.localScale = new Vector3(1, scale, 1);
        }


    }
}
