using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour
{
	int currentChain;
	bool displayed;
	public Text chainDisp;
	public Text comboDisp;
	Coroutine comboCoroutine;

	public void UpdateChains(int Checking)
	{
		if(Checking > 2 && Checking > currentChain)
		{
			if(comboCoroutine != null)
			{
				StopCoroutine(comboCoroutine);
			}

			comboCoroutine = StartCoroutine(WaitChain());

			chainDisp.enabled = true;
			currentChain = Checking;
			chainDisp.text = ("Chain: " + currentChain);
		}
	}

	public void UpdateCombo(int combo)
	{
		if (combo > 3)
		{
			comboDisp.enabled = true;
			comboDisp.text = ("Combo: " + combo);
			StartCoroutine(WaitCombo());
		}
	}

	IEnumerator WaitChain ()
	{
		yield return new WaitForSecondsRealtime(1f);
		currentChain = 0;
		chainDisp.enabled = false;
	}
	IEnumerator WaitCombo()
	{
		yield return new WaitForSecondsRealtime(1f);
		comboDisp.enabled = false;
	}
}