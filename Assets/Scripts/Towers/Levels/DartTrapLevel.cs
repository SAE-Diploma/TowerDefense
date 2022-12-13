using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DartTrap_Level", menuName = "Tower/Level/DartTrap", order = 0)]
public class DartTrapLevel : TowerLevel
{
    [SerializeField] float stacksPerShot;
    public float StacksPerShot => stacksPerShot;
}
