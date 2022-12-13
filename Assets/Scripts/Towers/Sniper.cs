using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Tower
{
    [SerializeField] int maxPenetrations = 1;
    [SerializeField] int damageReduction = 5;

    private void OnValidate()
    {
        if (projectile != null)
        {
            if (projectile.GetType() != typeof(PenetratingProjectile))
            {
                projectile = null;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetLevel(int newLevel)
    {
        base.SetLevel(newLevel);
        SniperLevel myLevel = (SniperLevel)currentLevelObj;
        maxPenetrations = myLevel.MaxPenetrations;
        damageReduction = myLevel.DamageReduction;
    }

    protected override void InitializeProjectile(Projectile newProjectile)
    {
        base.InitializeProjectile(newProjectile);
        PenetratingProjectile penetrating = (PenetratingProjectile)newProjectile;
        penetrating.maxPenetrations = maxPenetrations;
        penetrating.damageReduction = damageReduction;
    }
}
