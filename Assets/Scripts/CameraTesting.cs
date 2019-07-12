using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTesting : MonoBehaviour
{
    public Camera cam;
    public float defaultCamSize;
    public float newCamSize;
    public float horizontalCamSize;


    // Start is called before the first frame update
    void Start()
    {
        //calculating the default horizontal cam size determinced by the orthgraphic size
        //0.5625 is the aspect ratio of 9:16, 
        //using this ratio as we are setting that as our standard when designing the layouts
        horizontalCamSize = defaultCamSize * 0.5625f;

        //calculating and setting the new orthgraphic Size
        newCamSize = horizontalCamSize / cam.aspect;
        cam.orthographicSize = newCamSize;

        //moving the camera so the botton of the screen also in place.
        float newCamPositionY = cam.transform.position.y - (defaultCamSize - newCamSize);
        cam.transform.position = new Vector3(cam.transform.position.x, newCamPositionY, cam.transform.position.z);
        cam.cullingMask = 
#if UNITY_ANDROID
        Debug.Log("hi");
#endif
    }
}
