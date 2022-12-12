using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower", fileName = "Tower", order = 1)]

public class OLD_Tower : ScriptableObject
{
    [SerializeField] private Towers towerType;
    public Towers TowerType => towerType;

    [SerializeField] private EnemyType enemyType = EnemyType.Walking;
    public EnemyType EnemyType => enemyType;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private GameObject towerPrefab;
    public GameObject  TowerPrefab => towerPrefab;

    [SerializeField] private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [SerializeField] private int cost;
    public int Cost => cost;

    [SerializeField] private int unlockCost;
    public int UnlockCost => unlockCost;


    [Header("AttackSpeed")]
    [SerializeField, Tooltip("Rounds per second")] private float attackspeed;
    public float Attackspeed => attackspeed;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int attackspeedUpgradeCost;
    public int AttackspeedUpgradeCost => attackspeedUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private float attackspeedUpgradeValue;
    public float AttackspeedUpgradeValue => attackspeedUpgradeValue;

    [SerializeField, Tooltip("Maximum attackspeed Level")] private int attackspeedMaxLevel;
    public int AttackspeedMaxLevel => attackspeedMaxLevel;

    [Header("Damage")]
    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int damageUpgradeCost;
    public int DamageUpgradeCost => damageUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int damageUpgradeValue;
    public int DamageUpgradeValue => damageUpgradeValue;

    [SerializeField, Tooltip("Maximum damage Level")] private int damageMaxLevel;
    public int DamageMaxLevel => damageMaxLevel;

    [Header("Range")]
    [SerializeField, Tooltip("In meters")] private float range;
    public float Range => range;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int rangeUpgradeCost;
    public int RangeUpgradeCost => rangeUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int rangeUpgradeValue;
    public int RangeUpgradeValue => rangeUpgradeValue;

    [SerializeField, Tooltip("Maximum range Level")] private int rangeMaxLevel;
    public int RangeMaxLevel => rangeMaxLevel;

    [Header("ProjectileSpeed")]
    [SerializeField, Tooltip("In meters per second")] private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    [SerializeField, Tooltip("Added Cost per upgrade level")] private int projectileSpeedUpgradeCost;
    public int ProjectileSpeedUpgradeCost => projectileSpeedUpgradeCost;

    [SerializeField, Tooltip("Added Value per upgrade level")] private int projectileSpeedUpgradeValue;
    public int ProjectileSpeedUpgradeValue => projectileSpeedUpgradeValue;

    [SerializeField, Tooltip("Maximum projectileSpeed Level")] private int projectileSpeedMaxLevel;
    public int ProjectileSpeedMaxLevel => projectileSpeedMaxLevel;

    /// <summary>
    /// Apply the values read from the savefile
    /// </summary>
    /// <param name="upgrades">read PermanentUpgrade</param>
    public void ApplyPermanentUpgrade(PermanentUpgrade upgrades)
    {
        attackspeed = upgrades.AttackSpeedStartValue;
        attackspeedMaxLevel = upgrades.AttackSpeedMaxLevel;

        damage = upgrades.DamageStartValue;
        damageMaxLevel = upgrades.DamageMaxLevel;

        range = upgrades.RangeStartValue;
        rangeMaxLevel = upgrades.RangeMaxLevel;

        projectileSpeed = upgrades.ProjectileSpeedStartValue;
        projectileSpeedMaxLevel = upgrades.ProjectileSpeedMaxLevel;

    }

}
