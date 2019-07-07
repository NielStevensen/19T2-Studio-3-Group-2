using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarStats : MonoBehaviour
{
    public StatBlock stats;
    public ChargeAttack refrence;

    public GameObject[] buttons;

    public void setclass()
    {
        // set charecter stats
        refrence.attack = stats.attack;
        refrence.maxhealth = stats.maxHealth;
        refrence.defence = stats.defence;
        refrence.myType = stats.element;

        refrence.health = refrence.maxhealth;
        foreach(GameObject a in buttons)
        {
            a.SetActive(false);
        }
    }

}


[System.Serializable]
public class StatBlock
{
    public int element;
    public float maxHealth;
    public float attack;
    public float defence;
    public string ability;

}
