using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
	//Menu panels
	[Tooltip("Lobby menu with host and join functionality.")]
	public GameObject lobbyMenu;
	[Tooltip("Waiting room menu with ready up functionality.")]
	public GameObject waitingMenu;

	[Space(10)]

	//Text objects
	[Tooltip("The host's name text.")]
	public Text hostName;
	[Tooltip("The client's name text.")]
	public Text clientName;
	[Tooltip("The host's ready text.")]
	public Text hostReady;
	[Tooltip("The client's ready text.")]
	public Text clientReady;

	[Space(10)]

	//Cycle buttons
	[Tooltip("Fighter cycle left button.")]
	public Button cycleFighterLeft;
	[Tooltip("Fighter cycle right button.")]
	public Button cycleFighterRight;
	[Tooltip("Tile set cycle left button.")]
	public Button cycleTilesLeft;
	[Tooltip("Tile set cycle right button.")]
	public Button cycleTilesRight;

	[Space(10)]

	//Ready button
	[Tooltip("The ready button.")]
	public Button readyButton;
	[Tooltip("Ready button text.")]
	public Text readyButtonText;

	private void Start()
	{
		lobbyMenu.SetActive(true);
		waitingMenu.SetActive(false);
	}
}
