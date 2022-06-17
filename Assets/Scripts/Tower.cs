using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower", fileName = "Tower", order = 1)]

public class Tower : ScriptableObject
{
    [SerializeField] private Towers towerType;
    public Towers TowerType => towerType;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private GameObject towerPrefab;
    public GameObject  TowerPrefab => towerPrefab;

    [SerializeField] private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [SerializeField] private int cost;
    public int Cost => cost;

    [Header("AttackSpeed")]
    [SerializeField, Tooltip("Rounds per second")] private float attackspeed;
    public float Attackspeed => attackspeed;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int attackspeedUpgradeCost;
    public int AttackspeedUpgradeCost => attackspeedUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int attackspeedUpgradeValue;
    public int AttackspeedUpgradeValue => attackspeedUpgradeValue;

    [Header("Damage")]
    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int damageUpgradeCost;
    public int DamageUpgradeCost => damageUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int damageUpgradeValue;
    public int DamageUpgradeValue => damageUpgradeValue;

    [Header("Range")]
    [SerializeField, Tooltip("In meters")] private float range;
    public float Range => range;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int rangeUpgradeCost;
    public int RangeUpgradeCost => rangeUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int rangeUpgradeValue;
    public int RangeUpgradeValue => rangeUpgradeValue;

    [Header("ProjectileSpeed")]
    [SerializeField, Tooltip("In meters per second")] private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int projectileSpeedUpgradeCost;
    public int ProjectileSpeedUpgradeCost => projectileSpeedUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int projectileSpeedUpgradeValue;
    public int ProjectileSpeedUpgradeValue => projectileSpeedUpgradeValue;

}
