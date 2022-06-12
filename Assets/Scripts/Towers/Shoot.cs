using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Transform spawnOrigin;

    private void Start()
    {
        spawnOrigin = transform.GetChild(0);
    }

    /// <summary>
    /// spawns a projectile and sets its stats
    /// </summary>
    /// <param name="enemy"></param>
    public void ShootAt(GameObject enemy, GameObject projectilePrefab, int damage = 1, float speed = 6f)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnOrigin.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetDamage(damage);
        projectileScript.SetSpeed(speed);
        projectileScript.SetEnemy(enemy);
    }
}
