using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	//Profile display name
	public string profileName = "Player";

	//Last chosen items
	public int fighterIndex = 0;
	public int tileSetIndex = 0;
	public bool[] unlockedSets = new bool[4] { true, false, false, false };

	//Cursor control scheme
	public bool isCursorControl = true;

	//Stats
	public int HighestCombo;
    public int HighestChain;
    public int Wins;
    public int currency;

	public SaveData()
	{
		profileName = "Player";

		fighterIndex = 0;
		tileSetIndex = 0;
		unlockedSets = new bool[4] { true, false, false, false };

		isCursorControl = true;

		Wins = 0;
		HighestCombo = 0;
		HighestChain = 0;
		currency = 0;
	}

    public SaveData(SaveData info)
    {
		profileName = info.profileName;

		fighterIndex = info.fighterIndex;
		tileSetIndex = info.tileSetIndex;
		unlockedSets = info.unlockedSets;

		isCursorControl = info.isCursorControl;

		HighestCombo = info.HighestCombo;
		HighestChain = info.HighestChain;
		Wins = info.Wins;
		currency = info.currency;
    }

    public SaveData(int a, int b, int c, int d)
    {
		HighestCombo = a;
		HighestChain = b;
		Wins = c;
		currency = 0;
    }
}
