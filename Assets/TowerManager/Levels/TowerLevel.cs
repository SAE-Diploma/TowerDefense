using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TowerLevel : ScriptableObject
{
    [SerializeField] float damage;
    public float Damage => damage;

    [SerializeField] float attackSpeed;
    public float AttackSpeed => attackSpeed;

    [SerializeField] float range;
    public float Range => range;

    [SerializeField] float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    [SerializeField] float rotationSpeed;
    public float RotationSpeed => rotationSpeed;
}

