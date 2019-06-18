using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeAttack : MonoBehaviour
{
    //add to block manager to call
    //atkBar.FillBar(relevantType, allChains[newChainIndex], comboCount); goes under mui call
    //private ChargeAttack atkBar; under mui decleration
    //atkBar = gameObject.GetComponent<ChargeAttack>(); // set on start

    public Image Bar;

    [Tooltip("total capacity of bar")]
    public float capacity;

    [Tooltip("current capacity of bar")]
    public float Current;

    [HideInInspector]
    public float aCount;
    [HideInInspector]
    public float bCount;
    [HideInInspector]
    public float cCount;
    [HideInInspector]
    public float dCount;
    [HideInInspector]
    public float eCount;

    public void FillBar(BlockTypes colour,int chainCount, int comboCount)
    {
            switch (colour)
            {
                case BlockTypes.A:
                    aCount += comboCount;

                    break;
                case BlockTypes.B:
                    bCount += comboCount;

                    break;
                case BlockTypes.C:
                    cCount += comboCount;

                    break;
                case BlockTypes.D:
                    dCount += comboCount;

                    break;
                case BlockTypes.E:
                    eCount += comboCount;

                    break;
            }
        Current += comboCount;
        Bar.fillAmount = Current/capacity;
        Bar.material.SetColor("_Colour", Color.red);
        
    }
}
