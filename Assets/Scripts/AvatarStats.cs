using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarStats : MonoBehaviour
{
    public StatBlock Knight;
    public StatBlock Archer;
    public StatBlock Priest;
    public StatBlock Wizard;
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
