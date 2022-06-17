using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    private string filePath;

    [SerializeField] private int points = 0;
    public int Points => points;

    private PermanentUpgrades[] permanentUpgrades;
    public PermanentUpgrades[] PermanentUpgrades => permanentUpgrades;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        filePath = Application.dataPath + "/Saves/save.json";
        Debug.Log(filePath);
        permanentUpgrades = new PermanentUpgrades[3];
        if (File.Exists(filePath)) Load();
        else
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            InitializeDefaultValues();
            Save();
        }
    }

    private void InitializeDefaultValues()
    {
        points = 0;

        // Balliste
        permanentUpgrades[0] = new PermanentUpgrades(Towers.Balliste, true,
            0.5f, 0.5f, 2,  // Attackspeed
            10, 2, 2,       // Damage
            7.5f, 2.5f, 2,     // Range
            6, 1.5f, 2);    // ProjectileSpeed

        // Minigun
        permanentUpgrades[1] = new PermanentUpgrades(Towers.Minigun, true,
            2f, 0.5f, 2,  // Attackspeed
            5, 1, 2,       // Damage
            5, 1.5f, 2,     // Range
            6, 1.5f, 2);    // ProjectileSpeed

        // Sniper
        permanentUpgrades[2] = new PermanentUpgrades(Towers.Sniper, true,
            0.4f, 0.5f, 2,  // Attackspeed
            20, 5, 2,       // Damage
            15, 5f, 2,     // Range
            10, 2, 2);    // ProjectileSpeed
    }

    public void Save()
    {
        string jsonString = string.Empty;
        jsonString += JsonUtility.ToJson(this, true) + ";";
        foreach(PermanentUpgrades towerUpgrades in permanentUpgrades)
        {
            jsonString += JsonUtility.ToJson(towerUpgrades, true) + ";";
        }
        jsonString = jsonString.Substring(0,jsonString.Length - 1);
        File.WriteAllText(filePath, jsonString);
    }

    private void Load()
    {
        string jsonString = File.ReadAllText(filePath);
        string[] jsons = jsonString.Split(';');
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                points = JsonUtility.FromJson<int>(jsons[i]);
            }
            else
            {
                permanentUpgrades[i - 1] = JsonUtility.FromJson<PermanentUpgrades>(jsons[i]);
            }
        }
    }

}
