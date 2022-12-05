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
                    Debug.Log($"{stat} set from {speed} to {newValue}");
                    speed = newValue;
                    break;
                case EnemyStat.Damage:
                    tempValues.Add(stat, damage);
                    Debug.Log($"{stat} set from {damage} to {newValue}");
                    damage = Mathf.RoundToInt(newValue);
                    break;
                case EnemyStat.Armor:
                    tempValues.Add(stat, armor);
                    Debug.Log($"{stat} set from {armor} to {newValue}");
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
                    Debug.Log($"{stat} set back to {speed}");
                    break;
                case EnemyStat.Damage:
                    damage = Mathf.RoundToInt(value);
                    Debug.Log($"{stat} set back to {damage}");
                    break;
                case EnemyStat.Armor:
                    armor = Mathf.RoundToInt(value);
                    Debug.Log($"{stat} set back to {armor}");
                    break;
            }
            tempValues.Remove(stat);

        }
    }
}
