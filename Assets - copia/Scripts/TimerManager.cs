using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{

    public float timePerLevel = 20;
    public TextMeshProUGUI timerText;

    void Start() {
        
        StartTimer();
    }

    public void StartTimer() {
        
        StartCoroutine(Timer());
    }

    public void StopTimer() {
        
        StopAllCoroutines();
    }

    IEnumerator Timer() {
        
        float timeLeft = timePerLevel + 1;
        while (timeLeft > 0) {

            timeLeft -= Time.deltaTime;
            timerText.text = ((int)timeLeft).ToString();
            yield return null;
        }
        timerText.text = "0";
        FindObjectOfType<GameManager>().NextLevel();
    }
}
