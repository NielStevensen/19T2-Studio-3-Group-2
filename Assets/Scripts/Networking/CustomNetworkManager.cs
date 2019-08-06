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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.singleton.StopHost();
        }
    }

    public void Startuphost()
    {
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
        string IPAdress = FindObjectOfType<InputField>().transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = IPAdress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = port;
    }

    public void Home()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }


    private void OnLevelWasLoaded(int level)
    {
        if(level == lobbySceneNum)
        {
            print("test");

            Button startButton = GameObject.Find("StartHostButton").GetComponent<Button>();
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(Startuphost);

            Button joinButton = GameObject.Find("JoinButton").GetComponent<Button>();
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(Joingame);
        }
    }
}
