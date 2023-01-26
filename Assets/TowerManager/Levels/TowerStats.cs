using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Stat", menuName = "Tower/Stat")]
public class TowerStats : ScriptableObject
{
    [SerializeField] string towerName;
    public string TowerName { get { return towerName; } set { towerName = value; } }

    [SerializeField] string levelType = "";
    public Type LevelType { get { return Type.GetType(levelType); } set { levelType = value.AssemblyQualifiedName; } }

    [SerializeField] Sprite icon;
    public Sprite Icon { get { return icon; } set { icon = value; } }

    [SerializeField] bool isUnlocked;
    public bool IsUnlocked => isUnlocked;

    [SerializeField] int unlockCost;
    public int UnlockCost => unlockCost;

    [SerializeField] int buildCost;
    public int BuildCost => buildCost;

    [SerializeField] GameObject towerPrefab;
    public GameObject TowerPrefab => towerPrefab;

    [SerializeField] GameObject towerBlueprintPrefab;
    public GameObject TowerBlueprintPrefab => towerBlueprintPrefab;

    [SerializeField] List<TowerLevel> levelList;
    public List<TowerLevel> LevelList { get { return levelList; } set { levelList = value; } }
}
