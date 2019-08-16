using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuScript : MonoBehaviour
{
	public InputField NameField;
	public Toggle ColourGrading;
	public Slider MusicVolume;
	public Slider SFXVolume;
	public Slider SaturationLevel;
	public Slider ContrastLevel;
	public Slider ExposureLevel;

	public GameObject MainMenu;
	public GameObject SettingsMenu;
	public GameObject ProfileMenu;
	public GameObject ShopMenu;

	public AudioClip[] SFX;
	public AudioClip[] Music;
	public Image[] ShopArray;
	public AudioSource MusicSource;
	public AudioSource SFXSource;

    public GameManager Gamemanager = new GameManager();
	public GreyScale Greyscale;
	public Canvas MenuCanvas;

	public InputField.SubmitEvent SubmitEvent;

	private string Name;
	private Color TempColour;

    private void OnEnable()
    {
        LoadSettings();
    }

    void Start()
	{
		ColourGradingToggle();
	}

	public void ColourGradingToggle()
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

        Gamemanager.ColourGrading = ColourGrading.isOn;
	}

	public void OnMusicVolumeChange()
	{
		Gamemanager.MusicVolume = MusicSource.volume = MusicVolume.value;
	}

	public void OnSFXVolumeChange()
	{
		Gamemanager.SFXVolume =  SFXSource.volume = SFXVolume.value;
	}

	public void OnSaturationChange()
	{
		Gamemanager.Saturation = Greyscale.Saturation = SaturationLevel.value;
	}

	public void OnContrastChange()
	{
		Gamemanager.Contrast =  Greyscale.Contrast = ContrastLevel.value;
	}

	public void OnExposureChange()
	{
		Gamemanager.Exposure = Greyscale.Exposure = ExposureLevel.value;
	}

	public void OnPlayClick()
	{
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        else
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
	}

	public void OnShopClick()
	{
		MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
		MainMenu.SetActive(false);
		SettingsMenu.SetActive(false);
		ProfileMenu.SetActive(false);
		ShopMenu.SetActive(true);
	}

	public void OnSettingsClick()
	{
		MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
		MainMenu.SetActive(false);
		SettingsMenu.SetActive(true);
		ProfileMenu.SetActive(false);
		ShopMenu.SetActive(false);
	}

	public void OnQuitClick()
	{
		Application.Quit();
	}

	public void OnProfileClick()
	{
		MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
		ProfileMenu.SetActive(true);
		MainMenu.SetActive(false);
		SettingsMenu.SetActive(false);
		ShopMenu.SetActive(false);
	}

	public void OnProfileClose()
	{
		MenuCanvas.transform.GetChild(1).gameObject.SetActive(false);
		ProfileMenu.SetActive(false);
		MainMenu.SetActive(false);
		SettingsMenu.SetActive(true);
		ShopMenu.SetActive(false);
	}

	public void OnSaveClick()
	{
		MainMenu.SetActive(true);
		SettingsMenu.SetActive(false);
		ProfileMenu.SetActive(false);
		ShopMenu.SetActive(false);
		MenuCanvas.transform.GetChild(1).gameObject.SetActive(true);

        SaveSettings();
	}

	public void OnBackClick()
	{
		MainMenu.SetActive(true);
		SettingsMenu.SetActive(false);
		ProfileMenu.SetActive(false);
		ShopMenu.SetActive(false);
		MenuCanvas.transform.GetChild(1).gameObject.SetActive(true);
	}

	public void OnEnterName()
	{
		Name = NameField.text;
		NameField.onEndEdit = SubmitEvent;
		Debug.Log(Name);
	}

    public void SaveSettings()
    {
        string jsondata = JsonUtility.ToJson(Gamemanager, true); //this line serializes the Gamemanager variables and creates a string

        File.WriteAllText(Application.persistentDataPath + "/GameSettings.json", jsondata); //This line writes the jsondata string to a file at the application path0.
    }

    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/GameSettings.json"))
        {
            Gamemanager = JsonUtility.FromJson<GameManager>(File.ReadAllText(Application.persistentDataPath + "/GameSettings.json"));

            ColourGrading.isOn = Gamemanager.ColourGrading;
            MusicVolume.value = Gamemanager.MusicVolume;
            SFXVolume.value = Gamemanager.SFXVolume;
            SaturationLevel.value = Gamemanager.Saturation;
            ContrastLevel.value = Gamemanager.Contrast;
            ExposureLevel.value = Gamemanager.Exposure;
        }

        else
        {
            Gamemanager.ColourGrading = ColourGrading.isOn = false;
            Gamemanager.MusicVolume = MusicVolume.value = 0.5f;
            Gamemanager.SFXVolume = SFXVolume.value = 0.5f;
            Gamemanager.Saturation = SaturationLevel.value = 0;
            Gamemanager.Contrast = ContrastLevel.value = 0;
            Gamemanager.Exposure = ExposureLevel.value = 0;

            SaveSettings();
        }
    }
}
