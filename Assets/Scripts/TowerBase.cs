using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    // Attackspeed
    private float attackSpeed; // rounds per seconds
    public float AttackSpeed => attackSpeed;

    private int attackSpeedLevel = 1;
    public int AttackSpeedLevel
    {
        get { return attackSpeedLevel; }
        private set { attackSpeedLevel = value; }
    }

    // Damage
    private int damage;
    public int Damage => damage;

    private int damageLevel = 1;
    public int DamageLevel
    {
        get { return damageLevel; }
        private set { damageLevel = value; }
    }

    // Range
    private float range;
    public float Range => range;

    private int rangeLevel = 1;
    public int RangeLevel
    {
        get { return rangeLevel; }
        private set { rangeLevel = value; }
    }

    // ProjectileSpeed
    private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    private int projectileSpeedLevel = 1;
    public int ProjectileSpeedLevel
    {
        get { return projectileSpeedLevel; }
        private set { projectileSpeedLevel = value; }
    }

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

    private int totalCoinsSpent = 0;
    public int TotalCoinsSpent => totalCoinsSpent;

    private GameObject projectilePrefab;
    private GameObject enemiesParent;
    private List<GameObject> enemiesInRange;
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
        enemiesInRange = new List<GameObject>();
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
            GameObject enemy = GetProritizedEnemy(enemiesInRange);
            Debug.DrawLine(transform.position, enemy.transform.position, Color.red, Time.deltaTime);
            turret.transform.LookAt(enemy.transform);
            if (!shot)
            {
                turretShoot.ShootAt(enemy, projectilePrefab, damage, projectileSpeed);
                shot = true;
                StartCoroutine(ShootCooldown(1 / attackSpeed));
            }
        }
    }

    /// <summary>
    /// Set stats by a towerObject
    /// </summary>
    /// <param name="towerSpecs">TowerObject</param>
    private void Initialize(Tower towerSpecs)
    {
        range = towerSpecs.Range;
        damage = towerSpecs.Damage;
        attackSpeed = towerSpecs.Attackspeed;
        projectileSpeed = towerSpecs.ProjectileSpeed;
        projectilePrefab = towerSpecs.ProjectilePrefab;
        totalCoinsSpent = towerSpecs.Cost;
    }

    /// <summary>
    /// get all enemies that are in range.
    /// </summary>
    /// <param name="enemies">reference to the enemiesInRange list</param>
    private void GetEnemiesInRange(ref List<GameObject> enemies)
    {
        enemies.Clear();
        foreach (Transform enemy in enemiesParent.transform)
        {
            Vector3 dir = new Vector3(enemy.position.x, 0, enemy.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            if (dir.sqrMagnitude < Mathf.Pow(range, 2))
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }

    /// <summary>
    /// get the enemy GameObject to shoot at.
    /// </summary>
    /// <param name="enemiesInRange">all enemies in Range</param>
    /// <returns>Enemy to shoot at</returns>
    private GameObject GetProritizedEnemy(List<GameObject> enemiesInRange)
    {
        // ToDoo:
        // Consider aming on the enemy that has the highes progression score
        return enemiesInRange[0];
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
    /// <returns></returns>
    public int UpgradeStat(TowerStat stat, int coins)
    {
        int cost = 0;
        switch (stat)
        {
            case TowerStat.Attackspeed:
                cost = (AttackSpeedLevel) * this.Tower.AttackspeedUpgradeCost;
                if (coins >= cost)
                {
                    attackSpeedLevel++;
                    attackSpeed += this.Tower.AttackspeedUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }
            case TowerStat.Damage:
                cost = (DamageLevel) * this.Tower.DamageUpgradeCost;
                if (coins >= cost)
                {
                    damageLevel++;
                    damage += this.Tower.DamageUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }
            case TowerStat.Range:
                cost = (RangeLevel) * this.Tower.RangeUpgradeCost;
                if (coins >= cost)
                {
                    RangeLevel++;
                    range += this.Tower.RangeUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }
            case TowerStat.ProjectileSpeed:
                cost = (ProjectileSpeedLevel) * this.Tower.ProjectileSpeedUpgradeCost;
                if (coins >= cost)
                {
                    ProjectileSpeedLevel++;
                    projectileSpeed += this.Tower.ProjectileSpeedUpgradeValue;
                    totalCoinsSpent += cost;
                    return coins - cost;
                }
                else { return -1; }
            default:
                return -1;
        }

    }

}
