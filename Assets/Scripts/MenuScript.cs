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
    public Button PauseButton;

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject Profile;
    public GameObject Shop;
    public GameObject Pause;

    public AudioClip[] SFX;
    public AudioClip[] Music;
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    private bool GameOver = false;
    private string Name;
    private Image GetImage;
    private Color TempColour;
    public InputField.SubmitEvent SubmitEvent;

    private Canvas MenuCanvas;

    private void OnEnable()
    {
        MenuCanvas = FindObjectOfType<Canvas>();

        if(PlayButton != null)
        {
            PlayButton.onClick.AddListener(delegate { OnPlayClick(); });
        }

        if(ShopButton !=null)
        {
            ShopButton.onClick.AddListener(delegate { OnShopClick(); });
        }

        if(SettingsButton != null)
        {
            SettingsButton.onClick.AddListener(delegate { OnSettingsClick(); });
        }

        if(QuitButton !=null)
        {
            QuitButton.onClick.AddListener(delegate { OnQuitClick(); });
        }

        if(ProfileButton != null)
        {
            ProfileButton.onClick.AddListener(delegate { OnProfileClick(); });
        }

        if(CloseProfileButton != null)
        {
            CloseProfileButton.onClick.AddListener(delegate { OnProfileClose(); });
        }

        if (SaveButton != null)
        {
            SaveButton.onClick.AddListener(delegate { OnSaveClick(); });
        }

        if (PauseButton != null)
        {
            PauseButton.onClick.AddListener(delegate { OnPauseClick(); });
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
        MusicSource.volume = MusicVolume.value;
    }

    private void OnSFXVolumeChange()
    {
        SFXSource.volume = SFXVolume.value;
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

    private void OnShopClick()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(false);
            Profile.SetActive(false);
            Shop.SetActive(true);
        }

        else
        {
            SettingsMenu.SetActive(false);
            Profile.SetActive(false);
            Shop.SetActive(true);
        }
    }

    private void OnSettingsClick()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(true);
            Profile.SetActive(false);
            Shop.SetActive(false);
        }

        else
        {
            SettingsMenu.SetActive(true);
            Profile.SetActive(false);
            Shop.SetActive(false);
        }
    }

    private void OnQuitClick()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }

    private void OnProfileClick()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Profile.SetActive(true);
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(false);
            Shop.SetActive(false);
        }

        else
        {
            Profile.SetActive(true);
            SettingsMenu.SetActive(false);
            Shop.SetActive(false);
        }
    }

    private void OnProfileClose()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Profile.SetActive(false);
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(true);
            Shop.SetActive(false);
        }

        else
        {
            Profile.SetActive(false);
            SettingsMenu.SetActive(true);
            Shop.SetActive(false);
        }
    }

    private void OnSaveClick()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenu.SetActive(true);
            SettingsMenu.SetActive(false);
            Profile.SetActive(false);
            Shop.SetActive(false);
        }

        else
        {
            SettingsMenu.SetActive(false);
            Profile.SetActive(false);
            Shop.SetActive(false);
        }
    }

    private void OnPauseClick()
    {
        if(!Pause.activeSelf)
        {
            Pause.SetActive(true);
        }

        else
        {
            Pause.SetActive(false);
        }
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
        if(GameOver == true)
        {
            Name = NameField.text;
            NameField.onEndEdit = SubmitEvent;
        }

        if(Input.GetKeyDown((KeyCode.Escape)))
        {
            if (!Pause.activeSelf)
            {
                Pause.SetActive(true);
                Time.timeScale = 0;
            }

            else
            {
                Pause.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
