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
    #region variables
    public CombatHandler opponent;
    public CombatHandler[] scriptRefs;
    public GameObject statUI;

    public GameObject classSelection;
    public float attack;
    public float defence;
    public float dmgMod;
    public float defMod;
    public string ability;

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

    [SyncVar]
    int currentType;
    float currentHighest;

    public List<int> Chains;
    public List<int> Combos;

    public string toPrint = "";

    public TwoDimArray[] matchUpMatrix = new TwoDimArray[4];

    bool isleech = false;
    public bool didWin; // has the player won
    #endregion;
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
        CmdUpdate();
    }
    [Command]
    void CmdUpdate()
    {
        RpcUpdate();
    }

    [ClientRpc]
    void RpcUpdate()
    {
        opponent.Bar.fillAmount = opponent.Current / opponent.capacity;
        if (opponent.Current > opponent.capacity)
        {
            opponent.Current = opponent.capacity;
        }
        opponent.healthBar.fillAmount = opponent.health / opponent.maxhealth;
        if (opponent.health > opponent.maxhealth)
        {
            opponent.health = opponent.maxhealth;
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
            opponent.health -= ((Damage * matchUpMatrix[type].matchUp[myType]) * ((100-(defence + defMod)) /100));
            if(isleech) health += (Damage * matchUpMatrix[type].matchUp[myType]) * ((100 - (defence + defMod)) / 100)/2;
            isleech = false;
            opponent.defMod = 0;
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

        if (Input.GetKeyDown(KeyCode.Q) && Current > 200)
        {
            switch (ability)
            {
                case ("Fireball"):
                    dmgMod = 1.4f;
                    Attack();
                    return;

                case ("Leech"):
                    Attack();
                    isleech = true;
                    return;

                case ("DoubleShot"):
                    float secondatk = Current;
                    dmgMod = .6f;
                    Attack();
                    StartCoroutine(attack2(secondatk)); // wait between attacks
                    return;

                case ("Armour"):
                    defMod = 15;
                    return;
            }
        }

        //check win and lose conditions
        if (opponent != null && isLocalPlayer)
        {
            if (opponent.health <= 0)
            {
                GameObject Stats = GameObject.Instantiate(statUI);
                Stats.GetComponentsInChildren<Text>()[0].text = "Congratulations you win";
                didWin = true; // flag the player as winner
                printStats(Stats); // print stas and update save data
                this.enabled = false;
            }
            else if (health <= 0)
            {
                GameObject Stats = GameObject.Instantiate(statUI);
                Stats.GetComponentsInChildren<Text>()[0].text = "You lose";
                printStats(Stats); // print stas and update save data
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

        if(comboCount.Count > 3)
        {
            for (int a = 3; a < comboCount.Count; a++)
            {
                toPrint += "\n " + (a + 1) + " Combo: " + comboCount[a].ToString();
            }
        }
        stats.GetComponentsInChildren<Text>()[1].text += toPrint;
        UpdateSave(); // update save statistics
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
            dmgMod = 1;
            for (int a = 0; a < 4; a++)
            {
                Counts[a] = 0f;
            }
        }
    }

    void UpdateSave()
    {
        SaveData data;
        if (SaveSystem.LoadSave() != null)
        {
            data = SaveSystem.LoadSave();
        }
        else
        {
            data = new SaveData(0, 0, 0);
        }

        if (didWin)
        {
            data.Wins += 1;
        }
        if (Combos.Count > 0 && Combos[Combos.Count - 1] > data.HighestCombo)
        {
            data.HighestCombo = Combos[Combos.Count - 1];
        }
        if (Chains.Count > 0 && Chains[Chains.Count - 1] > data.HighestChain)
        {
            data.HighestChain = Chains[Chains.Count - 1];
        }
        SaveSystem.Save(data);
    }

    IEnumerator attack2(float secondatk)
    {
        yield return new WaitForSeconds(2);
        Counts[myType] = secondatk; // set power for second attack
        dmgMod = .6f;
        Attack();
    }
}