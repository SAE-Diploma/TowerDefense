using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public struct SerializableInt
{
    [SerializeField] private int value;
    public int Value => value;

    public SerializableInt(int value)
    {
        this.value = value;
    }

    public void SetValue(int value)
    {
        this.value = value;
    }
}


public class SaveFile : MonoBehaviour
{
    private string filePath;

    [SerializeField] private SerializableInt points = new SerializableInt(0);
    public int Points => points.Value;

    public void SetPoints(int newValue) { points.SetValue(newValue); }

    private PermanentUpgrade[] permanentUpgrades;
    public PermanentUpgrade[] PermanentUpgrades => permanentUpgrades;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        filePath = Application.dataPath + "/Saves/save.json";
        Debug.Log(filePath);
        permanentUpgrades = new PermanentUpgrade[3];
        InitializeDefaultValues();

        if (File.Exists(filePath)) Load();
        else
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            
            Save();
        }
    }

    private void InitializeDefaultValues()
    {
        points.SetValue(100);

        // Balliste
        permanentUpgrades[0] = new PermanentUpgrade(Towers.Balliste, true,
            0.5f, 0.5f, 25, 35,     // Attackspeed
            10  , 2   , 40, 50,     // Damage
            7.5f, 2.5f, 30, 40,     // Range
            6   , 1.5f, 40, 50);    // ProjectileSpeed

        // Minigun
        permanentUpgrades[1] = new PermanentUpgrade(Towers.Minigun, false,
            2f , 0.5f, 25, 35,      // Attackspeed
            5  , 1   , 50, 60,      // Damage
            5  , 1.5f, 40, 50,      // Range
            6  , 1.5f, 30, 40);     // ProjectileSpeed

        // Sniper
        permanentUpgrades[2] = new PermanentUpgrade(Towers.Sniper, false,
            0.4f, 0.5f, 50, 60,      // Attackspeed
            20  , 5   , 30, 40,      // Damage
            15  , 5f  , 25, 35,      // Range
            10  , 2   , 30, 40);     // ProjectileSpeed
    }

    public void Save()
    {
        Debug.Log("Save");
        string jsonString = string.Empty;
        jsonString += JsonUtility.ToJson(points, true) + ";";
        foreach(PermanentUpgrade towerUpgrades in permanentUpgrades)
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
                points = JsonUtility.FromJson<SerializableInt>(jsons[i]);
            }
            else
            {
                permanentUpgrades[i - 1].UpdateValues(JsonUtility.FromJson<PermanentUpgrade>(jsons[i]));
            }
        }
    }

}
