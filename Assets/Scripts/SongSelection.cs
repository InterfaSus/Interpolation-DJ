using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongSelection : MonoBehaviour
{

    public GameObject elementPrefab;
    public GameObject container;

    void Start() {
        
        Globals.LoadSongs();

        // Add every song name in the Resources/Songs folder to the list
        foreach (var song in Globals.songs) {
            
            if (song.name == "default") continue;

            var button = Instantiate(elementPrefab, container.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = song.name;

            // Add a listener to the button that will call the StartLevel function in the GameManager
            button.GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<SongSelection>().StartLevel(song));
        }
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<SceneControl>().LoadScene("MainMenu");
        }
    }

    public void StartLevel(AudioClip song) {
        
        Globals.CurrentSong = song;
        // Load the game scene
        GetComponent<SceneControl>().LoadScene("PlayScene");
    }
}
