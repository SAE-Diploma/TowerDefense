using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sniper_Level", menuName = "Tower/Level/Sniper", order = 0)]
public class SniperLevel : TowerLevel
{
    [SerializeField] int maxPenetrations;
    public int MaxPenetrations => maxPenetrations;

    [SerializeField] int damageReduction;
    public int DamageReduction => damageReduction;
}
