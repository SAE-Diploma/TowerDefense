using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStat", fileName = "EnemyStat", order = 1)]


public class EnemyStats : ScriptableObject
{
    private Dictionary<EnemyStat,float> tempValues = new Dictionary<EnemyStat,float>();

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

    public void DecreaseStat(EnemyStat stat, float newValue)
    {
        if (!tempValues.ContainsKey(stat))
        {
            switch (stat)
            {
                case EnemyStat.Speed:
                    tempValues.Add(stat, speed);
                    speed = newValue;
                    break;
                case EnemyStat.Damage:
                    tempValues.Add(stat, damage);
                    damage = Mathf.RoundToInt(newValue);
                    break;
                case EnemyStat.Armor:
                    tempValues.Add(stat, armor);
                    damage = Mathf.RoundToInt(newValue);
                    break;
            }
        }
    }

    public void ResetStat(EnemyStat stat)
    {
        if (tempValues.ContainsKey(stat))
        {
            float value = tempValues[stat];
            switch (stat)
            {
                case EnemyStat.Speed:
                    speed = value;
                    break;
                case EnemyStat.Damage:
                    damage = Mathf.RoundToInt(value);
                    break;
                case EnemyStat.Armor:
                    armor = Mathf.RoundToInt(value);
                    break;
            }
            tempValues.Remove(stat);

        }
    }
}
