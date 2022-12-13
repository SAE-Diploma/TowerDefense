using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Tower
{
    [SerializeField] float explosionRadius = 1f;

    protected override void SetLevel(int newLevel)
    {
        base.SetLevel(newLevel);
        MortarLevel myLevel = (MortarLevel)currentLevelObj;
        explosionRadius = myLevel.ExplosionRadius;
    }

    protected override void InitializeProjectile(Projectile newProjectile)
    {
        base.InitializeProjectile(newProjectile);
        ((ExplodingProjectile)newProjectile).radius = explosionRadius;
    }

}
