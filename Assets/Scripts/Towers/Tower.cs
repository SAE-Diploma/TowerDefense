using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower", fileName = "Tower", order = 1)]

public class Tower : ScriptableObject
{
    [SerializeField] private string name;
    public string Name => name;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private GameObject towerPrefab;
    public GameObject  TowerPrefab => towerPrefab;

    [SerializeField] private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [SerializeField] private int cost;
    public int Cost => cost;

    [SerializeField, Tooltip("In meters")] private float range;
    public float Range => range;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField, Tooltip("Rounds per second")] private float attackspeed;
    public float Attackspeed => attackspeed;

    [SerializeField, Tooltip("In meters per second")] private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

}
