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

public class CombatHandler : NetworkBehaviour
{
    public CombatHandler opponent;
    public CombatHandler[] scriptRefs;
    public GameObject statUI;

    public GameObject classSelection;
    public float attack;
    public float defence;
    public float dmgMod;
    public float defMod;

    [HideInInspector]
    public Image Bar;
    [HideInInspector]
    public Image healthBar;

    [Tooltip("total capacity of bar")]
    [SyncVar]
    public float capacity;

    [Tooltip("current capacity of bar")]
    [SyncVar]
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
        // open class selection on local player
        if (isLocalPlayer)
        {
            GameObject Selector  = Instantiate(classSelection);
            foreach(AvatarStats a in Selector.GetComponentsInChildren<AvatarStats>())
            {
                a.refrence = this;
            }
        }
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        if(GameObject.FindObjectsOfType<CombatHandler>().Length  > 1)
        {
            scriptRefs = GameObject.FindObjectsOfType<CombatHandler>();
            foreach (CombatHandler a in scriptRefs)
            {
                if (a != this)
                { 
                    // set a reference to opponents script then refrence yourself
                    opponent = a;
                    opponent.opponent = this;
                }
            }
        }
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

    [Command]
    void CmdDamage(float Damage, int type)
    {
        RpcDamage(Damage, type);
    }

    [ClientRpc]
    void RpcDamage(float Damage, int type)
    {
        if (hasAuthority)
        {
            return;
        }
        else
        {
            opponent.health -= ((Damage * matchUpMatrix[type].matchUp[myType]) * (100-(defence + defMod) /100));
        }
    }

    public void Update()
    {
        float stab;
        if (opponent != null)
        {
            opponent.healthBar.fillAmount = opponent.health / opponent.maxhealth;
        }
        healthBar.fillAmount = health / maxhealth;
        Bar.fillAmount = Current / capacity;
        if (currentType == myType)
        {
            stab = 1.1f;
        }
        else
        {
            stab = 1f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (isLocalPlayer)
            {
                CmdDamage(Current / 30 * attack * stab * dmgMod, currentType);
                Current = 0;
                for (int a = 0; a < 4; a++)
                {
                    Counts[a] = 0f;
                }
            }
        }
        //check win and lose conditions
        if (opponent != null && isLocalPlayer)
        {
            if (opponent.health <= 0)
            {
                GameObject Stats = GameObject.Instantiate(statUI);
                Stats.GetComponentInChildren<Text>().text = "Congratulations you win";
                this.enabled = false;
            }
            else if (health <= 0)
            {
                GameObject Stats = GameObject.Instantiate(statUI);
                Stats.GetComponentInChildren<Text>().text = "You lose";
                this.enabled = false;
            }
        }
    }
}