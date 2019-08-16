using UnityEngine;

public class GameManager
{
    public bool colourgrading;
    public float Musicvolume;
    public float SFXvolume;
    public float Saturationvalue;
    public float Contrastvalue;
    public float Exposurevalue;

    public bool ColourGrading
    {
        get
        {
            return colourgrading;
        }

        set
        {
            colourgrading = value;
        }
    }

    public float MusicVolume
    {
        get
        {
            return Musicvolume;
        }

        set
        {
            Musicvolume = value;
        }
    }

    public float SFXVolume
    {
        get
        {
            return SFXvolume;
        }

        set
        {
            SFXvolume = value;
        }
    }

    public float Saturation
    {
        get
        {
            return Saturationvalue;
        }

        set
        {
            Saturationvalue = value;
        }
    }

    public float Contrast
    {
        get
        {
            return Contrastvalue;
        }

        set
        {
            Contrastvalue = value;
        }
    }

    public float Exposure
    {
        get
        {
            return Exposurevalue;
        }

        set
        {
            Exposurevalue = value;
        }
    }
}
