using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioClip MouseOver;
    public AudioClip MouseClick;
    public AudioSource SFXSource;

    public void OnMouseEnter()
    {
        SFXSource.PlayOneShot(MouseOver, 50);
    }

    public void OnMouseDown()
    {
        SFXSource.PlayOneShot(MouseClick, 50);
    }
}
