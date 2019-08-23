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

        if (SFX != null)
        {
            SFX.volume = Gamemanager.SFXVolume;
        }

        if (Music != null)
        {
            Music.volume = Gamemanager.MusicVolume;
        }

        Greyscale.Saturation = Gamemanager.Saturation;
        Greyscale.Contrast = Gamemanager.Contrast;
        Greyscale.Exposure = Gamemanager.Exposure;
    }
}
