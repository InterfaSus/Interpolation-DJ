using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    public Slider timerSlider;

    void Start() {
        
        timerSlider.value = timerSlider.maxValue;
        StartTimer();
    }

    public void StartTimer() {
        
        StartCoroutine(Timer());
    }

    public void AddTime(float time) {

        time *= 3;
        timerSlider.value = Mathf.Min(timerSlider.value + time, timerSlider.maxValue);
    }

    IEnumerator Timer() {
        
        timerSlider.fillRect.gameObject.SetActive(true);

        while (timerSlider.value > 0) {

            timerSlider.value -= Time.deltaTime;
            yield return null;
        }

        // Deactivate the FillArea of the slider
        timerSlider.fillRect.gameObject.SetActive(false);

        GetComponent<GameManager>().FinishRound();
    }
}
