using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomLobbyPlayer : NetworkLobbyPlayer
{
	//Lobby scene build index
	[Tooltip("Build index of the lobby scene.")]
	public int lobbySceneNum;

	//UI objects
	private Button readyButton;
	private Text readyButtonText;
	private Button cycleFighterLeft;
	private Button cycleFighterRight;
	private Button cycleTilesLeft;
	private Button cycleTilesRight;

	//Retrieve references
	private void Awake()
	{
		readyButton = GameObject.Find("Button Ready").GetComponent<Button>();
		readyButtonText = readyButton.transform.GetChild(0).GetComponent<Text>();
		
		readyButton.onClick.RemoveAllListeners();
		readyButton.onClick.AddListener(OnReadyClicked);

		cycleFighterLeft = GameObject.Find("Button fighter left").GetComponent<Button>();
		cycleFighterRight = GameObject.Find("Button fighter right").GetComponent<Button>();
		cycleTilesLeft = GameObject.Find("Button tiles left").GetComponent<Button>();
		cycleTilesRight = GameObject.Find("Button tiles right").GetComponent<Button>();
	}

	private void OnLevelWasLoaded(int level)
	{
		if (level == lobbySceneNum)
		{
			readyButton = GameObject.Find("Button Ready").GetComponent<Button>();
			readyButtonText = readyButton.transform.GetChild(0).GetComponent<Text>();

			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(OnReadyClicked);

			cycleFighterLeft = GameObject.Find("Button fighter left").GetComponent<Button>();
			cycleFighterRight = GameObject.Find("Button fighter right").GetComponent<Button>();
			cycleTilesLeft = GameObject.Find("Button tiles left").GetComponent<Button>();
			cycleTilesRight = GameObject.Find("Button tiles right").GetComponent<Button>();
		}
	}

	//Setup stuff when entering the lobby
	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();
		
		/*if (isLocalPlayer)
		{
			SetupLocalPlayer();
		}
		else
		{
			SetupOtherPlayer();
		}*/
	}

	//Setup on receiving authority - is this required?
	public override void OnStartAuthority()
	{
		base.OnStartAuthority();
		
		//SetupLocalPlayer();
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
		/*if (readyState)
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
		}*/
	}
	
	//Set ready
	public void OnReadyClicked()
	{
		print(readyToBegin);

		if (!readyToBegin)
		{
			readyButtonText.text = "UNREADY...";

			ToggleCycleInteractability(false);

			//call to save to profile

			SendReadyToBeginMessage();
		}
		else
		{
			readyButtonText.text = "READY!";

			ToggleCycleInteractability(true);

			SendNotReadyToBeginMessage();
		}

		StartCoroutine(DelayedReset());
	}

	//Toggle cycle button interactability
	void ToggleCycleInteractability(bool isInteractable)
	{
		cycleFighterLeft.interactable = isInteractable;
		cycleFighterRight.interactable = isInteractable;
		cycleTilesLeft.interactable = isInteractable;
		cycleTilesRight.interactable = isInteractable;
	}

	IEnumerator DelayedReset()
	{
		yield return new WaitForEndOfFrame();

		readyButton.interactable = true;
	}
}
