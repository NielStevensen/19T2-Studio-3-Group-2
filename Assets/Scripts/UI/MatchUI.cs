using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchUI : NetworkBehaviour
{
    //refrence to the current chain counters
	int currentChain;
	int oppCurrentChain;

    // refrence the current combo on each field
    int highestCombo;
    int oppHighestCombo;

    // refrence to own diosplay boxes
	public Text chainDisp;
	public Text comboDisp;
    //refrence to oppents display boxes
    public Text oppChainDisp;
	public Text oppComboDisp;

    // used to detrmine if either board has a chain displayed
	Coroutine chainCoroutine;
	Coroutine oppChainCoroutine;


	public void UpdateChains(int Checking, bool localPlayer)
	{
        if (localPlayer)
        {
            if (Checking > 2 && Checking > currentChain)
            {
                chainDisp.enabled = true;
                currentChain = Checking;
                chainDisp.text = ("Chain: " + currentChain);
                CmdUpodatechain(Checking);

                if (chainCoroutine != null)
                {
                    StopCoroutine(chainCoroutine);
                }
                chainCoroutine = StartCoroutine(WaitChain(localPlayer));

            }
        }
        else
        {
            if (Checking > 2 && Checking > oppCurrentChain)
            {
                oppChainDisp.enabled = true;
                oppCurrentChain = Checking;
                oppChainDisp.text = ("Chain: " + oppCurrentChain);
                CmdUpodatechain(Checking);

                if (oppChainCoroutine != null)
                {
                    StopCoroutine(oppChainCoroutine);
                }
                oppChainCoroutine = StartCoroutine(WaitChain(localPlayer));

            }
        }
    }

	public void UpdateCombo(int combo, bool LocalPlayer)
	{
        if (LocalPlayer)
        {
            if (combo > 3 && combo > highestCombo)
            {
                highestCombo = combo;
                comboDisp.enabled = true;
                comboDisp.text = ("Combo: " + combo);
                CmdUpodatecombo(combo);

                StartCoroutine(WaitCombo(LocalPlayer));
            }
        }
        else
        {
            if (combo > 3 && combo > oppHighestCombo)
            {
                oppHighestCombo = combo;
                oppComboDisp.enabled = true;
                oppComboDisp.text = ("Combo: " + combo);
                CmdUpodatecombo(combo);

                StartCoroutine(WaitCombo(LocalPlayer));
            }
        }
	}

    [Command]
    void CmdUpodatecombo(int combo)
    {
        RpcUpodatecombo(combo);
    }
    [ClientRpc]
    void RpcUpodatecombo(int combo)
    {
        if (!isLocalPlayer)
        {
            UpdateCombo(combo, isLocalPlayer);
        }
    }

    [Command]
    void CmdUpodatechain(int chain)
    {
        RpcUpodatechain(chain);
    }
    [ClientRpc]
    void RpcUpodatechain(int chain)
    {
        if (!isLocalPlayer)
        {
            UpdateChains(chain, isLocalPlayer);
        }
    }

    IEnumerator WaitChain(bool local)
	{
        //yield return new WaitForSecondsRealtime(1f);
        yield return new WaitForSeconds(1f);

        if (local)
        {
            currentChain = 0;
            chainDisp.enabled = false;
        }
        else
        {
            oppCurrentChain = 0;
            oppChainDisp.enabled = false;
        }
	}
	IEnumerator WaitCombo(bool local)
	{
		//yield return new WaitForSecondsRealtime(1f);
        yield return new WaitForSeconds(1f);
        if (local)
        {
            highestCombo = 0;
            comboDisp.enabled = false;
        }
        else
        {
            oppHighestCombo = 0;
            oppComboDisp.enabled = false;
        }
	}
}