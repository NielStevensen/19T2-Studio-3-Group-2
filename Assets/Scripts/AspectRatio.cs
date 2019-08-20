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

    void Start()
    {
        MainCamera = FindObjectOfType<Camera>();
        MainCanvas = FindObjectOfType<Canvas>();

        Width = Screen.width;
        Height = Screen.height;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("mobile");
            Screen.orientation = ScreenOrientation.Portrait;

            Aspectratio = Mathf.RoundToInt((Width / Height) * 100.0f) / 100.0f;

            if (Aspectratio == 0.75f)
            {
                Aspectratio = 0.65f;
            }

            Mobile();

            MainCamera.aspect = Aspectratio;
        }

        else
        {
            Debug.Log("desktop");
            Screen.orientation = ScreenOrientation.Landscape;

            Desktop();

            MainCamera.aspect = (Width / Height) * (Screen.width / Screen.height);
        }

    }

    private void Mobile()
    {
        #region Main Menu
            MainMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200);
            MainMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);

            MainMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            MainMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);

            MainMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            MainMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);

            MainMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
            MainMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);

            MainMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -600);
            MainMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);

            MainMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -800);
            MainMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);
        #endregion

        #region Settings Menu
            SettingsMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            SettingsMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(600, 150);

            SettingsMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            SettingsMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            SettingsMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(200, -200);
            SettingsMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            SettingsMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 200);
            SettingsMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            SettingsMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            SettingsMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
            SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            SettingsMenu.transform.GetChild(7).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);
        #endregion

        #region Profile Menu
            ProfileMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(525, -150);
            ProfileMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);

            ProfileMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -350);
            ProfileMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);

            ProfileMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-520, -550);
            ProfileMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);

            ProfileMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-750, -200);
            ProfileMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            ProfileMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            ProfileMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
            ProfileMenu.transform.GetChild(4).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(-400f, -745f, 400f, 745f);
            ProfileMenu.transform.GetChild(4).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(-372f, -585f, 372f, 585f);
            ProfileMenu.transform.GetChild(4).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-96f, -403f, 96f, 403f);
            ProfileMenu.transform.GetChild(4).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(310f, -403f, -310f, 403f);
            ProfileMenu.transform.GetChild(4).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(580f, -585f, -580f, 585f);

            ProfileMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 250);
            ProfileMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            ProfileMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, -200);
            ProfileMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            ProfileMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 100);
            ProfileMenu.transform.GetChild(7).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ProfileMenu.transform.GetChild(8).GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 100);
            ProfileMenu.transform.GetChild(8).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        #endregion

        #region Shop Menu
            ShopMenu.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-250f, -545f);
            ShopMenu.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500,500);
            ShopMenu.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            ShopMenu.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
            ShopMenu.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -545f);
            ShopMenu.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
            ShopMenu.transform.GetChild(0).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            ShopMenu.transform.GetChild(0).GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            ShopMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(290, -550);
            ShopMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-290, -550);
            ShopMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -550);
            ShopMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(150, 100);
            ShopMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(-150, 100);
            ShopMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            ShopMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(600, 150);
            ShopMenu.transform.GetChild(6).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector4(50, 0, 0, 0);
            ShopMenu.transform.GetChild(6).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-50, 0, 0, 0);
        #endregion
    }

    private void Desktop()
    {
        #region Main Menu
            MainMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
            MainMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(240, 80);

            MainMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75);
            MainMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(240, 80);

            MainMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -175);
            MainMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(240, 80);

            MainMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -275);
            MainMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(240, 80);

            MainMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -375);
            MainMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(240, 80);

            MainMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -475);
            MainMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(240, 80);
        #endregion

        #region Settings Menu
            SettingsMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            SettingsMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(600, 160);

            SettingsMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            SettingsMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            SettingsMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(200, -200);
            SettingsMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            SettingsMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 200);
            SettingsMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            SettingsMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            SettingsMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
            SettingsMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);

            SettingsMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
            SettingsMenu.transform.GetChild(7).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);
        #endregion

        #region Profile Menu
            ProfileMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(300, -150);
            ProfileMenu.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);

            ProfileMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);
            ProfileMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);

            ProfileMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, -150);
            ProfileMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);

            ProfileMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-725, 0);
            ProfileMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

            ProfileMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(-225, 0);
            ProfileMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

            ProfileMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(250, 0);
            ProfileMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

            ProfileMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, 0);
            ProfileMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

            ProfileMenu.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 100);
            ProfileMenu.transform.GetChild(7).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ProfileMenu.transform.GetChild(8).GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 100);
            ProfileMenu.transform.GetChild(8).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        #endregion

        #region Shop Menu
            ShopMenu.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-645f, 0f);
            ShopMenu.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
            ShopMenu.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            ShopMenu.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
            ShopMenu.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(645f, 0f);
            ShopMenu.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            ShopMenu.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector4(290, -150);
            ShopMenu.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector4(-290, -150);
            ShopMenu.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -150);
            ShopMenu.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector4(200, 100);
            ShopMenu.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector4(-200, 100);
            ShopMenu.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            ShopMenu.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 100);
            ShopMenu.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(600, 150);
        #endregion
    }
}
