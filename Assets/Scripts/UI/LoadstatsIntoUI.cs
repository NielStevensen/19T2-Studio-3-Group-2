using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadstatsIntoUI : MonoBehaviour
{
    public Text combos;
    public Text chains;
    public Text wins;
    public Text currency;

    private void OnEnable()
    {
        SaveData data;
        if (SaveSystem.LoadSave() != null)
        {
            data = SaveSystem.LoadSave();
        }
        else
        {
            data = new SaveData(0, 0, 0, 0);
        }
        Debug.Log("Updatecombo");

        //for each non null text add the apropriate value
        if (combos != null)
        {
            combos.text = data.HighestCombo.ToString();
        }
        if (chains != null)
        {
            chains.text = data.HighestChain.ToString();
        }
        if (wins != null)
        {
            wins.text = data.Wins.ToString();
        }
        if (currency != null)
        {
            currency.text = data.currency.ToString();
        }
    }

}
