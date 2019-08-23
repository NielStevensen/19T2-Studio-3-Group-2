using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSettings : MonoBehaviour
{
    public GameManager Gamemanager = new GameManager();
    public GreyScale Greyscale;
    public AudioSource SFX;
    public AudioSource Music;

    void Start()
    {
        Gamemanager = JsonUtility.FromJson<GameManager>(File.ReadAllText(Application.persistentDataPath + "/GameSettings.json"));

        SFX.volume = Gamemanager.SFXVolume;
        Music.volume = Gamemanager.MusicVolume;
        Greyscale.Saturation = Gamemanager.Saturation;
        Greyscale.Contrast = Gamemanager.Contrast;
        Greyscale.Exposure = Gamemanager.Exposure;
    }
}
