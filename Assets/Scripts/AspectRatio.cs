using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AspectRatio : MonoBehaviour
{
    private float Width;
    private float Height;

    private Camera MainCamera;
    private Canvas MainCanvas;

    [Header("General")]
    [SerializeField] private float Aspectratio;

    [Header("Main")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject ProfileMenu;
    [SerializeField] private GameObject ShopMenu;

    [Header("Lobby")]
    [SerializeField] private GameObject LobbyPanels;
    [SerializeField] private GameObject NetworkButtons;
    [SerializeField] private GameObject HomeButton;

    void Start()
    {
        MainCamera = FindObjectOfType<Camera>();
        MainCanvas = FindObjectOfType<Canvas>();

        Width = Screen.width;
        Height = Screen.height;

        if (Application.isMobilePlatform == true)
        {
            Debug.Log("mobile");
            Screen.orientation = ScreenOrientation.Portrait;

            Aspectratio = Mathf.RoundToInt((Width / Height) * 100.0f) / 100.0f;

            if (Aspectratio == 0.75f)
            {
                Aspectratio = 0.65f;
            }

            #region Menu Scene
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    #region Main Menu
                        MainMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 0, 300, 150);

                        MainMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -200, 300, 150);

                        MainMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -400, 300, 150);

                        MainMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -600, 300, 150);
                    #endregion

                    #region Settings Menu
                        SettingsMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -200, 600, 150);

                        SettingsMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 150, 150);

                        SettingsMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(200, -200, 150, 150);

                        SettingsMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 200, 800, 45);

                        SettingsMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 800, 45);

                        SettingsMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 0, 800, 45);

                        SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -100, 800, 45);

                        SettingsMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -200, 800, 45);
                    #endregion

                    #region Profile Menu
                        ProfileMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(540, -150, 500, 150);

                        ProfileMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -350, 500, 150);

                        ProfileMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-540, -550, 500, 150);

                        ProfileMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(-300, 0, 500, 500);

                        ProfileMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 0, 500, 500);
                        ProfileMenu.transform.GetChild(4).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-400f, -745f, 400f, 745f);
                        ProfileMenu.transform.GetChild(4).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(-372f, -585f, 372f, 585f);
                        ProfileMenu.transform.GetChild(4).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-96f, -403f, 96f, 403f);
                        ProfileMenu.transform.GetChild(4).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(310f, -403f, -310f, 403f);
                        ProfileMenu.transform.GetChild(4).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(580f, -585f, -580f, 585f);

                        ProfileMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(300, 0, 500, 500);

                        ProfileMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(200, 100, 150, 150);

                        ProfileMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector4(-200, 100, 150, 150);
                    #endregion

                    #region Shop Menu
                        ShopMenu.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-250f, -545f, 500f, 500f);
                        ShopMenu.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 500f, 500f);
                        ShopMenu.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(250f, -545f, 500f, 500f);

                        ShopMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(290, -550, 150, 150);

                        ShopMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-290, -550, 150, 150);

                        ShopMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -550, 150, 150);

                        ShopMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(150, 100, 150, 150);

                        ShopMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(-150, 100, 150, 150);

                        ShopMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 600, 150);
                    #endregion
                }
            #endregion

            #region Lobby Scene
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    #region LobbyPanels
                        LobbyPanels.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -150f, 0, 0);
                        LobbyPanels.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-260f, -650f, 200f, 200f);
                        LobbyPanels.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, -125f, 200f, 200f);
                        LobbyPanels.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(260f, -650f, 200f, 200f);
                    #endregion

                    #region NetworkButtons
                        NetworkButtons.transform.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 215, 0, 0);
                        NetworkButtons.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 500f, 180f, 80f);
                        NetworkButtons.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 275f, 180f, 80f);
                        NetworkButtons.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 75f, 180f, 80f);
                    #endregion

                    #region HomeButton
                        HomeButton.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 150, 150);
                    #endregion
                }
            #endregion


            MainCamera.aspect = Aspectratio;
        }

        else
        {
            Debug.Log("desktop");
            Screen.orientation = ScreenOrientation.Landscape;

            #region Menu Scene
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    #region Main Menu
                        MainMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(0,0, 180, 80);

                        MainMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0,-100, 180, 80);

                        MainMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -200, 180, 80);

                        MainMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -300, 180, 80);
                    #endregion

                    #region Settings Menu
                        SettingsMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -200, 600, 150);

                        SettingsMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 150, 150);

                        SettingsMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(200, -200, 150, 150);

                        SettingsMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 200, 800, 45);

                        SettingsMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 800, 45);

                        SettingsMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 0, 800, 45);

                        SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -100, 800, 45);
                        SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

                        SettingsMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -200, 800, 45);
                    #endregion

                    #region Profile Menu
                        ProfileMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(300, -150, 500, 150);

                        ProfileMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -150, 500, 150);

                        ProfileMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-300, -150, 500, 150);

                        ProfileMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(-300, 0, 500, 500);

                        ProfileMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 0, 500, 500);

                        ProfileMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(300, 0, 500, 500);

                        ProfileMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(200, 100, 150, 150);

                        ProfileMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector4(-200, 100, 150, 150);
                    #endregion

                    #region Shop Menu
                        ShopMenu.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-645f, 0f, 500f, 500f);
                        ShopMenu.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 500f, 500f);
                        ShopMenu.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(645f, 0f, 500f, 500f);

                        ShopMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(290, -150, 150, 150);

                        ShopMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-290, -150, 150, 150);

                        ShopMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -150, 150, 150);

                        ShopMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(200, 100, 150, 150);

                        ShopMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(-200, 100, 150, 150);

                        ShopMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 600, 150);
                    #endregion
                }
            #endregion

            #region Lobby Scene
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    #region LobbyPanels
                        LobbyPanels.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -150f, 0, 0);
                        LobbyPanels.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-600f, -135f, 500f, 500f);
                        LobbyPanels.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, -135f, 500f, 500f);
                        LobbyPanels.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(600f, -135f, 500f, 500f);
                    #endregion

                    #region NetworkButtons
                        NetworkButtons.transform.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 400, 0, 0);
                        NetworkButtons.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-600f, 0f, 250f, 80f);
                        NetworkButtons.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 250f, 80f);
                        NetworkButtons.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(600f, 0f, 250f, 80f);
                    #endregion

                    #region HomeButton
                        HomeButton.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100, 150, 150);
                    #endregion
                }
            #endregion

            MainCamera.aspect = (Width / Height) * (Screen.width / Screen.height);
        }

    }
}
