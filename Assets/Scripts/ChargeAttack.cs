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
    public Image healthBar;

    [Tooltip("total capacity of bar")]
    public float capacity;

    [Tooltip("current capacity of bar")]
    public float Current;

    [Tooltip("current health")]
    public float health;
    [Tooltip("maximum health")]
    public float maxhealth;
        

    //[HideInInspector]
    public float[] Counts;
    public Sprite[] tiles;
    public Image Symbol;

    int currentType;
    float currentHighest;
    

    private void Start() 
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    public void FillBar(BlockTypes colour,int chainCount, int comboCount)
    {
            switch (colour)
            {
                case BlockTypes.A:
                    Counts[0] += comboCount *chainCount + comboCount;

                    break;
                case BlockTypes.B:
                    Counts[1] += comboCount * chainCount + comboCount;

                    break;
                case BlockTypes.C:
                    Counts[2] += comboCount * chainCount + comboCount;

                    break;
                case BlockTypes.D:
                    Counts[3] += comboCount * chainCount + comboCount;

                    break;
                case BlockTypes.E:
                Heal(chainCount, comboCount);
                    break;
        }

        Current = Counts[0] + Counts[1] + Counts[2] + Counts[3];
        Bar.fillAmount = Current/capacity;
        if( Current > capacity)
        {
            Current = capacity;
        }
        

        for (int a = 0;a < 4; a++)
        {
            if (Counts[a] > currentHighest)
            {
                currentHighest = Counts[a];
                currentType = a;
                Symbol.sprite = tiles[a];
            }
        }
        switch (currentType)
        {
            case 0:
                Bar.material.SetColor("_Color", Color.red);
                break;
            case 1:
                Bar.material.SetColor("_Color", Color.yellow);
                break;
            case 2:
                Bar.material.SetColor("_Color", Color.green);
                break;
            case 3:
                Bar.material.SetColor("_Color", Color.blue);
                break;
        }
    }

    void Heal(int chain, int combo)
    {
        health += (chain + combo);
        healthBar.fillAmount = health / maxhealth;
        if (health > maxhealth)
        {
            health = maxhealth;
        }
    }
}
