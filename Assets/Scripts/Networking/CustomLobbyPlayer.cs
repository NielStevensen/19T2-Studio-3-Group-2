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

	//Reference manager
	private ReferenceManager references;

	//UI objects
	//Text objects
	public Text hostNameText;
	public Text clientNameText;
	public Text hostReadyText;
	public Text clientReadyText;
	//Cycle buttons
	public Button cycleFighterLeft;
	public Button cycleFighterRight;
	public Button cycleTilesLeft;
	public Button cycleTilesRight;
	//Ready button
	public Button readyButton;
	public Text readyButtonText;
	
	//Retrieve references
	private void Awake()
	{
		RetrieveReferences();
	}

	private void OnLevelWasLoaded(int level)
	{
		if (level == lobbySceneNum)
		{
			RetrieveReferences();
		}
	}

	//Retrieve references from another script
	void RetrieveReferences()
	{
		references = FindObjectOfType<ReferenceManager>();

		hostNameText = references.hostName;
		clientNameText = references.clientName;
		hostReadyText = references.hostReady;
		clientReadyText = references.clientReady;

		cycleFighterLeft = references.cycleFighterLeft;
		cycleFighterRight = references.cycleFighterRight;
		cycleTilesLeft = references.cycleTilesLeft;
		cycleTilesRight =	references.cycleTilesRight;

		readyButton = references.readyButton;
		readyButtonText = references.readyButtonText;

		readyButton.onClick.RemoveAllListeners();
		readyButton.onClick.AddListener(OnReadyClicked);
	}
	
	//Set ready
	public void OnReadyClicked()
	{
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

		CmdUpdateReadyText(readyToBegin, isServer);

		StartCoroutine(DelayedReset());
	}

	//Ripple ready state text update
	[Command]
	void CmdUpdateReadyText(bool isReady, bool isHost)
	{
		RpcUpdateReadyText(isReady, isHost);
	}

	//Receive ready state text update
	[ClientRpc]
	void RpcUpdateReadyText(bool isReady, bool isHost)
	{
		if (isLocalPlayer)
		{
			return;
		}

		if (isReady)
		{
			if (isHost)
			{
				hostReadyText.text = "READY!";
			}
			else
			{
				clientReadyText.text = "READY!";
			}
		}
		else
		{
			if (isHost)
			{
				hostReadyText.text = "NOT READY...";
			}
			else
			{
				clientReadyText.text = "NOT READY...!";
			}
		}
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
