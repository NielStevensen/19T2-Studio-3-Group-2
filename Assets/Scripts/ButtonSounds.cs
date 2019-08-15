using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour
{
    public AudioClip MouseOver;
    public AudioClip MouseClick;
    public AudioSource SFXSource;
    private EventSystem Event;

    private void Start()
    {
        Event = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void OnMouseEnter()
    {
        SFXSource.PlayOneShot(MouseOver);
    }

    public void OnMouseDown()
    {
        SFXSource.PlayOneShot(MouseClick);
        Event.SetSelectedGameObject(null);
    }
}
