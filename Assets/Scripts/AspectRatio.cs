using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    private float Width;
    private float Height;

    public Camera MainCamera;


    void Start()
    {
        Width = MainCamera.orthographicSize * MainCamera.aspect;
        Height = MainCamera.orthographicSize;
    }

    void Update()
    {
        MainCamera.orthographicSize = Width / MainCamera.aspect;

        MainCamera.transform.position = new Vector3(1 * (Width - MainCamera.orthographicSize * MainCamera.aspect), 1 * (Height - MainCamera.orthographicSize), MainCamera.transform.position.z);
    }
}
