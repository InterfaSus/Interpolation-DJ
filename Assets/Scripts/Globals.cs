using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals {

    public static AudioClip CurrentSong = Resources.Load<AudioClip>("Songs/default");

    public static List<AudioClip> songs;
    public static float menuSongTime = 0;
    public static string lastSongName = "";
    public static float lastSongTime = 0;

    public static void LoadSongs() {

        if (songs != null) return;

        songs = new List<AudioClip>();
        foreach (var song in Resources.LoadAll<AudioClip>("Songs")) {
            if (song.name == "default") continue;
            songs.Add(song);
        }
    }
}