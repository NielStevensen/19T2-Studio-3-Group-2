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
        if (isServer)
        {
            if (!isLocalPlayer)
            {
                Debug.Log("client");
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
                Debug.Log("client");
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
    }
}
