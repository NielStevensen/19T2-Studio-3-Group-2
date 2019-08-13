using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ArrangeUI : NetworkBehaviour
{

    public GameObject[] flip;
    public Image[] reverseFill;
    public Material customShader;

    public GameObject[] mobileRefrences; // list of objetcst for rearanging on mkbile
    Camera cam;
    public void Start()
    {
#if UNITY_STANDALONE
        if (isServer)
        {
            if (!isLocalPlayer)
            {
                foreach (GameObject a in flip)
                {
                    a.transform.localScale = new Vector3(a.transform.localScale.x * -1, a.transform.localScale.y, a.transform.localScale.z);
                }

                foreach (Image a in reverseFill)
                {
                    a.fillOrigin = 1;
                }
            } // rearange for server\
            gameObject.GetComponent<MatchUI>().comboDisp = GameObject.FindObjectOfType<UiSetup>().hostInfo[0];
            gameObject.GetComponent<MatchUI>().chainDisp = GameObject.FindObjectOfType<UiSetup>().hostInfo[1];
            gameObject.GetComponent<MatchUI>().oppComboDisp = GameObject.FindObjectOfType<UiSetup>().clientInfo[0];
            gameObject.GetComponent<MatchUI>().oppChainDisp = GameObject.FindObjectOfType<UiSetup>().clientInfo[1];
        }
        else
        {
            if (isLocalPlayer)
            {
                foreach (GameObject a in flip)
                {
                    a.transform.localScale = new Vector3(a.transform.localScale.x * -1, a.transform.localScale.y, a.transform.localScale.z);
                }

                foreach (Image a in reverseFill)
                {
                    a.fillOrigin = 1;
                }
            }
            gameObject.GetComponent<MatchUI>().comboDisp = GameObject.FindObjectOfType<UiSetup>().clientInfo[0];
            gameObject.GetComponent<MatchUI>().chainDisp = GameObject.FindObjectOfType<UiSetup>().clientInfo[1];
            gameObject.GetComponent<MatchUI>().oppComboDisp = GameObject.FindObjectOfType<UiSetup>().hostInfo[0];
            gameObject.GetComponent<MatchUI>().oppChainDisp = GameObject.FindObjectOfType<UiSetup>().hostInfo[1];
        } // rearange for client

#elif UNITY_ANDROID // if on an android platform

        cam = FindObjectOfType<Camera>();
        if (isServer) // set refrences to chain and combo printing
        {
            gameObject.GetComponent<MatchUI>().comboDisp = GameObject.FindObjectOfType<UiSetup>().hostInfo[0];
            gameObject.GetComponent<MatchUI>().chainDisp = GameObject.FindObjectOfType<UiSetup>().hostInfo[1];
        }
        else
        {
            gameObject.GetComponent<MatchUI>().comboDisp = GameObject.FindObjectOfType<UiSetup>().clientInfo[0];
            gameObject.GetComponent<MatchUI>().chainDisp = GameObject.FindObjectOfType<UiSetup>().clientInfo[1];
        } 

        if(isLocalPlayer)
        {
            mobileRefrences[0].transform.position = new Vector3(-1.45f, 3.11f, 0);
            mobileRefrences[1].transform.position = new Vector3(0f, -4.15f, 0);
            mobileRefrences[1].transform.localScale *= 1.5f;
            mobileRefrences[2].transform.position = new Vector3(0f, 5.25f, 0);
        } 
        else
        {
            mobileRefrences[0].transform.position = new Vector3(1.45f, 3.11f, 0);
        }

#endif

    }
}
