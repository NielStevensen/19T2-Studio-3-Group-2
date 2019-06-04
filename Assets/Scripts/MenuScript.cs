using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public InputField NameField;
    public Toggle ColourGrading;
    public Slider MusicVolume;
    public Slider SFXVolume;
    public Slider SaturationLevel;
    public Slider ContrastLevel;
    public Slider ExposureLevel;
    public Button SaveButton;
    public AudioClip[] SFX;
    public AudioClip[] Music;
    public Button PlayButton;
    public Button SettingsButton;
    public Button QuitButton;

    private string Name;
    private Image GetImage;
    private Color TempColour;

    public Canvas MenuCanvas;

    private void OnEnable()
    {
        MenuCanvas = FindObjectOfType<Canvas>();

        if(PlayButton != null)
        {
            PlayButton.onClick.AddListener(delegate { OnPlayClick(); });
        }

        if(SettingsButton != null)
        {
            SettingsButton.onClick.AddListener(delegate { OnSettingsClick(); });
        }

        if(QuitButton !=null)
        {
            QuitButton.onClick.AddListener(delegate { OnQuitClick(); });
        }

        if (SaveButton != null)
        {
            SaveButton.onClick.AddListener(delegate { OnSaveClick(); });
        }

        if (ColourGrading != null)
        {
            ColourGrading.onValueChanged.AddListener(delegate { ColourGradingToggle(); });
            ColourGradingToggle();
        }

        if (SFXVolume != null)
        {
            SFXVolume.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });
        }

        if (MusicVolume != null)
        {
            MusicVolume.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        }

        if (SaturationLevel != null)
        {
            SaturationLevel.onValueChanged.AddListener(delegate { OnSaturationChanged(); });
        }

        if (ContrastLevel != null)
        {
            ContrastLevel.onValueChanged.AddListener(delegate { OnContrastChanged(); });
        }

        if (ExposureLevel != null)
        {
            ExposureLevel.onValueChanged.AddListener(delegate { OnExposureChanged(); });
        }
    }
    void Start()
    {
        //Name = GameObject.Find("Canvas").transform.GetChild(6).GetComponent<InputField>().text;
    }

    private void ColourGradingToggle()
    {
        GetComponent<GreyScale>().Greyscale = ColourGrading.isOn;
        GetImage = SaturationLevel.gameObject.transform.GetChild(0).GetComponent<Image>();

        if (!ColourGrading.isOn)
        {
            TempColour = GetImage.color;
            TempColour = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            GetImage.color = TempColour;
            SaturationLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
            SaturationLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = GetImage.color;
            SaturationLevel.GetComponent<Slider>().enabled = false;

            ContrastLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = GetImage.color;
            ContrastLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
            ContrastLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = GetImage.color;
            ContrastLevel.GetComponent<Slider>().enabled = false;

            ExposureLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = GetImage.color;
            ExposureLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
            ExposureLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = GetImage.color;
            ExposureLevel.GetComponent<Slider>().enabled = false;
        }
        else
        {
            TempColour = GetImage.color;
            TempColour = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GetImage.color = TempColour;
            SaturationLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
            SaturationLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = GetImage.color;
            SaturationLevel.GetComponent<Slider>().enabled = true;

            ContrastLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = GetImage.color;
            ContrastLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
            ContrastLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = GetImage.color;
            ContrastLevel.GetComponent<Slider>().enabled = true;

            ExposureLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = GetImage.color;
            ExposureLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
            ExposureLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = GetImage.color;
            ExposureLevel.GetComponent<Slider>().enabled = true;
        }
    }

    private void OnMusicVolumeChange()
    {

    }

    private void OnSFXVolumeChange()
    {

    }

    private void OnSaturationChanged()
    {
        GetComponent<GreyScale>().Saturation = SaturationLevel.value;
    }

    private void OnContrastChanged()
    {
        GetComponent<GreyScale>().Contrast = ContrastLevel.value;
    }

    private void OnExposureChanged()
    {
        GetComponent<GreyScale>().Exposure = ExposureLevel.value;
    }

    private void OnPlayClick()
    {
        SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
    }

    private void OnSettingsClick()
    {
        MenuCanvas.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        MenuCanvas.gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }

    private void OnQuitClick()
    {

    }

    private void OnSaveClick()
    {
        MenuCanvas.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        MenuCanvas.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //var SubmitEvent = new InputField.SubmitEvent();

        //SubmitEvent.AddListener(delegate { SubmitName(); });

        //Name = GameObject.Find("Canvas").transform.GetChild(6).GetComponent<InputField>().text;

        //GameObject.Find("Canvas").transform.GetChild(6).GetComponent<InputField>().onEndEdit = SubmitEvent;
    }
}
