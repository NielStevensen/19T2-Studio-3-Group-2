using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class SaveData
    {
        int HighestCombo;
        int HighestChain;
        int Wins;

        public SaveData(CombatHandler combatStats)
        {
        HighestCombo = combatStats.Combos[combatStats.Combos.Count - 1];
            if(combatStats.didWin)// add to wins if the pleyr won
            {
                
            }
            else
            {

            }
        }
    }
