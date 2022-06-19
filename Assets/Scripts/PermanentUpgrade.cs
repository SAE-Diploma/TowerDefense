using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PermanentStats
{
    AttackspeedValueLevel,
    DamageValueLevel,
    RangeValueLevel,
    ProjectileSpeedValueLevel,
    AttackspeedMaxLevel,
    DamageMaxLevel,
    RangeMaxLevel,
    ProjectileSpeedMaxLevel
}

public class PermanentUpgrade
{
    private Towers towerType;
    public Towers TowerType => towerType;

    [SerializeField] private bool unlocked;
    public bool Unlocked => unlocked;

    public void Unlock() { unlocked = true; }

    // Attackspeed
    private float attackSpeedStartValue;
    public float AttackSpeedStartValue => attackSpeedStartValue + attackSpeedIncrementValue * (attackSpeedValueLevel - 1);

    private float attackSpeedIncrementValue;
    public float AttackSpeedIncrementValue => attackSpeedIncrementValue;

    private int attackSpeedValueCost;
    public int AttackSpeedValueCost => attackSpeedValueCost * attackSpeedValueLevel;

    [SerializeField] private int attackSpeedValueLevel = 1;
    public int AttackSpeedValueLevel => attackSpeedValueLevel;

    [SerializeField] private int attackSpeedMaxLevel = 1;
    public int AttackSpeedMaxLevel => attackSpeedMaxLevel;

    private int attackSpeedLevelCost;
    public int AttackSpeedLevelCost => attackSpeedLevelCost * AttackSpeedMaxLevel;


    // Damage
    private int damageStartValue;
    public int DamageStartValue => damageStartValue + damageIncrementValue * (damageValueLevel - 1);

    private int damageIncrementValue;
    public int DamageIncrementValue => damageIncrementValue;

    private int damageValueCost;
    public int DamageValueCost => damageValueCost * DamageValueLevel;

    [SerializeField] private int damageValueLevel = 1;
    public int DamageValueLevel => damageValueLevel;

    [SerializeField] private int damageMaxLevel = 1;
    public int DamageMaxLevel => damageMaxLevel;

    private int damageLevelCost;
    public int DamageLevelCost => damageLevelCost * damageMaxLevel;


    // Range
    private float rangeStartValue;
    public float RangeStartValue => rangeStartValue + rangeIncrementValue * (rangeValueLevel - 1);

    private float rangeIncrementValue;
    public float RangeIncrementValue => rangeIncrementValue;

    private int rangeValueCost;
    public int RangeValueCost => rangeValueCost * rangeValueLevel;

    [SerializeField] private int rangeValueLevel = 1;
    public int RangeValueLevel => rangeValueLevel;

    [SerializeField] private int rangeMaxLevel = 1;
    public int RangeMaxLevel => rangeMaxLevel;

    private int rangeLevelCost;
    public int RangeLevelCost => rangeLevelCost * RangeMaxLevel;


    // ProjectileSpeed
    private float projectileSpeedStartValue;
    public float ProjectileSpeedStartValue => projectileSpeedStartValue + projectileSpeedIncrementValue * (projectileSpeedValueLevel - 1);

    private float projectileSpeedIncrementValue;
    public float ProjectileSpeedIncrementValue => projectileSpeedIncrementValue;

    private int projectileSpeedValueCost;
    public int ProjectileSpeedValueCost => projectileSpeedValueCost * projectileSpeedValueLevel;

    [SerializeField] private int projectileSpeedValueLevel = 1;
    public int ProjectileSpeedValueLevel => projectileSpeedValueLevel;

    [SerializeField] private int projectileSpeedMaxLevel = 1;
    public int ProjectileSpeedMaxLevel => projectileSpeedMaxLevel;

    private int projectileSpeedLevelCost;
    public int ProjectileSpeedLevelCost => projectileSpeedLevelCost * projectileSpeedMaxLevel;


    public PermanentUpgrade(Towers towerType, bool unlocked,
        float attackSpeedStartValue, float attackSpeedIncrementValue, int attackSpeedLevelCost, int attackSpeedValueCost,
        int damageStartValue, int damageIncrementValue, int damageLevelCost, int damageValueCost,
        float rangeStartValue, float rangeIncrementValue, int rangeLevelCost, int rangeValueCost,
        float projectileSpeedStartValue, float projectileSpeedIncrementValue, int projectileSpeedLevelCost, int projectileSpeedValueCost)
    {
        this.towerType = towerType;
        this.unlocked = unlocked;
        this.attackSpeedStartValue = attackSpeedStartValue;
        this.attackSpeedIncrementValue = attackSpeedIncrementValue;
        this.attackSpeedLevelCost = attackSpeedLevelCost;
        this.damageStartValue = damageStartValue;
        this.damageIncrementValue = damageIncrementValue;
        this.damageLevelCost = damageLevelCost;
        this.rangeStartValue = rangeStartValue;
        this.rangeIncrementValue = rangeIncrementValue;
        this.rangeLevelCost = rangeLevelCost;
        this.projectileSpeedStartValue = projectileSpeedStartValue;
        this.projectileSpeedIncrementValue = projectileSpeedIncrementValue;
        this.projectileSpeedLevelCost = projectileSpeedLevelCost;
        this.projectileSpeedValueCost = projectileSpeedValueCost;
        this.rangeValueCost = rangeValueCost;
        this.damageValueCost = damageValueCost;
        this.attackSpeedValueCost = attackSpeedValueCost;
    }

    public void UpdateValues(PermanentUpgrade loaded)
    {
        unlocked = loaded.unlocked;
        attackSpeedValueLevel = loaded.AttackSpeedValueLevel;
        attackSpeedMaxLevel = loaded.AttackSpeedMaxLevel;
        damageValueLevel = loaded.DamageValueLevel;
        damageMaxLevel = loaded.DamageMaxLevel;
        rangeValueLevel = loaded.RangeValueLevel;
        rangeMaxLevel = loaded.RangeMaxLevel;
        projectileSpeedValueLevel = loaded.ProjectileSpeedValueLevel;
        projectileSpeedMaxLevel = loaded.ProjectileSpeedMaxLevel;
    }

    public void IncrementStat(PermanentStats stat)
    {
        switch (stat)
        {
            case PermanentStats.AttackspeedValueLevel:
                attackSpeedValueLevel++;
                break;
            case PermanentStats.AttackspeedMaxLevel:
                attackSpeedMaxLevel++;
                break;
            case PermanentStats.DamageValueLevel:
                damageValueLevel++;
                break;
            case PermanentStats.DamageMaxLevel:
                damageMaxLevel++;
                break;
            case PermanentStats.RangeValueLevel:
                rangeValueLevel++;
                break;
            case PermanentStats.RangeMaxLevel:
                rangeMaxLevel++;
                break;
            case PermanentStats.ProjectileSpeedValueLevel:
                projectileSpeedValueLevel++;
                break;
            case PermanentStats.ProjectileSpeedMaxLevel:
                projectileSpeedMaxLevel++;
                break;
        }
    }

    public int[] GetCostArray()
    {
        return new int[]
        {
            AttackSpeedValueCost,
            DamageValueCost,
            RangeValueCost,
            ProjectileSpeedValueCost,
            AttackSpeedLevelCost,
            DamageLevelCost,
            RangeLevelCost,
            ProjectileSpeedLevelCost
        };
    }

}
