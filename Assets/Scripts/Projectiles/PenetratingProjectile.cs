using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetratingProjectile : Projectile
{
    [SerializeField] public int maxPenetrations = 0;
    [SerializeField] public int damageReduction = 10;
    int penetrations = 0;

    protected override void AfterEnemyHit()
    {
        if (penetrations >= maxPenetrations)
        {
            base.AfterEnemyHit();
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
