using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetratingProjectile : Projectile
{
    [SerializeField] int maxPenetrations = 0;
    [SerializeField] int damageReduction = 10;
    int penetrations = 0;

    protected override void EnemyHit(Enemy enemy, int damage)
    {
        base.EnemyHit(enemy, damage);
        if (penetrations >= maxPenetrations)
        {
            Destroy(gameObject);
        }
        else
        {
            penetrations++;
            damage -= damageReduction;
            if (damage < 0) damage = 0;
        }
    }

    protected override void TrackEnemy(Enemy enemy)
    {
        if (penetrations == 0)
        {
            base.TrackEnemy(enemy);
        }
    }
}
