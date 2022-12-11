using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStat", fileName = "EnemyStat", order = 1)]
[Serializable]
public class EnemyStats : ScriptableObject
{
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
}
