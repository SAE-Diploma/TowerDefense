using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStat", fileName = "EnemyStat", order = 1)]


public class EnemyStats : ScriptableObject
{
    private Dictionary<EffectType,float> tempValues = new Dictionary<EffectType,float>();

    [SerializeField] private int health;
    public int Health => health;

    [SerializeField, Range(0f,100f)] private float armor;
    public float Armor => armor;

    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private float attackSpeed;
    public float AttackSpeed => attackSpeed;

    [SerializeField] private int coins;
    public int Coins => coins;

    [SerializeField] private int difficulty;
    public int Difficulty => difficulty;

    public void DecreaseStat(EffectType stat, float newValue)
    {
        if (!tempValues.ContainsKey(stat))
        {
            switch (stat)
            {
                case EffectType.Slowness:
                    tempValues.Add(stat, speed);
                    speed = newValue;
                    break;
            }
        }
    }

    public void ResetStat(EffectType stat)
    {
        if (tempValues.ContainsKey(stat))
        {
            float value = tempValues[stat];
            switch (stat)
            {
                case EffectType.Slowness:
                    speed = value;
                    break;
            }
            tempValues.Remove(stat);

        }
    }
}
