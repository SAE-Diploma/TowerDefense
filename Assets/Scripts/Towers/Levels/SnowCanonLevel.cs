using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnowCanon_Level", menuName = "Tower/Level/SnowCanon", order = 0)]
public class SnowCanonLevel : TowerLevel
{
    [SerializeField] float stacksPerShot;
    public float StacksPerShot => stacksPerShot;
}
