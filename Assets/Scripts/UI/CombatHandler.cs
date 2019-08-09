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

    public Color Barcolour;

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

    //[HideInInspector]
    public Image healthSymbol;

    public Text nameField;

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

    public void FillBar(int[] colour, int chainCount, int comboCount)
    {
        if (comboCount > 3)
        {
            Combos.Add(comboCount);
        }
        for (int a = 0; a < 5; a ++)
        {
            if (a < 4 && colour[a] > 0)
            {
                Counts[a] += colour[a] * chainCount + comboCount;
            }
            else
            {
                Heal(colour[a], comboCount);
            }
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
        Barcolour = Bar.color;
        if (opponent != null)
        {
            CmdUpdate(Current, capacity,currentType,health,maxhealth);
        }
    }
    [Command]
    void CmdUpdate(float currentCharge, float chargeCapacity, int typenum, float health, float maxhealth)
    {
        RpcUpdate(currentCharge,chargeCapacity, typenum, health,maxhealth);
    }

    [ClientRpc]
    void RpcUpdate(float currentCharge, float chargeCapacity, int typenum, float currenthealth, float healthcapacity)
    {
        if (isLocalPlayer)
        {
            return;
        }
        Bar.fillAmount = currentCharge / chargeCapacity;
        switch (typenum) // update colour based on the specified index
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
        Symbol.sprite = tiles[typenum]; // change bar to type specified by cmd

        healthBar.fillAmount = currenthealth / healthcapacity;
    }
    void Heal(int type, int combo)
    {
        if (type > 0)
        {
            health += (type + combo);
            healthBar.fillAmount = health / maxhealth;
            if (health > maxhealth)
            {
                health = maxhealth;
            }
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
            opponent.healthBar.fillAmount = opponent.health / opponent.maxhealth;
        }
    }

    public void Update()
    {
        if (isLocalPlayer)
        {
            healthBar.fillAmount = health / maxhealth;
            Bar.fillAmount = Current / capacity;
            CmdUpdate(Current, capacity, currentType, health, maxhealth);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Attack();
        }
        //abilities
        if (Input.GetKeyDown(KeyCode.Q) && Current > 200)
        {
            //switch based on the ability field of the charecter
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
                    StartCoroutine(attack2(secondatk)); //coroutine for wait between attacks
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
        //sort and print combos
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

        //sort and print chains
        Chains.Sort();

        highestValue = Chains[Chains.Count - 1];

        List<int> chainCount = new List<int>();

        for (int i = 0; i < highestValue; i++)
        {
            chainCount.Add(0);
        }

        for (int i = 0; i < Chains.Count; i++)
        {
            chainCount[Chains[i] - 1]++;
        }

        if (chainCount.Count > 3)
        {
            for (int a = 3; a < chainCount.Count; a++)
            {
                toPrint += "\n " + (a + 1) + " Chain: " + chainCount[a].ToString();
            }
        }
        // print to ui
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
            currentHighest = 0;
            dmgMod = 1;
            for (int a = 0; a < 4; a++)
            {
                Counts[a] = 0f;
            }
        }
    }

    void UpdateSave()
    {
        SaveData data = SaveSystem.LoadSave();

        //if the player win add 1 win to total wins
        if (didWin)
        {
            data.Wins += 1;
            data.currency += 10;
        }
       
        //update highest if it higher than current save
        if (Combos.Count > 0 && Combos[Combos.Count - 1] > data.HighestCombo)
        {
            data.HighestCombo = Combos[Combos.Count - 1];
            data.currency += (Combos[Combos.Count - 1]);
        }
        //update highest if it higher than current save
        if (Chains.Count > 0 && Chains[Chains.Count - 1] > data.HighestChain)
        {
            data.HighestChain = Chains[Chains.Count - 1];
            data.currency += (Chains[Chains.Count - 1]);
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

    public void UiLayout(int setIndex, int fighterIndex, string name)
    {
        tiles = FindObjectOfType<UiSetup>().sets[setIndex].set;
        healthSymbol.sprite = tiles[4];

        Symbol.sprite = tiles[AvatarStats.allFighters[fighterIndex].element];
        nameField.text = name;
    }
}