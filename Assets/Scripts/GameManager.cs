using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{   

    public GameObject enterName;
    public TextMeshProUGUI countdownText;

    public bool GameEnded { get; private set; } = false;

    int currentPoints = 2;
    int currentLevel = 1;

    PointsController pointsController;

    void Start() {
        
        // Play the song
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.clip = Globals.CurrentSong;
        audioSource.Play();

        if (Globals.lastSongName == Globals.CurrentSong.name) {
            audioSource.time = Globals.lastSongTime;
        }
        Globals.lastSongName = Globals.CurrentSong.name;

        pointsController = FindObjectOfType<PointsController>();

        // Start a coroutine to count down
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {
        // Count down from 3
        for (int i = 3; i > 0; i--) {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        // Start the game
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1);
        countdownText.text = "";

        // Start the points controller
        pointsController.SpawnPoints(currentPoints);
        GetComponent<TimerManager>().StartTimer();
    }

    void Update() {

        if (GameEnded && Input.GetKeyDown(KeyCode.Return) && enterName.activeSelf) {
            EnterScore();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<SceneControl>().LoadScene("SongSelection");
        }

        // If Ctrl + A is pressed, clear scores
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B)) {
            GetComponent<ScoreManager>().ClearScores();
        }

        Globals.lastSongTime = FindObjectOfType<AudioSource>().time;
    }

    public void NextLevel() {
        
        if (GameEnded) return;

        var graphPolinomial = FindObjectOfType<GraphPolinomial>();

        Vector3[] mobilePoints = pointsController.MobilePoints;
        Vector3[] staticPoints = pointsController.StaticPoints;

        // Calculate mean square error
        float error = 0;
        foreach (var point in staticPoints) {
            error += Mathf.Pow(point.y - graphPolinomial.CalculatePoints(point.x).y, 2);
        }
        error /= staticPoints.Length;
        // Score is arc cotangent of (error - 3) + 1
        float score = Mathf.Atan(1 / (error - 3)) + 1;
        if (error < 3) score += Mathf.PI;

        if (score < 1.7f) return;

        GetComponent<TimerManager>().AddTime(score);

        GetComponent<ScoreManager>().AddScore((int)(score * Mathf.Pow(currentPoints, 2) * 100));

        if (currentLevel == 1 || currentLevel % 2 == 1) currentPoints++;

        // Do a screen shake
        StartCoroutine(Shake(0.2f, 0.2f));

        currentLevel++;
        pointsController.DespawnPoints();
        currentPoints = Mathf.Min(currentPoints, 6);
        pointsController.SpawnPoints(currentPoints);
    }
    
    public void FinishRound() {

        GameEnded = true;
        var scoreManager = GetComponent<ScoreManager>();

        if (scoreManager.CheckIfHighScore()) {
            enterName.SetActive(true);
            enterName.GetComponentInChildren<TMP_InputField>().Select();
        }
        else scoreManager.ShowScoreList();
    }

    public void EnterScore() {
            
        var scoreManager = GetComponent<ScoreManager>();
        scoreManager.SaveScore();
        enterName.SetActive(false);
        scoreManager.ShowScoreList();
    }

    IEnumerator Shake(float duration, float magnitude) {
        
        Vector3 originalPos = Camera.main.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration) {

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}
