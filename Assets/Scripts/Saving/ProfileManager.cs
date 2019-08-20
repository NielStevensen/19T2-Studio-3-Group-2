using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TileSet
{
	public string name = "Set";
	public Sprite[] set;
}

public class ProfileManager : MonoBehaviour
{
	//Save data
	[SerializeField]
	private SaveData data;
	
	[Space(10)]

	//Profile name + input
	[Tooltip("Profile display name input field.")]
	public InputField nameInput;

	[Space(10)]

	//Tile set cycle control
	[Tooltip("Base image of each tile set.")]
	public TileSet[] tileSets;
	[Tooltip("The sprites that display the current tile set.")]
	public Image[] tileImages;
	private int tileIndex;
	private bool[] tilesUnlocked;

	[Space(10)]

	//Fighter cycle control
	[Tooltip("All fighter sprites.")]
	public Sprite[] fighters;
	[Tooltip("The sprite that displays the current fighter.")]
	public Image fighterImage;
    public Animator FighterAnimator;
	public RuntimeAnimatorController[] Controllers;
	private int fighterIndex;

	//Fighter stat display
	[Tooltip("Fighter element text.")]
	public Text fighterElement;
	[Tooltip("Fighter health text.")]
	public Text fighterHealth;
	[Tooltip("Fighter attack text.")]
	public Text fighterAttack;
	[Tooltip("Fighter defence text.")]
	public Text fighterDefence;
	[Tooltip("Fighter ability text.")]
	public Text fighterAbility;
	//[Tooltip("Fighter element text.")]
	//public Text fighterElement;

	[Space(10)]

	//Stats display
	[Tooltip("Wins display.")]
	public Text winsText;
	[Tooltip("Combos display.")]
	public Text comboText;
	[Tooltip("Chains display.")]
	public Text chainText;

	//Set the initial state of things
	private void OnEnable()
	{
		data = SaveSystem.LoadSave();

		//temp
		if(data == null)
		{
			data = new SaveData();
		}

		//Set display name
		nameInput.text = data.profileName;

		//Set current tile set
		tileIndex = data.tileSetIndex;
		tilesUnlocked = data.unlockedSets;

		for(int i = 0; i < 5; i++)
		{
			tileImages[i].sprite = tileSets[tileIndex].set[i];
		}

		//Set current fighter
		fighterIndex = data.fighterIndex;
		fighterImage.sprite = fighters[fighterIndex];
		//FighterAnimator.runtimeAnimatorController = Controllers[fighterIndex];

		UpdateFighterStats();

		//Set stats display
		winsText.text = data.Wins.ToString();
		comboText.text = data.HighestCombo.ToString();
		chainText.text = data.HighestChain.ToString();
	}

	//Set profile display name
	public void OnSetProfileName()
	{
		data.profileName = nameInput.text;
	}

	//Cycle tile sets
	public void OnCycleTileSet(int alt)
	{
		int newTileIndex = tileIndex;

		for(int i = 1; i < 4; i++)
		{
			int index = IntCycle(tileIndex + i * alt, 4);

			if (tilesUnlocked[index])
			{
				newTileIndex = index;

				break;
			}
		}

		data.tileSetIndex = newTileIndex;
		tileIndex = newTileIndex;

		for (int i = 0; i < 5; i++)
		{
			tileImages[i].sprite = tileSets[tileIndex].set[i];
		}
	}

	//Cycle fighter
	public void OnCycleFighter(int alt)
	{
		fighterIndex = IntCycle(fighterIndex + alt, 4);

		data.fighterIndex = fighterIndex;

		fighterImage.sprite = fighters[fighterIndex];
        FighterAnimator.runtimeAnimatorController = Controllers[fighterIndex];

		UpdateFighterStats();
	}

	//Display the current fighter's stats
	void UpdateFighterStats()
	{
		switch (fighterIndex)
		{
			case 0:
				fighterElement.text = "Earth";
				fighterAbility.text = "Armour\nGreatly increase defence for the next attack.";

				break;
			case 1:
				fighterElement.text = "Lightning";
				fighterAbility.text = "Double Shot\nSend over 2 weaker attacks.";

				break;
			case 2:
				fighterElement.text = "Water";
				fighterAbility.text = "Leech\nHeal half of the damage inflicted.";

				break;
			case 3:
				fighterElement.text = "Fire";
				fighterAbility.text = "Fireball\nSend over a very powerfull attack.";

				break;
		}

		fighterHealth.text = AvatarStats.allFighters[fighterIndex].maxHealth.ToString();
		fighterAttack.text = AvatarStats.allFighters[fighterIndex].attack.ToString();
		fighterDefence.text = AvatarStats.allFighters[fighterIndex].defence.ToString();
	}

	//Keep values between min and max
	public static int IntCycle(int input, int max)
	{
		if(input < 0)
		{
			input += max;
		}

		return input %= max;
	}

	//Save data
	public void SaveProfile()
	{
		SaveSystem.Save(data);
	}
}
