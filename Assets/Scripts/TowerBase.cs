using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    #region upgradeStats
    // Attackspeed
    private float attackSpeed; // rounds per seconds
    public float AttackSpeed => attackSpeed;

    private int attackSpeedLevel = 0;
    public int AttackSpeedLevel
    {
        get { return attackSpeedLevel; }
        private set { attackSpeedLevel = value; }
    }

    private int attackSpeedMaxLevel = 1;
    public int AttackSpeedMaxLevel => attackSpeedMaxLevel;

    // Damage
    private int damage;
    public int Damage => damage;

    private int damageLevel = 0;
    public int DamageLevel
    {
        get { return damageLevel; }
        private set { damageLevel = value; }
    }

    private int damageMaxLevel = 1;
    public int DamageMaxLevel => damageMaxLevel;

    // Range
    private float range;
    public float Range => range;

    private int rangeLevel = 0;
    public int RangeLevel
    {
        get { return rangeLevel; }
        private set { rangeLevel = value; }
    }

    private int rangeMaxLevel = 1;
    public int RangeMaxLevel => rangeMaxLevel;

    // ProjectileSpeed
    private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    private int projectileSpeedLevel = 0;
    public int ProjectileSpeedLevel
    {
        get { return projectileSpeedLevel; }
        private set { projectileSpeedLevel = value; }
    }

    private int projectileSpeedMaxLevel = 1;
    public int ProjectileSpeedMaxLevel => projectileSpeedMaxLevel;
    #endregion

    private Tower tower;
    public Tower Tower
    {
        get { return tower; }
        private set
        {
            tower = value;
            Initialize(tower);
        }
    }
    public void SetTower(Tower tower) { Tower = tower; }

    private TowerPlace place;
    public TowerPlace Place => place;
    public void SetTowerPlace(TowerPlace place) { this.place = place; }

    private int totalCoinsSpent = 0;
    public int TotalCoinsSpent => totalCoinsSpent;

    private GameObject projectilePrefab;
    private GameObject enemiesParent;
    private List<Enemy> enemiesInRange;
    private GameObject turret;
    private Shoot turretShoot;
    private bool shot = false;


    private void Start()
    {
        enemiesParent = GameObject.Find("Enemies");
        if (enemiesParent == null)
        {
            Debug.LogError("Enemies parent not found!");
            return;
        }
        enemiesInRange = new List<Enemy>();
        turret = GetTurret();
        turretShoot = turret.GetComponent<Shoot>();
    }

    private void Update()
    {
        // make sure that the enemiesParent was found
        if (enemiesParent == null)
        {
            Debug.LogError("Enemies parent not found!");
            return;
        }

        // get the enemy to aim at
        GetEnemiesInRange(ref enemiesInRange);
        if (enemiesInRange.Count > 0)
        {
            Enemy enemy = GetProritizedEnemy(enemiesInRange);
            if (enemy != null)
            {
                turret.transform.LookAt(enemy.transform);
                if (!shot)
                {
                    turretShoot.ShootAt(enemy, projectilePrefab, damage, projectileSpeed);
                    shot = true;
                    StartCoroutine(ShootCooldown(1 / attackSpeed));
                }
            }
        }
    }

    /// <summary>
    /// Set stats by a towerObject
    /// </summary>
    /// <param name="towerSpecs">TowerObject</param>
    private void Initialize(Tower towerSpecs)
    {
        attackSpeed = towerSpecs.Attackspeed;
        attackSpeedMaxLevel = towerSpecs.AttackspeedMaxLevel;
        damage = towerSpecs.Damage;
        damageMaxLevel = towerSpecs.DamageMaxLevel;
        range = towerSpecs.Range;
        rangeMaxLevel = towerSpecs.RangeMaxLevel;
        projectileSpeed = towerSpecs.ProjectileSpeed;
        projectileSpeedMaxLevel = towerSpecs.ProjectileSpeedMaxLevel;
        projectilePrefab = towerSpecs.ProjectilePrefab;
        totalCoinsSpent = towerSpecs.Cost;
    }

    /// <summary>
    /// get all enemies that are in range.
    /// </summary>
    /// <param name="enemies">reference to the enemiesInRange list</param>
    private void GetEnemiesInRange(ref List<Enemy> enemies)
    {
        enemies.Clear();
        foreach (Transform enemy in enemiesParent.transform)
        {
            Vector3 dir = new Vector3(enemy.position.x, 0, enemy.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            if (dir.sqrMagnitude < Mathf.Pow(range, 2))
            {
                enemies.Add(enemy.GetComponent<Enemy>());
            }
        }
    }

    /// <summary>
    /// get the enemy GameObject to shoot at.
    /// </summary>
    /// <param name="enemiesInRange">all enemies in Range</param>
    /// <returns>Enemy to shoot at</returns>
    private Enemy GetProritizedEnemy(List<Enemy> enemiesInRange)
    {
        // ToDoo:
        // Consider aming on the enemy that has the highes progression score
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy.IncommingDamage < enemy.MaxHealth)
            {
                return enemy;
            }
        }
        return null;
    }

    /// <summary>
    /// get the child with the Name "Turret"
    /// </summary>
    /// <returns>the GameObject of the Turret or null if not found</returns>
    private GameObject GetTurret()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Turret")
            {
                return child.gameObject;
            }
        }
        Debug.LogError("Turret not found!");
        return null;
    }

    /// <summary>
    /// waits for the cooldown before reseting the shot variable
    /// </summary>
    /// <param name="cooldown">time in second to wait before reseting</param>
    /// <returns></returns>
    private IEnumerator ShootCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        shot = false;
    }

    /// <summary>
    /// Increase a stat by one Level
    /// </summary>
    /// <param name="stat">stat to upgrade</param>
    /// <param name="coins">current coins</param>
    /// <returns>success: remaining coins | -1 : not enough coins </returns>
    public int UpgradeStat(TowerStat stat, int coins)
    {
        int cost = 0;
        switch (stat)
        {
            case TowerStat.Attackspeed:
                cost = (AttackSpeedLevel + 1) * Tower.AttackspeedUpgradeCost;
                if (coins >= cost)
                {
                    attackSpeedLevel++;
                    attackSpeed += Tower.AttackspeedUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }

            case TowerStat.Damage:
                cost = (DamageLevel + 1) * Tower.DamageUpgradeCost;
                if (coins >= cost)
                {
                    damageLevel++;
                    damage += Tower.DamageUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }

            case TowerStat.Range:
                cost = (RangeLevel + 1) * Tower.RangeUpgradeCost;
                if (coins >= cost)
                {
                    RangeLevel++;
                    range += Tower.RangeUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }

            case TowerStat.ProjectileSpeed:
                cost = (ProjectileSpeedLevel + 1) * Tower.ProjectileSpeedUpgradeCost;
                if (coins >= cost)
                {
                    ProjectileSpeedLevel++;
                    projectileSpeed += Tower.ProjectileSpeedUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }

            default:
                return -1;
        }

    }

}
