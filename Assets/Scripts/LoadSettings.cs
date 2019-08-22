using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSettings : MonoBehaviour
{
    public GameManager Gamemanager = new GameManager();
    public GreyScale Greyscale;
    private AudioSource SFX;
    private AudioSource Music;

    void Start()
    {
        if (GameObject.Find("SFXSource") != null)
        {
            SFX = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        }

        if (GameObject.Find("MusicSource") != null)
        {
            Music = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        }

        Load();
    }

    private void Load()
    {
        Gamemanager = JsonUtility.FromJson<GameManager>(File.ReadAllText(Application.persistentDataPath + "/GameSettings.json"));

        SFX.volume = Gamemanager.SFXVolume;
        Music.volume = Gamemanager.MusicVolume;
        Greyscale.Saturation = Gamemanager.Saturation;
        Greyscale.Contrast = Gamemanager.Contrast;
        Greyscale.Exposure = Gamemanager.Exposure;
    }
}
