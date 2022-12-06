using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 6f;
    int damage = 50;
    Enemy enemy;
    Quaternion offsetRotation;

    [SerializeField] StatusEffect statusEffect;
    [SerializeField] int maxPenetrations = 0;
    int penetrations = 0;

    List<Enemy> ignoredEnemies = new List<Enemy>();
    LayerMask mask;

    private void Awake()
    {
        mask = LayerMask.GetMask("Enemy");
    }

    void Start()
    {
        offsetRotation = Quaternion.AngleAxis(90, transform.right);
        transform.rotation *= offsetRotation;
    }

    void Update()
    {
        // rotate towards the enemy
        if (transform.position.y > 0)
        {
            if (penetrations == 0 && enemy != null)
            {
                transform.LookAt(enemy.HitPosition);
                transform.rotation *= offsetRotation;
            }

            // Move towards enemy
            transform.localPosition += transform.up * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.up * 0.4f, out RaycastHit hitInfo,0.4f, mask))
        {
            Enemy enemy = hitInfo.transform.GetComponentInParent<Enemy>();
            if (enemy != null && !ignoredEnemies.Contains(enemy))
            {
                EnemyHit(enemy);
                ignoredEnemies.Add(enemy);
            }
        }
    }

    /// <summary>
    /// runs when the projectile hit the enemy
    /// </summary>
    private void EnemyHit(Enemy enemyClass)
    {
        // inflict damage to enemy
        if (enemyClass != null)
        {
            enemyClass.TakeDamage(damage);
            if (statusEffect != null)
            {
                enemyClass.AddStatusEffect(statusEffect);
            }
        }
        if (penetrations >= maxPenetrations)
        {
            Destroy(gameObject);
        }
        else
        {
            penetrations++;
        }
    }

    /// <summary>
    /// set the enemy GameObject to home towards
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    /// <summary>
    /// set the projectile speed
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// set the projectile damage
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
