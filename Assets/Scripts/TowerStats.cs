using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Tower/Stat")]
public class TowerStats : ScriptableObject
{
    [SerializeField] List<TowerLevel> levelList;
    public List<TowerLevel> LevelList => levelList;

    [SerializeField] int buildCost;
    public int BuildCost => buildCost;

    [SerializeField] int unlockCost;
    public int UnlockCost => unlockCost;

    [SerializeField] bool isUnlocked;
    public bool IsUnlocked => isUnlocked;

    [SerializeField] Tower towerPrefab;
    public Tower TowerPrefab => towerPrefab;

    [SerializeField] GameObject towerBlueprintPrefab;
    public GameObject TowerBlueprintPrefab => towerBlueprintPrefab;
}