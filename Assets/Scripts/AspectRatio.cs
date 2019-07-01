using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    private float Width;
    private float Height;

    private Camera MainCamera;
    private Canvas MainCanvas;
    private float Aspectratio;
    [SerializeField] private bool IsMobile = false;

    void Start()
    {
        MainCamera = FindObjectOfType<Camera>();
        MainCanvas = FindObjectOfType<Canvas>();

        Width = Screen.width;
        Height = Screen.height;

        if (Application.isMobilePlatform == true)
        {
            IsMobile = true;
        }


        if (IsMobile == true)
        {
            Debug.Log("mobile");
            Screen.orientation = ScreenOrientation.Portrait;

            Aspectratio = Mathf.RoundToInt((Width / Height) * 100.0f) / 100.0f;

            if (Aspectratio == 0.75f)
            {
                Aspectratio = 0.65f;
            }


            MainCanvas.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0.2464228f, 0.3463228f);
            MainCanvas.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(0.747f, 0.3946773f);
            MainCanvas.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);

            MainCanvas.transform.GetChild(2).GetChild(1).GetComponent<RectTransform>().anchorMin = new Vector2(0.2464228f, 0.247f);
            MainCanvas.transform.GetChild(2).GetChild(1).GetComponent<RectTransform>().anchorMax = new Vector2(0.747f, 0.301f);
            MainCanvas.transform.GetChild(2).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);

            MainCanvas.transform.GetChild(2).GetChild(2).GetComponent<RectTransform>().anchorMin = new Vector2(0.2464228f, 0.1493168f);
            MainCanvas.transform.GetChild(2).GetChild(2).GetComponent<RectTransform>().anchorMax = new Vector2(0.747f, 0.203f);
            MainCanvas.transform.GetChild(2).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);

            MainCanvas.transform.GetChild(2).GetChild(3).GetComponent<RectTransform>().anchorMin = new Vector2(0.2464228f, 0.05084162f);
            MainCanvas.transform.GetChild(2).GetChild(3).GetComponent<RectTransform>().anchorMax = new Vector2(0.747f, 0.1055249f);
            MainCanvas.transform.GetChild(2).GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);

            MainCamera.aspect = Aspectratio;
        }

        else
        {
            Debug.Log("desktop");
            Screen.orientation = ScreenOrientation.Landscape;

            MainCanvas.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0.4305217f, 0.3463228f);
            MainCanvas.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(0.5694783f, 0.3946773f);

            MainCamera.aspect = (Width / Height) * (Screen.width / Screen.height);
        }

    }
}
