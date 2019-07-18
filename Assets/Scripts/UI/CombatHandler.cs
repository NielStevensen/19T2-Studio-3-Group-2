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

    public List<int> Chains;
    public List<int> Combos;

    public string toPrint = "";

    public TwoDimArray[] matchUpMatrix = new TwoDimArray[4];

    bool isDead = false;
    public bool didWin; // has the player won

    private void Start()
    {
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
        if (comboCount > 3)
        {
            Combos.Add(comboCount);
        }
        if(Chains .Capacity > 0 &&Chains[Chains.Capacity -1] + 1 == chainCount)
        {
            Chains.Add(chainCount);
        }
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
                Bar.color = Color.red;
                break;
            case 1:
                Bar.color = Color.yellow;
                break;
            case 2:
                Bar.color = Color.green;
                break;
            case 3:
                Bar.color = Color.blue;
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
        Debug.Log((Damage * matchUpMatrix[type].matchUp[myType]) * ((100 - (defence + defMod)) / 100));
        if (hasAuthority)
        {
            return;
        }
        else
        {
            opponent.health -= ((Damage * matchUpMatrix[type].matchUp[myType]) * ((100-(defence + defMod)) /100));
        }
    }

    public void Update()
    {
       
        if (opponent != null)
        {
            opponent.healthBar.fillAmount = opponent.health / opponent.maxhealth;
        }
        healthBar.fillAmount = health / maxhealth;
        Bar.fillAmount = Current / capacity;

        if (Input.GetButtonDown("Jump"))
        {
            Attack();
        }
        //check win and lose conditions
        if (opponent != null && isLocalPlayer)
        {
            if (opponent.health <= 0)
            {
                GameObject Stats = GameObject.Instantiate(statUI);
                Stats.GetComponentsInChildren<Text>()[0].text = "Congratulations you win";
                printStats(Stats);
                this.enabled = false;
            }
            else if (health <= 0)
            {
                GameObject Stats = GameObject.Instantiate(statUI);
                Stats.GetComponentsInChildren<Text>()[0].text = "You lose";
                printStats(Stats);
                this.enabled = false;
            }
        }
    }

    public void printStats( GameObject stats)
    {
        Combos.Sort();

        int highestValue = Combos[Combos.Count - 1];

        List<int> comboCount = new List<int>();

        for(int i = 0; i < highestValue; i++)
        {
            comboCount.Add(0);
        }

        for(int i = 0; i < Combos.Count; i++)
        {
            comboCount[Combos[i] - 1]++;
        }

        print(Combos.Count);
        print(comboCount.Count);

        if(comboCount.Count > 3)
        {
            for (int a = 3; a < comboCount.Count; a++)
            {
                print(a - 1);

                toPrint += "\n " + (a + 1) + " Combo: " + comboCount[a].ToString();
            }
        }

        stats.GetComponentsInChildren<Text>()[1].text += toPrint;
    }
    public void Attack()
    {
        float stab;
        if (currentType == myType)
        {
            stab = 1.1f;
        }
        else
        {
            stab = 1f;
        }
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
}