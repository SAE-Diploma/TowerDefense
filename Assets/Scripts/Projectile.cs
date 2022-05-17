using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 6f;
    int damage = 1;
    float hitDistance = 1f;
    GameObject enemy;
    Quaternion offsetRotation;

    void Start()
    {
        offsetRotation = Quaternion.AngleAxis(90, transform.right);
        transform.rotation *= offsetRotation;
    }

    void Update()
    {
        // rotate towards the enemy
        if (enemy != null)
        {
            transform.LookAt(enemy.transform);
        }
        transform.rotation *= offsetRotation;

        // Move towards enemy
        transform.localPosition += transform.up * speed * Time.deltaTime;

        // Check if hit enemy
        Vector3 dir = enemy.transform.position - transform.position;
        if (dir.sqrMagnitude < Mathf.Pow(hitDistance, 2))
        {
            EnemyHit();
        }
    }

    /// <summary>
    /// runs when the projectile hit the enemy
    /// </summary>
    private void EnemyHit()
    {
        // inflict damage to enemy
        Destroy(gameObject);
        Destroy(enemy);
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
