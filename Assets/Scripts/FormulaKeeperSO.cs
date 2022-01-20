using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Formula", menuName = "ScriptableObjects/FormulaKeeperSO", order = 1)]
public class FormulaKeeperSO : ScriptableObject
{
    [SerializeField] private int fullDamage;

    [SerializeField] private int lessDamage;

    public int CalcDamage(int weaponType, int targetType)
    {
        if (weaponType == targetType)
        {
            return fullDamage;
        }
        else
        {
            return lessDamage;
        }
    }
}
