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
    public Button PlayButton;
    public Button ShopButton;
    public Button SettingsButton;
    public Button ProfileButton;
    public Button CloseProfileButton;
    public Button QuitButton;
    public Button BackButton;

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject Profile;
    public GameObject Shop;

    public Material BackgroundMaterial;
    public Material TitleMaterial;
    public Material ButtonMaterial;

    public AudioClip[] SFX;
    public AudioClip[] Music;
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    private bool GameOver = false;
    private string Name;
    private Color TempColour;

    public InputField.SubmitEvent SubmitEvent;

    private Canvas MenuCanvas;

    private void OnEnable()
    {
        MenuCanvas = FindObjectOfType<Canvas>();

        if (PlayButton != null)
        {
            PlayButton.onClick.AddListener(delegate { OnPlayClick(); });
        }

        if (ShopButton != null)
        {
            ShopButton.onClick.AddListener(delegate { OnShopClick(); });
        }

        if (SettingsButton != null)
        {
            SettingsButton.onClick.AddListener(delegate { OnSettingsClick(); });
        }

        if (QuitButton != null)
        {
            QuitButton.onClick.AddListener(delegate { OnQuitClick(); });
        }

        if (ProfileButton != null)
        {
            ProfileButton.onClick.AddListener(delegate { OnProfileClick(); });
        }

        if (CloseProfileButton != null)
        {
            CloseProfileButton.onClick.AddListener(delegate { OnProfileClose(); });
        }

        if (SaveButton != null)
        {
            SaveButton.onClick.AddListener(delegate { OnSaveClick(); });
        }

        if (BackButton != null)
        {
            BackButton.onClick.AddListener(delegate { OnBackClick(); });
        }

        if (NameField != null)
        {
            SubmitEvent.AddListener(delegate { SubmitScore(); });
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
        ColourGradingToggle();
    }

    private void ColourGradingToggle()
    {
        if (!ColourGrading.isOn)
        {
            TempColour = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            SaturationLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = TempColour;
            SaturationLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
            SaturationLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = TempColour;
            SaturationLevel.GetComponent<Slider>().enabled = false;

            ContrastLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = TempColour;
            ContrastLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
            ContrastLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = TempColour;
            ContrastLevel.GetComponent<Slider>().enabled = false;

            ExposureLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = TempColour;
            ExposureLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
            ExposureLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = TempColour;
            ExposureLevel.GetComponent<Slider>().enabled = false;
        }
        else
        {
            TempColour = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            SaturationLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = TempColour;
            SaturationLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
            SaturationLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = TempColour;
            SaturationLevel.GetComponent<Slider>().enabled = true;

            ContrastLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = TempColour;
            ContrastLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
            ContrastLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = TempColour;
            ContrastLevel.GetComponent<Slider>().enabled = true;

            ExposureLevel.gameObject.transform.GetChild(0).GetComponent<Image>().color = TempColour;
            ExposureLevel.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
            ExposureLevel.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = TempColour;
            ExposureLevel.GetComponent<Slider>().enabled = true;
        }
    }

    private void OnMusicVolumeChange()
    {
        MusicSource.volume = MusicVolume.value;
    }

    private void OnSFXVolumeChange()
    {
        SFXSource.volume = SFXVolume.value;
    }

    private void OnSaturationChanged()
    {
        BackgroundMaterial.SetFloat("_Saturation", SaturationLevel.value);
        TitleMaterial.SetFloat("_Saturation", SaturationLevel.value);
        ButtonMaterial.SetFloat("_Saturation", SaturationLevel.value);
    }

    private void OnContrastChanged()
    {
        BackgroundMaterial.SetFloat("_Contrast", ContrastLevel.value);
        TitleMaterial.SetFloat("_Contrast", ContrastLevel.value);
        ButtonMaterial.SetFloat("_Contrast", ContrastLevel.value);
    }

    private void OnExposureChanged()
    {
        BackgroundMaterial.SetFloat("_Exposure", ExposureLevel.value);
        TitleMaterial.SetFloat("_Exposure", ExposureLevel.value);
        ButtonMaterial.SetFloat("_Exposure", ExposureLevel.value);
    }

    private void OnPlayClick()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    private void OnShopClick()
    {
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        Profile.SetActive(false);
        Shop.SetActive(true);
    }

    private void OnSettingsClick()
    {
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        Profile.SetActive(false);
        Shop.SetActive(false);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }

    private void OnProfileClick()
    {
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
        Profile.SetActive(true);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        Shop.SetActive(false);
    }

    private void OnProfileClose()
    {
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
        Profile.SetActive(false);
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        Shop.SetActive(false);
    }

    private void OnSaveClick()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Profile.SetActive(false);
        Shop.SetActive(false);
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnBackClick()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Profile.SetActive(false);
        Shop.SetActive(false);
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void SubmitScore()
    {
        //GameManager.GetComponent<GameLoader>().GameOver.gameObject.SetActive(false);

        //GameObject.Find("Canvas").transform.GetChild(6).GetComponent<InputField>().text = "";

        //GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(false);

        //GameLoader.GameInstance.Leaderboard.SetActive(true);

        //GameLoader.GameInstance.Leaderboard.transform.GetChild(1).GetComponent<PlayerScoreList>().NewHighScore(Name, Score);

        //GameManager.GetComponent<GameLoader>().GameOver.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver == true)
        {
            Name = NameField.text;
            NameField.onEndEdit = SubmitEvent;
        }
    }
}
