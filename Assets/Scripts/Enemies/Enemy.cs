using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyStats stats;
    public EnemyStats Stats => stats;

    [SerializeField] EnemyType type;
    public EnemyType Type => type;

    [SerializeField] GameObject coinPrefab;
    Dictionary<EffectType, StatusEffect> activeStatusEffects = new Dictionary<EffectType, StatusEffect>();
    Coroutine statusEffectCoroutine;

    DamageIndicator damageIndicator;

    private float currentHealth;
    public float CurrentHealth => currentHealth;

    private float incommingDamage = 0;
    public float IncommingDamage => incommingDamage;

    private float slowness = 1;
    public float CurrentSpeed => slowness * Stats.Speed;

    float randomOffset = 1f;

    Tresor tresor;
    private bool isAttacking = false;

    GameManager gameManager;
    Transform coinsParent;
    float randomDropDistance = 0.5f;

    // Checkpoints
    float sqrDistToNextPoint;
    [SerializeField] float progress = 0;
    public float Progress => progress;

    protected List<Vector3> checkpoints = new List<Vector3>();
    int currentCheckpointIndex = 0;

    Transform hitPosition;
    public Transform HitPosition
    {
        get
        {
            if (hitPosition == null)
            {
                foreach (Transform t in transform)
                {
                    if (t.tag == "Target")
                    {
                        hitPosition = t;
                        break;
                    }
                }
                if (hitPosition == null) hitPosition = transform;
            }
            return hitPosition;
        }
    }

    protected virtual void Start()
    {
        //transform.position = new Vector3(transform.position.x + Random.Range(-randomOffset, randomOffset), transform.position.y, transform.position.z + Random.Range(-randomOffset, randomOffset));
        currentHealth = Stats.Health;
        damageIndicator = GetComponentInChildren<DamageIndicator>();
    }

    void Update()
    {
        if (checkpoints.Count > 0)
        {
            if (currentCheckpointIndex < checkpoints.Count)
            {
                MoveToCheckpoint(checkpoints[currentCheckpointIndex]);
            }
            else
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackTresor(Stats.AttackSpeed));
                    isAttacking = true;
                }
            }
        }
    }

    /// <summary>
    /// Move to the next checkpoint
    /// </summary>
    /// <param name="checkpoint">current ceckpoint</param>
    private void MoveToCheckpoint(Vector3 checkpoint)
    {
        transform.LookAt(checkpoint);
        transform.position = transform.position + transform.forward * CurrentSpeed * Time.deltaTime;
        Vector3 direction = checkpoint - transform.position;
        float squareDist = Vector3.SqrMagnitude(direction);
        CalculateProgress(squareDist);
        if (squareDist < 1f)
        {
            currentCheckpointIndex++;
            if (currentCheckpointIndex < checkpoints.Count) sqrDistToNextPoint = (checkpoints[currentCheckpointIndex] - checkpoints[currentCheckpointIndex - 1]).sqrMagnitude;
        }
    }

    /// <summary>
    /// Calculate the percent of the track walked
    /// </summary>
    /// <param name="distance">distance to next checkpoint</param>
    private void CalculateProgress(float distance)
    {
        float percToNextPoint = 1 / sqrDistToNextPoint * (sqrDistToNextPoint - distance);
        progress = 1f / (checkpoints.Count - 1) * (currentCheckpointIndex - 1 + percToNextPoint);
    }

    public virtual void SetCheckPoints(List<Transform> checkPoints)
    {
        foreach (Transform t in checkPoints)
        {
            checkpoints.Add(new Vector3(t.position.x + UnityEngine.Random.Range(-randomOffset, randomOffset), transform.position.y, t.position.z + UnityEngine.Random.Range(-randomOffset, randomOffset)));
        }
    }

    /// <summary>
    /// Set the reference to the gamemanager
    /// </summary>
    /// <param name="refernce">gamanager</param>
    public void SetGameManager(GameManager refernce)
    {
        gameManager = refernce;
    }

    /// <summary>
    /// Set the tresor reference
    /// </summary>
    /// <param name="tresor"></param>
    public void SetTresor(Tresor tresor)
    {
        this.tresor = tresor;
    }

    /// <summary>
    /// set the coinsparent refernce form outside
    /// </summary>
    /// <param name="parent">transfrom</param>
    public void SetCoinParent(Transform parent)
    {
        coinsParent = parent;
    }

    /// <summary>
    /// Decrease health or die if low on health
    /// </summary>
    /// <param name="damage">incomming damage</param>
    public void TakeDamage(float damage, EffectType type = EffectType.None)
    {
        currentHealth -= damage;
        damageIndicator.AddDamageNumber(damage, type);
        if (currentHealth <= 0) Die();
    }

    /// <summary>
    /// Coroutine for attacking the tresor
    /// </summary>
    /// <param name="attackspeed">time between attacks</param>
    /// <returns></returns>
    private IEnumerator AttackTresor(float attackspeed)
    {
        while (true)
        {
            yield return new WaitForSeconds(attackspeed);
            if (tresor != null) tresor.TakeDamage(Stats.Damage);
            else { break; }
        }
    }

    /// <summary>
    /// On Enemy Death
    /// </summary>
    private void Die()
    {
        for (int i = 0; i < Stats.Coins; i++)
        {
            Vector3 position = new Vector3(transform.position.x + UnityEngine.Random.Range(-randomDropDistance, randomDropDistance), 0, transform.position.z + UnityEngine.Random.Range(-randomDropDistance, randomDropDistance));
            GameObject coinObject = Instantiate(coinPrefab, position, Quaternion.identity, coinsParent);
            Coin coin = coinObject.GetComponent<Coin>();
            coin.SetValue(1);
        }
        //gameManager.AddEnemyKilled();
        Destroy(gameObject);
    }

    /// <summary>
    /// adds damage to the incomming damage
    /// </summary>
    /// <param name="incommingDamage">incomming projectile damage</param>
    public void AddIncommingDamage(int incommingDamage)
    {
        this.incommingDamage += incommingDamage;
    }

    // Status Effects

    public void AddStatusEffect(StatusEffect effect)
    {
        if (!activeStatusEffects.ContainsKey(effect.EffectType))
        {
            StatusEffect copy = Instantiate(effect);
            if (copy.Stackable) copy.AddStack();
            activeStatusEffects.Add(copy.EffectType, copy); // store copy of the effect
        }
        else
        {
            StatusEffect activeEffect = activeStatusEffects[effect.EffectType];
            if (activeEffect.Stackable)
            {
                activeEffect.AddStack();
            }
            else
            {
                activeEffect.SetDuration(effect.Duration);
            }

        }
        if (statusEffectCoroutine == null)
        {
            statusEffectCoroutine = StartCoroutine(ApplyStatusEffects());
        }
    }

    private void RemoveStatusEffect(StatusEffect effect)
    {
        activeStatusEffects.Remove(effect.EffectType);
        if (activeStatusEffects.Count == 0 && statusEffectCoroutine != null)
        {
            StopCoroutine(statusEffectCoroutine);
            statusEffectCoroutine = null;
        }
    }

    private IEnumerator ApplyStatusEffects()
    {
        StatusEffect effect;
        while (true)
        {
            EffectType[] keys = activeStatusEffects.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                effect = activeStatusEffects[keys[i]];
                ApplyStatusEffect(effect, 1);
                effect.CountDownOneSecond();
                if (effect.Duration <= 0)
                {
                    RemoveStatusEffect(effect);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void ApplyStatusEffect(StatusEffect effect, int duration)
    {
        //Debug.Log($"applying status effect to {effect.EffectType}");
        switch (effect.EffectType)
        {
            case EffectType.Fire:
                TakeDamage(duration * effect.GetIntValue(), effect.EffectType);
                break;
            case EffectType.Poison:
                TakeDamage(duration * effect.GetIntValue(), effect.EffectType);
                break;
            case EffectType.Slowness:
                if (effect.Duration > 0) slowness = effect.Value; 
                else slowness = 1; 
                break;
        }
    }

}
