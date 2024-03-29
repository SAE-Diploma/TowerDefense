using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected EnemyType enemyType = EnemyType.Walking;
    [SerializeField] protected Priority priority = Priority.MostProgress;
    [SerializeField] protected float damage = 10;
    [SerializeField] protected float attackSpeed = 1;
    [SerializeField] protected float range = 5;
    [SerializeField] protected float rotationSpeed = 1;
    [SerializeField] protected float projectileSpeed = 1;
    [SerializeField, Tooltip("In Degrees")] protected float maxAngleToShoot = 10;
    [SerializeField] protected int level = 1;
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected TowerStats towerStats;

    private LayerMask enemyMask;
    protected List<Enemy> enemiesInRange = new List<Enemy>();
    protected Enemy prioritizedEnemy;

    protected Gun gun;
    private float angleDifference; // angle needed to turn until looking at enemy
    private Quaternion initialRotation;
    private bool canShoot = true;
    private Coroutine cooldownCoroutine;

    protected TowerLevel currentLevelObj;

    private void Awake()
    {
        enemyMask = LayerMask.GetMask("Enemy");
        gun = GetComponentInChildren<Gun>();
        if (gun == null) Debug.LogError($"No Gun found on turret {name}");
        initialRotation = transform.rotation;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetLevel(level);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetEnemiesInRange(ref enemiesInRange);
        RotateTowardsEnemy(prioritizedEnemy);
        if (prioritizedEnemy != null)
        {
            Shoot();
        }
    }

    /// <summary>
    /// get all enemies that are in range.
    /// </summary>
    /// <param name="enemies">reference to the enemiesInRange list</param>
    private void GetEnemiesInRange(ref List<Enemy> enemies)
    {
        enemies.Clear();
        prioritizedEnemy = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyMask);

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy.Type == enemyType || enemyType == EnemyType.Both)
            {
                enemies.Add(enemy);
                if (prioritizedEnemy == null) prioritizedEnemy = enemy;
                else prioritizedEnemy = GetPrioritized(prioritizedEnemy, enemy, priority);
            }
        }
    }

    /// <summary>
    /// Get the enemy with the higher priority
    /// </summary>
    /// <param name="current">The currently highest prioritized enemy</param>
    /// <param name="newEnemy">The enemy to compare to the current one</param>
    /// <param name="priority">The priority to sort by</param>
    /// <returns>The Enemy with the higher priority</returns>
    private Enemy GetPrioritized(Enemy current, Enemy newEnemy, Priority priority)
    {
        Enemy better = current;
        switch (priority)
        {
            case Priority.MostProgress:
                if (newEnemy.Progress > current.Progress) better = newEnemy;
                break;
            case Priority.LeastProgress:
                if (newEnemy.Progress < current.Progress) better = newEnemy;
                break;
            case Priority.MostSpeed:
                if (newEnemy.CurrentSpeed > current.CurrentSpeed) better = newEnemy;
                break;
            case Priority.LeastSpeed:
                if (newEnemy.CurrentSpeed < current.CurrentSpeed) better = newEnemy;
                break;
            case Priority.MostDamage:
                if (newEnemy.Stats.Damage > current.Stats.Damage) better = newEnemy;
                break;
            case Priority.MostArmor:
                if (newEnemy.Stats.Armor > current.Stats.Armor) better = newEnemy;
                break;
            case Priority.MostHealth:
                if (newEnemy.Stats.Armor > current.Stats.Armor) better = newEnemy;
                break;
            case Priority.LeastHealth:
                if (newEnemy.CurrentHealth < current.CurrentHealth) better = newEnemy;
                break;
        }
        return better;
    }

    /// <summary>
    /// Smoothly rotates towards an enemy
    /// </summary>
    /// <param name="enemy">enemy to rotate towards</param>
    private void RotateTowardsEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            Vector3 dir = enemy.transform.position - gun.MuzzleTransform.position;
            Quaternion destination = Quaternion.LookRotation(dir);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, Quaternion.Euler(0,destination.eulerAngles.y,0), rotationSpeed * Time.deltaTime);
            angleDifference = Vector3.Angle(gun.transform.forward, new Vector3(dir.x, 0, dir.z));
        }
        else
        {
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Shoots a projectile homing in to the prioritized enemy
    /// </summary>
    private void Shoot()
    {
        if (canShoot)
        {
            if (angleDifference <= maxAngleToShoot)
            {
                canShoot = false;
                cooldownCoroutine = null;
                Projectile newProjectile = Instantiate(projectile, gun.MuzzleTransform.position, Quaternion.identity);
                InitializeProjectile(newProjectile);
            }
        }
        else if (cooldownCoroutine == null)
        {
            cooldownCoroutine = StartCoroutine(ShootCooldown(1 / attackSpeed));
        }
    }

    /// <summary>
    /// Spawns a projectile at the gun muzzle
    /// </summary>
    protected virtual void InitializeProjectile(Projectile newProjectile)
    {
        newProjectile.Initialize(prioritizedEnemy, damage, projectileSpeed);
    }

    /// <summary>
    /// waits for the cooldown before reseting the canShoot variable
    /// </summary>
    /// <param name="cooldown">time in second to wait before reseting</param>
    /// <returns></returns>
    private IEnumerator ShootCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    protected virtual void SetLevel(int newLevel)
    {
        if (newLevel > towerStats.LevelList.Count) return; 
        Debug.Log($"Setting level {newLevel}");
        level = newLevel;
        currentLevelObj = towerStats.LevelList[newLevel - 1];
        damage = currentLevelObj.Damage;
        attackSpeed = currentLevelObj.AttackSpeed;
        range = currentLevelObj.Range;
        projectileSpeed = currentLevelObj.ProjectileSpeed;
        rotationSpeed = currentLevelObj.RotationSpeed;
    }

}
