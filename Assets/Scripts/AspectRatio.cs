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

    public bool IsMobile = false;

    void Start()
    {
        MainCamera = FindObjectOfType<Camera>();
        MainCanvas = FindObjectOfType<Canvas>();

        Width = Screen.width;
        Height = Screen.height;

        if (IsMobile == true)
        {
            Debug.Log("mobile");
            Screen.orientation = ScreenOrientation.Portrait;

            Aspectratio = Mathf.RoundToInt((Width / Height) * 100.0f) / 100.0f;

            if (Aspectratio == 0.75f)
            {
                Aspectratio = 0.65f;
            }

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

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                #region LobbyPanels
                    LobbyPanels.GetComponent<GridLayoutGroup>().padding.bottom = 315;
                    LobbyPanels.GetComponent<GridLayoutGroup>().cellSize = new Vector2(125, 150);
                    LobbyPanels.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-140f, 0f, 140f, 0f);
                    LobbyPanels.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 0f, 0f);
                    LobbyPanels.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(140f, 0f, -140f, 0f);
                #endregion

                #region NetworkButtons
                    NetworkButtons.GetComponent<GridLayoutGroup>().padding.top = 0;
                    NetworkButtons.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 50);
                    NetworkButtons.GetComponent<GridLayoutGroup>().spacing = new Vector2(25, 0);
                    NetworkButtons.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, -25f, 0f, 25f);
                    NetworkButtons.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, -100f, 0f, 100f);
                    NetworkButtons.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, -175f, 0f, 175f);
                #endregion

                #region HomeButton
                    HomeButton.GetComponent<GridLayoutGroup>().padding.top = 550;
                #endregion
            }


            MainCamera.aspect = Aspectratio;
        }

        else
        {
            Debug.Log("desktop");
            Screen.orientation = ScreenOrientation.Landscape;

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

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                #region LobbyPanels
                LobbyPanels.GetComponent<GridLayoutGroup>().padding.bottom = 0;
                LobbyPanels.GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 250);
                LobbyPanels.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-275f, 0f, 275f, 0f);
                LobbyPanels.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 0f, 0f);
                LobbyPanels.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(275f, 0f, -275f, 0f);
                #endregion

                #region NetworkButtons
                NetworkButtons.GetComponent<GridLayoutGroup>().padding.top = 320;
                NetworkButtons.GetComponent<GridLayoutGroup>().padding.left = 50;
                NetworkButtons.GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 50);
                NetworkButtons.GetComponent<GridLayoutGroup>().spacing = new Vector2(25, 0);
                NetworkButtons.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-300f, -25f, 250f, 25f);
                NetworkButtons.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(-24f, -25f, -24f, 25f);
                NetworkButtons.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(250f, -25f, -300f, 25f);
                #endregion

                #region HomeButton
                HomeButton.GetComponent<GridLayoutGroup>().padding.top = 550;
                #endregion
            }

            MainCamera.aspect = (Width / Height) * (Screen.width / Screen.height);
        }

    }
}
