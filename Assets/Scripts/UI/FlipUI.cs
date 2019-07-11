using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FlipUI : NetworkBehaviour
{

    public GameObject[] flip;
    public Image[] reverseFill;
    public Material customShader;
    public void  Start()
    {
#if UNITY_EDITOR
        if (isServer)
        {
            if (!isLocalPlayer)
            {
                foreach (GameObject a in flip)
                {
                    a.transform.localScale = new Vector3(-1, 1, 1);
                }

                foreach (Image a in reverseFill)
                {
                    a.fillOrigin = 1;
                }
                reverseFill[0].material = new Material(customShader);
            }
        }
        else
        {
            if (isLocalPlayer)
            {
                foreach (GameObject a in flip)
                {
                    a.transform.localScale = new Vector3(-1, 1, 1);
                }

                foreach (Image a in reverseFill)
                {
                    a.fillOrigin = 1;
                }
                reverseFill[0].material = new Material(customShader);
            }
        }
        #endif
#if UNITY_ANDROID
       //rearagnge UI
#endif
    }
}
