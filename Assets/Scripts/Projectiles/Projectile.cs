using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float speed = 6f;
    protected int damage = 50;
    Enemy enemy;
    Quaternion offsetRotation;
    List<Enemy> ignoredEnemies = new List<Enemy>();
    protected LayerMask enemyLayer;

    [SerializeField] StatusEffect statusEffect;

    private void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Start()
    {
        offsetRotation = Quaternion.AngleAxis(90, transform.right);
        transform.rotation *= offsetRotation;
    }

    void Update()
    {
        // rotate towards the enemy
        if (transform.position.y > 0 && transform.position.y < 30)
        {
            TrackEnemy(enemy);

            // Move towards enemy
            transform.localPosition += transform.up * speed * Time.deltaTime;
        }
        else
        {
            OnGroundHit();
        }

    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.up * 0.4f, out RaycastHit hitInfo,0.4f, enemyLayer))
        {
            Enemy enemy = hitInfo.transform.GetComponentInParent<Enemy>();
            if (enemy != null && !ignoredEnemies.Contains(enemy))
            {
                EnemyHit(enemy, damage);
                ignoredEnemies.Add(enemy);
            }
        }
    }

    /// <summary>
    /// runs when the projectile hit the enemy
    /// </summary>
    protected virtual void EnemyHit(Enemy enemy, int damage)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (statusEffect != null) enemy.AddStatusEffect(statusEffect); 
            AfterEnemyHit();
        }
        
    }

    protected virtual void AfterEnemyHit()
    {
        Destroy(gameObject);
    }

    protected virtual void OnGroundHit()
    {
        Destroy(gameObject);
    }

    protected virtual void TrackEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            transform.LookAt(enemy.HitPosition);
            transform.rotation *= offsetRotation;
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
