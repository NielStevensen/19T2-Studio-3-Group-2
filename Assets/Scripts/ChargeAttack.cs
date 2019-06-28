using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class TwoDimArray
{
    public string name = "Type";

    public float[] matchUp;

    public TwoDimArray()
    {
        matchUp = new float[4] { 1, 1, 1, 1 };
    }
}

public class ChargeAttack : NetworkBehaviour
{
    [HideInInspector]
    public Image Bar;
    [HideInInspector]
    public Image healthBar;

    [Tooltip("total capacity of bar")]
    public float capacity;

    [Tooltip("current capacity of bar")]
    public float Current;

    [Tooltip("current health")]
    [SyncVar]
    public float health;
    [Tooltip("maximum health")]
    [SyncVar]
    public float maxhealth;

    [HideInInspector]
    public float[] Counts;
    public Sprite[] tiles;
    [HideInInspector]
    public Image Symbol;

    public int myType;

    int currentType;
    float currentHighest;

    public TwoDimArray[] matchUpMatrix = new TwoDimArray[4];

    private void Start()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    public void FillBar(BlockTypes colour, int chainCount, int comboCount)
    {
        switch (colour)
        {
            case BlockTypes.A:
                Counts[0] += comboCount * chainCount + comboCount;
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
        Bar.fillAmount = Current / capacity;
        if (Current > capacity)
        {
            Current = capacity;
        }


        for (int a = 0; a < 4; a++)
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

    [ClientRpc]
    void RpcDamage(float Damage, int type)
    {
        if (hasAuthority)
        {
            return;
        }
        health -= (Damage * matchUpMatrix[type].matchUp[myType]);
        healthBar.fillAmount = health / maxhealth;
    }
    [Command]
    void CmdDamage (float Damage, int type)
    {
        RpcDamage(Damage, type);
    }

    public void Update()
    {
        Bar.fillAmount = Current / capacity;
        if (Input.GetButtonDown("Jump"))
        {
            CmdDamage(Current / 30, currentType);
            Current = 0;
            for (int a = 0; a < 4; a++)
            {
               Counts[a] = 0f;
            } 
        }
    }
}