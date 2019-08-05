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

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject ProfileMenu;
    public GameObject ShopMenu;

    public AudioClip[] SFX;
    public AudioClip[] Music;
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    private bool GameOver = false;
    private string Name;
    private Color TempColour;

    public GreyScale Greyscale;
    public Canvas MenuCanvas;

    public InputField.SubmitEvent SubmitEvent;

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
    }

    public void OnMusicVolumeChange()
    {
        MusicSource.volume = MusicVolume.value;
    }

    public void OnSFXVolumeChange()
    {
        SFXSource.volume = SFXVolume.value;
    }

    public void OnSaturationChange()
    {
        Greyscale.Saturation = SaturationLevel.value;
    }

    public void OnContrastChange()
    {
        Greyscale.Contrast = ContrastLevel.value;
    }

    public void OnExposureChange()
    {
        Greyscale.Exposure = ExposureLevel.value;
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
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
    }

    public void OnBackClick()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        ProfileMenu.SetActive(false);
        ShopMenu.SetActive(false);
        MenuCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnTileSetLeftClick()
    {

    }

    public void OnTileSetRightClick()
    {

    }

    public void OnPortaitLeftClick()
    {

    }

    public void OnPortraitRightClick()
    {

    }

    public void OnPurchaseClick()
    {

    }

    public void OnShopLeftClick()
    {

    }

    public void OnShopRightClick()
    {

    }

    public void OnEnterName()
    {
        Name = NameField.text;
        NameField.onEndEdit = SubmitEvent;
        Debug.Log(Name);
    }
}
