using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadColourGrading : MonoBehaviour
{
    public GameManager Gamemanager = new GameManager();
    public GreyScale Greyscale;
    void Start()
    {
        Gamemanager = JsonUtility.FromJson<GameManager>(File.ReadAllText(Application.persistentDataPath + "/GameSettings.json"));

        Greyscale.Saturation = Gamemanager.Saturation;
        Greyscale.Contrast = Gamemanager.Contrast;
        Greyscale.Exposure = Gamemanager.Exposure;
    }
}
