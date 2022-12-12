using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplodingProjectile : Projectile
{

    [SerializeField] float radius = 1f;
    [SerializeField, Range(0f,1f)] float maxReduction = 0;
    [SerializeField] ExplosionParticleEffect EffectPrefab;

    protected override void EnemyHit(Enemy enemyClass, float damage)
    {
        List<Collider> enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer).ToList();
        Explode();
        foreach (Collider c in enemies)
        {
            Enemy enemy = c.transform.GetComponentInParent<Enemy>();
            if (enemy != enemyClass)
            {
                float distance = Vector3.Distance(transform.position, c.transform.position);
                base.EnemyHit(enemy, CalcDamage(distance));
            }
        }
        base.EnemyHit(enemyClass, damage);
    }

    protected override void OnGroundHit()
    {
        Explode();
        base.OnGroundHit();
    }

    private int CalcDamage(float distance)
    {
        return Mathf.RoundToInt((1 - (1 - maxReduction) / radius * distance) * (float)damage);
    }

    private void Explode()
    {
        if (EffectPrefab != null) Instantiate(EffectPrefab, transform.position, Quaternion.identity);
    }
}
