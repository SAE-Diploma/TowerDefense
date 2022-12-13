using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SMART_Level", menuName = "Tower/Level/SMART", order = 0)]
public class SMARTLevel : TowerLevel
{
    [SerializeField] float maxRicochets;
    public float MaxRicochets => maxRicochets;

    [SerializeField] int damageReduction;
    public int DamageReduction => damageReduction;
}
