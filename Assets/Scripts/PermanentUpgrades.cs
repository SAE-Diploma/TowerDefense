using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentUpgrades
{
    [SerializeField] private Towers towerType;
    public Towers TowerType => towerType;

    [SerializeField] private bool unlocked;
    public bool Unlocked => unlocked;

    // Attackspeed
    [SerializeField] private float attackSpeedStartValue;
    public float AttackSpeedStartValue => attackSpeedStartValue;

    [SerializeField] private float attackSpeedIncrementValue;
    public float AttackSpeedIncrementValue => attackSpeedIncrementValue;

    [SerializeField] private int attackSpeedMaxLevel;
    public int AttackSpeedMaxLevel => attackSpeedMaxLevel;

    // Damage
    [SerializeField] private int damageStartValue;
    public int DamageStartValue => damageStartValue;

    [SerializeField] private int damageIncrementValue;
    public int DamageIncrementValue => damageIncrementValue;

    [SerializeField] private int damageMaxLevel;
    public float DamageMaxLevel => damageMaxLevel;

    // Range
    [SerializeField] private float rangeStartValue;
    public float RangeStartValue => rangeStartValue;

    [SerializeField] private float rangeIncrementValue;
    public float RangeIncrementValue => rangeIncrementValue;

    [SerializeField] private int rangeMaxLevel;
    public int RangeMaxLevel => rangeMaxLevel;

    // ProjectileSpeed
    [SerializeField] private float projectileSpeedStartValue;
    public float ProjectileSpeedStartValue => projectileSpeedStartValue;

    [SerializeField] private float projectileSpeedIncrementValue;
    public float ProjectileSpeedIncrementValue => projectileSpeedIncrementValue;

    [SerializeField] private int projectileSpeedMaxLevel;
    public int ProjectileSpeedMaxLevel => projectileSpeedMaxLevel;


    public PermanentUpgrades(Towers towerType, bool unlocked,
        float attackSpeedStartValue, float attackSpeedIncrementValue, int attackSpeedMaxLevel,
        int damageStartValue, int damageIncrementValue, int damageMaxLevel,
        float rangeStartValue, float rangeIncrementValue, int rangeMaxLevel,
        float projectileSpeedStartValue, float projectileSpeedIncrementValue, int projectileSpeedMaxLevel)
    {
        this.towerType = towerType;
        this.unlocked = unlocked;
        this.attackSpeedStartValue = attackSpeedStartValue;
        this.attackSpeedIncrementValue = attackSpeedIncrementValue;
        this.attackSpeedMaxLevel = attackSpeedMaxLevel;
        this.damageStartValue = damageStartValue;
        this.damageIncrementValue = damageIncrementValue;
        this.damageMaxLevel = damageMaxLevel;
        this.rangeStartValue = rangeStartValue;
        this.rangeIncrementValue = rangeIncrementValue;
        this.rangeMaxLevel = rangeMaxLevel;
        this.projectileSpeedStartValue = projectileSpeedStartValue;
        this.projectileSpeedIncrementValue = projectileSpeedIncrementValue;
        this.projectileSpeedMaxLevel = projectileSpeedMaxLevel;
    }

}
