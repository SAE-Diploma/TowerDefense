using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyStats statsObject;
    private EnemyStats stats;
    private EnemyStats Stats
    {
        get
        {
            if (stats == null)
            {
                stats = Instantiate(statsObject);
            }
            return stats;
        }
    }

    [SerializeField] GameObject coinPrefab;
    Dictionary<EnemyStat, StatusEffect> activeStatusEffects = new Dictionary<EnemyStat, StatusEffect>();
    Coroutine statusEffectCoroutine;

    private int health;

    private int maxHealth = 1000;
    public int MaxHealth => maxHealth;

    private int incommingDamage = 0;
    public int IncommingDamage => incommingDamage;

    float randomOffset = 1f;

    Tresor tresor;
    private bool isAttacking = false;

    GameManager gameManager;
    Transform coinsParent;
    float randomDropDistance = 0.5f;

    // Checkpoints
    float sqrDistToNextPoint;
    float progress = 0;

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
        maxHealth = Stats.Health;
        health = maxHealth;
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
        transform.position = transform.position + transform.forward * Stats.Speed * Time.deltaTime;
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
            checkpoints.Add(new Vector3(t.position.x + Random.Range(-randomOffset, randomOffset), transform.position.y, t.position.z + Random.Range(-randomOffset, randomOffset)));
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
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
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
            Vector3 position = new Vector3(transform.position.x + Random.Range(-randomDropDistance, randomDropDistance), 0, transform.position.z + Random.Range(-randomDropDistance, randomDropDistance));
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
            Debug.Log($"add effect {effect.EffectType}");
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
            EnemyStat[] keys = activeStatusEffects.Keys.ToArray();
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
            case EnemyStat.Health:
                TakeDamage(duration * effect.GetIntValue());
                Debug.Log($"Took {duration * effect.GetIntValue()} poison damage");
                break;
            case EnemyStat.Speed:
            case EnemyStat.Damage:
            case EnemyStat.Armor:
                if (effect.Duration > 0)
                {
                    Stats.DecreaseStat(effect.EffectType, effect.Value);
                }
                else
                {
                    Stats.ResetStat(effect.EffectType);
                }
                break;
        }
    }

}
