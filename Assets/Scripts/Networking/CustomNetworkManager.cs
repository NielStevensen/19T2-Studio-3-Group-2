using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [Space(10)]

    public int lobbySceneNum = 0;
    public int port = 7777;

    /*private bool wasLoaded = false;

    public GameObject manager;

    private void Awake()
    {
        manager = GameObject.Find("Custom Network Manager");

        if(manager == null)
        {
            manager = gameObject;

            manager.name = "Custom Network Manager";

            DontDestroyOnLoad(manager);
        }
        else
        {
            if(gameObject.name != "Custom Network Manager")
            {
                wasLoaded = true;

                Destroy(gameObject);
            }
        }
    }*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.singleton.StopHost();
        }
    }

    public void Startuphost()
    {
        print("start up");

        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void Joingame ()
    {
        SetupIpAdress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetupIpAdress ()
    {
        string IPAdress = FindObjectOfType<InputField>().transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = IPAdress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = port;
    }


    private void OnLevelWasLoaded(int level)
    {
        if(level == lobbySceneNum)// && !wasLoaded)
        {
            print("test");

            Button startButton = GameObject.Find("StartHostButton").GetComponent<Button>();
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(Startuphost);

            Button joinButton = GameObject.Find("JoinButton").GetComponent<Button>();
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(Joingame);

            //wasLoaded = true;
        }
    }
}
