using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flamethrower_Level", menuName = "Tower/Level/Flamethrower", order = 0)]
public class FlamethrowerLevel : TowerLevel
{
    [SerializeField] float flamedamage;
    public float Flamedamage => flamedamage;
}
