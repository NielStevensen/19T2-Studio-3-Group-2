using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class SaveData
    {
        public int HighestCombo;
        public int HighestChain;
        public int Wins;

        public SaveData(SaveData info)
        {

        HighestCombo = info.HighestCombo;
        HighestChain = info.HighestChain;
        Wins = info.Wins;

        }
        public SaveData(int a, int b,int c)
        {

        HighestCombo = a;
        HighestChain = b;
        Wins = c;

        }
    }
