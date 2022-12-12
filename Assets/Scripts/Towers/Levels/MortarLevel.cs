using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mortar_Level", menuName = "Tower/Level/Mortar", order = 0)]
public class MortarLevel : TowerLevel
{
    [SerializeField] float explosionRadius;
    public float ExplosionRadius => explosionRadius;

    [SerializeField] float explosionDamage;
    public float ExplosionDamage => explosionDamage;
}
