using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomLobbyPlayer : NetworkLobbyPlayer
{
	//UI objects
	public Button readyButton;
	
	//Setup stuff when entering the lobby
	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();
		
		if (isLocalPlayer)
		{
			SetupLocalPlayer();
		}
		else
		{
			SetupOtherPlayer();
		}
	}

	//Setup on receiving authority - is this required?
	public override void OnStartAuthority()
	{
		base.OnStartAuthority();
		
		SetupLocalPlayer();
	}
	
	//Set the text and interactability of the ready button as well as define listening
	void SetupLocalPlayer()
	{
		readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
		readyButton.interactable = true;
		
		readyButton.onClick.RemoveAllListeners();
		readyButton.onClick.AddListener(OnReadyClicked);
	}

	//Set the text and interactability of the ready button
	void SetupOtherPlayer()
	{
		readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
		readyButton.interactable = false;

		OnClientReady(false);
	}

	//Set ready button text and interactability based on ready state
	public override void OnClientReady(bool readyState)
	{
		if (readyState)
		{
			Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
			textComponent.text = "READY";
			readyButton.interactable = false;
		}
		else
		{
			Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
			textComponent.text = isLocalPlayer ? "JOIN" : "...";
			readyButton.interactable = isLocalPlayer;
		}
	}
	
	//Set ready
	public void OnReadyClicked()
	{
		SendReadyToBeginMessage();
	}
}
