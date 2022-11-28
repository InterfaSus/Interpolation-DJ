using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SongSelection : MonoBehaviour
{

    public GameObject elementPrefab;
    public GameObject container;

    void Start() {
        
        // Add every song name in the Resources/Songs folder to the list
        foreach (var song in Resources.LoadAll<AudioClip>("Songs")) {
            
            if (song.name == "default") continue;

            var button = Instantiate(elementPrefab, container.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = song.name;

            // Add a listener to the button that will call the StartLevel function in the GameManager
            button.GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<SongSelection>().StartLevel(song));
        }
    }

    public void StartLevel(AudioClip song) {
        
        Globals.CurrentSong = song;
        // Load the game scene
        SceneManager.LoadScene("PlayScene");
    }
}
