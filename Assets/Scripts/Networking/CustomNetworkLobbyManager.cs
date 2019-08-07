using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomNetworkLobbyManager : NetworkLobbyManager
{
    [Space(10)]

	//Exposed variables
	[Tooltip("The build index of this scene.")]
    public int lobbySceneNum = 0;
	[Tooltip("The port to use.")]
    public int port = 7777;

	[Space(10)]

	//Menu EGO
	[Tooltip("Lobby menu with host and join functionality.")]
	public GameObject lobbyMenu;
	[Tooltip("Waiting room menu with ready up functionality.")]
	public GameObject waitingMenu;
	
	//temp. unecessary
	private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
			NetworkLobbyManager.singleton.StopHost();
        }*/
    }

	#region Starting a game
	//Start host
	public void Startuphost()
    {
        NetworkServer.Reset();
        SetPort();
        SetIPAddressServer();
		NetworkLobbyManager.singleton.StartHost();

		ToggleCanvas(false);
    }

	//Join a game
    public void JoinGame ()
    {
        SetPort();
        SetIPAddressClient();
		NetworkLobbyManager.singleton.StartClient();
    }

	//this doesn't work
	public override void OnLobbyClientEnter()
	{
		ToggleCanvas(false);
	}

	//Set port
    void SetPort()
    {
		NetworkLobbyManager.singleton.networkPort = port;
    }

	//Serverside IP stuff
    void SetIPAddressServer()
    {
        string ipAddress = FindObjectOfType<InputField>().transform.Find("Text").GetComponent<Text>().text;
		
        if (ipAddress != "")
        {
			NetworkLobbyManager.singleton.serverBindToIP = true;
			NetworkLobbyManager.singleton.serverBindAddress = ipAddress;
        }
    }

	//Clientside IP stuff
	void SetIPAddressClient()
    {
        string ipAddress = FindObjectOfType<InputField>().transform.Find("Text").GetComponent<Text>().text;

        if(ipAddress != "")
        {
			NetworkLobbyManager.singleton.networkAddress = ipAddress;
        }
    }
	#endregion

	#region Scene management
	//Swap between lobby and waiting room
	public void ToggleCanvas(bool isLobby)
	{
		lobbyMenu.SetActive(isLobby);
		waitingMenu.SetActive(!isLobby);
	}

	//Set up references on loading the lobby scene
    private void OnLevelWasLoaded(int level)
    {
        if(level == lobbySceneNum)
        {
			lobbyMenu = GameObject.Find("Lobby Panel");
			waitingMenu = GameObject.Find("Waiting Panel");
			
			Button startButton = GameObject.Find("StartHostButton").GetComponent<Button>();
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(Startuphost);

            Button joinButton = GameObject.Find("JoinButton").GetComponent<Button>();
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(JoinGame);
        }
    }
	#endregion
}
