using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Purchasable
{
	public Sprite item;
	public int cost;
}

public class ShopManager : MonoBehaviour
{
	//Save data
	[SerializeField]
	private SaveData data;

	[Space(10)]

	//Tile set unlock display
	[Tooltip("All tile set images.")]
	public Purchasable[] allTileSets;
	[Tooltip("Tile set display image.")]
	public Image tileSetImage;
	[Tooltip("Tile set purchase button.")]
	public Button tilePurchaseButton;
	[Tooltip("Tile set purchased overlay image.")]
	public Image tilePurchasedImage; //An image to overlay above the set if already purchased. like real estate signs
	private int setIndex = 0;
	private bool[] setsUnlocked;

	[Space(10)]

	//Currency display
	[Tooltip("Currency display text.")]
	public Text currencyText;

	//Set the initial state of things
	private void OnEnable()
	{
		data = SaveSystem.LoadSave();

		//temp
		if (data == null)
		{
			data = new SaveData();
		}

		//Retrieve which sets have been unlocked and set UI accordingly
		setsUnlocked = data.unlockedSets;

		AdjustSets();

		//Display currency
		currencyText.text = data.currency.ToString();
	}

	//Cycle through tile sets
	public void OnCycleTileSet(int alt)
	{
		setIndex = ProfileManager.IntCycle(setIndex + alt, 3);
		
		AdjustSets();
	}

	//Adjust UI
	void AdjustSets()
	{
		tileSetImage.sprite = allTileSets[setIndex].item;

		if (data.currency >= allTileSets[setIndex].cost)
		{
			tilePurchaseButton.interactable = !setsUnlocked[setIndex + 1];
		}
		else
		{
			tilePurchaseButton.interactable = false;
		}

		if (tilePurchaseButton.interactable)
		{
			tilePurchaseButton.GetComponent<Image>().color = Color.white;
		}
		else
		{
			tilePurchaseButton.GetComponent<Image>().color = Color.grey;
		}

		tilePurchasedImage.enabled = setsUnlocked[setIndex + 1];
	}

	//Purchase tile set
	public void OnPurchaseSet()
	{
		setsUnlocked[setIndex + 1] = true;

		data.unlockedSets = setsUnlocked;
		data.currency -= allTileSets[setIndex].cost;
		SaveSystem.Save(data);

		currencyText.text = data.currency.ToString();

		OnCycleTileSet(0);
	}

	//Purchase ingame currency
	public void OnPurchaseCurrency()
	{
		data.currency += 10;
		
        SaveSystem.Save(data);

		AdjustSets();
        currencyText.text = data.currency.ToString();
		
        Application.OpenURL("https://gords.itch.io/fantasy-puzzle-arena");
    }
}
