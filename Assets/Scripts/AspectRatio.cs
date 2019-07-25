using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatio : MonoBehaviour
{
    private float Width;
    private float Height;

    private Camera MainCamera;
    private Canvas MainCanvas;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject ProfileMenu;
    [SerializeField] private GameObject ShopMenu;
    [SerializeField] private float Aspectratio;
    [SerializeField] private bool IsMobile = false;

    void Start()
    {
        MainCamera = FindObjectOfType<Camera>();
        MainCanvas = FindObjectOfType<Canvas>();

        MainMenu = MainCanvas.transform.GetChild(2).transform.gameObject;
        SettingsMenu = MainCanvas.transform.GetChild(3).transform.gameObject;
        ProfileMenu = MainCanvas.transform.GetChild(4).transform.gameObject;
        ShopMenu = MainCanvas.transform.GetChild(5).transform.gameObject;

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

            #region Main Menu
                MainMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.top = 500;
                MainMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(150, 50);
                MainMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 25);
            #endregion

            #region Settings Menu
                SettingsMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.left = 550;
                SettingsMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.bottom = 650;
                SettingsMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 65);

                SettingsMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.top = 750;
                SettingsMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.left = -330;
                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.right = 0;
                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.bottom = 650;
                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                SettingsMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.top = 250;
                SettingsMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 45);
                SettingsMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 25);
            #endregion

            #region Profile Menu
                ProfileMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.bottom = 700;
                ProfileMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.right = 0;
                ProfileMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(200,50);

                ProfileMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.top = 250;
                ProfileMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.bottom = 0;
                ProfileMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 75);

                ProfileMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.bottom = 485;
                ProfileMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.left = 0;
                ProfileMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().cellSize = new Vector2(250, 75);

                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.top = 0;
                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.bottom = 170;
                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.left = 200;
                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 200);

                ProfileMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().padding.top = 700;
                ProfileMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().cellSize = new Vector2(300, 300);
                ProfileMenu.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-206f,0f, 206f, 0f);
                ProfileMenu.transform.GetChild(4).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(-208.76f, 95.496f, 208.76f, -95.496f);
                ProfileMenu.transform.GetChild(4).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-58.127f, 205.63f, 58.127f, -205.63f);
                ProfileMenu.transform.GetChild(4).GetChild(0).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(171.25f, 205.63f, -171.25f, -205.63f);
                ProfileMenu.transform.GetChild(4).GetChild(0).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(310f, 95.696f, -310f, -95.696f);


                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.top = 0;
                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.bottom = 170;
                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.right = 200;
                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 200);

                ProfileMenu.transform.GetChild(6).GetComponent<GridLayoutGroup>().padding.top = 750;
                ProfileMenu.transform.GetChild(6).GetComponent<GridLayoutGroup>().padding.right = 340;
                ProfileMenu.transform.GetChild(6).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ProfileMenu.transform.GetChild(7).GetComponent<GridLayoutGroup>().padding.top = 750;
                ProfileMenu.transform.GetChild(7).GetComponent<GridLayoutGroup>().padding.left = 340;
                ProfileMenu.transform.GetChild(7).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);
            #endregion

            #region Shop Menu
                ShopMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.bottom = 300;
                ShopMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().cellSize = new Vector2(240, 240);
                ShopMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-125f, -246f, 125f, 246f);
                ShopMenu.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 0f, 0f);
                ShopMenu.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(125f, -246f, -125f, 246f);

                ShopMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.top = 550;

                ShopMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.top = 550;
                ShopMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ShopMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().padding.top = 750;
                ShopMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().padding.right = 275;
                ShopMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ShopMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.top = 750;
                ShopMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.left = 275;
                ShopMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);
            #endregion

            MainCamera.aspect = Aspectratio;
        }

        else
        {
            Debug.Log("desktop");
            Screen.orientation = ScreenOrientation.Landscape;

            #region Main Menu
                MainMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.top = 300;
                MainMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 25);
                MainMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 25);
            #endregion

            #region Settings Menu
                SettingsMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.left = 950;
                SettingsMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.bottom = 350;
                SettingsMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(300,65);

                SettingsMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.top = 425;
                SettingsMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.right = 725;
                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.bottom = 350;
                SettingsMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75,75);

                SettingsMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.top = 25;
                SettingsMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().cellSize = new Vector2(200,45);
                SettingsMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 25);
            #endregion

            #region Profile Menu
                ProfileMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.bottom = 360;
                ProfileMenu.transform.GetChild(0).GetComponent<GridLayoutGroup>().padding.right = 610;

                ProfileMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.bottom = 360;

                ProfileMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.bottom = 360;
                ProfileMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.left = 610;

                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.top = 20;
                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.left = 700;
                ProfileMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().cellSize = new Vector2(185, 185);

                ProfileMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().padding.top = 20;
                ProfileMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().cellSize = new Vector2(185, 185);

                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.top = 20;
                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.right = 700;
                ProfileMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().cellSize = new Vector2(185, 185);

                ProfileMenu.transform.GetChild(6).GetComponent<GridLayoutGroup>().padding.top = 425;
                ProfileMenu.transform.GetChild(6).GetComponent<GridLayoutGroup>().padding.right = 725;
                ProfileMenu.transform.GetChild(6).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ProfileMenu.transform.GetChild(7).GetComponent<GridLayoutGroup>().padding.top = 425;
                ProfileMenu.transform.GetChild(7).GetComponent<GridLayoutGroup>().padding.left = 725;
                ProfileMenu.transform.GetChild(7).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);
            #endregion

            #region Shop Menu
                ShopMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().padding.bottom = 100;
                ShopMenu.transform.GetChild(1).GetComponent<GridLayoutGroup>().cellSize = new Vector2(285, 285);
                ShopMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-285f, 0f, 285f, 0f);
                ShopMenu.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(0f, 0f, 0f, 0f);
                ShopMenu.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(285f, 0f, -285f, 0f);

                ShopMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().padding.top = 270;
                ShopMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().spacing = new Vector2(200, 0);
                ShopMenu.transform.GetChild(2).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ShopMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().padding.top = 270;
                ShopMenu.transform.GetChild(3).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ShopMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().padding.top = 425;
                ShopMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().padding.right = 725;
                ShopMenu.transform.GetChild(4).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);

                ShopMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.top = 425;
                ShopMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().padding.left = 725;
                ShopMenu.transform.GetChild(5).GetComponent<GridLayoutGroup>().cellSize = new Vector2(75, 75);
            #endregion

            MainCamera.aspect = (Width / Height) * (Screen.width / Screen.height);
        }

    }
}
