using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 6f;
    int damage = 50;
    float hitDistance = 1f;
    GameObject enemy;
    Quaternion offsetRotation;

    [SerializeField] StatusEffect statusEffect;
    [SerializeField] int maxPenetrations = 0;
    int penetrations = 0;

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
                transform.LookAt(enemy.transform);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHit(other.gameObject.GetComponentInParent<Enemy>());
        }
        else if(other.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
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
    public void SetEnemy(GameObject enemy)
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
