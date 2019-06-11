using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeAttack : MonoBehaviour
{

    public Image Bar;

    [Tooltip("total capacity of bar")]
    public float capacity;

    [Tooltip("current capacity of bar")]
    public float Current;

    public float aCount;
    public float bCount;
    public float cCount;
    public float dCount;
    public float eCount;

    void FillBar(BlockTypes colour,int chainCount, int comboCount)
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
        
    }
}
