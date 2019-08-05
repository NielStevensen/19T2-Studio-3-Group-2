﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomNetworkLobbyManager : NetworkLobbyManager
{
    [Space(10)]

	//Exposed variables
    public int lobbySceneNum = 0;
    public int port = 7777;

	//temp. unecessary
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.singleton.StopHost();
        }
    }

	//Start host
    public void Startuphost()
    {
        NetworkServer.Reset();
        SetPort();
        SetIPAddressServer();
        NetworkManager.singleton.StartHost();
    }

    public void Joingame ()
    {
        SetPort();
        SetIPAddressClient();
        NetworkManager.singleton.StartClient();
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = port;
    }

    void SetIPAddressServer()
    {
        string ipAddress = FindObjectOfType<InputField>().transform.Find("Text").GetComponent<Text>().text;
        
        if (ipAddress != "")
        {
            NetworkManager.singleton.serverBindToIP = true;
            NetworkManager.singleton.serverBindAddress = ipAddress;
        }
    }

    void SetIPAddressClient()
    {
        string ipAddress = FindObjectOfType<InputField>().transform.Find("Text").GetComponent<Text>().text;

        if(ipAddress != "")
        {
            NetworkManager.singleton.networkAddress = ipAddress;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == lobbySceneNum)
        {
            Button startButton = GameObject.Find("StartHostButton").GetComponent<Button>();
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(Startuphost);

            Button joinButton = GameObject.Find("JoinButton").GetComponent<Button>();
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(Joingame);
        }
    }
}
