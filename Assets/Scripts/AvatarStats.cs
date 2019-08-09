using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AvatarStats
{
    public static StatBlock[] allFighters = new StatBlock[4]
    {
        new StatBlock(2, 600, 18, 23, "Armour"),
        new StatBlock(1, 540, 21, 19, "DoubleShot"),
        new StatBlock(3, 680, 16, 18, "Leech"),
        new StatBlock(0, 580, 24, 17, "Fireball")
    };
    
    //public void setclass()
    //{
    //    // set charecter stats
    //    refrence.attack = stats.attack;
    //    refrence.maxhealth = stats.maxHealth;
    //    refrence.defence = stats.defence;
    //    refrence.myType = stats.element;
    //    refrence.health = refrence.maxhealth;
    //}
}


[System.Serializable]
public class StatBlock
{
    public int element;
    public float maxHealth;
    public float attack;
    public float defence;
    public string ability;

    public StatBlock(int _element, float _health, float _attack, float _defence, string _ability)
    {
        element = _element;
        maxHealth = _health;
        attack = _attack;
        defence = _defence;
        ability = _ability;
    }
}
