using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// necessary to serialize an integer value
/// </summary>
public struct SerializableInt
{
    [SerializeField] private int value;
    public int Value => value;

    public SerializableInt(int value)
    {
        this.value = value;
    }

    /// <summary>
    /// set the value
    /// </summary>
    /// <param name="value">integer value</param>
    public void SetValue(int value)
    {
        this.value = value;
    }
}


public class SaveFile : MonoBehaviour
{
    private string filePath;

    private bool loaded = false;

    public bool Loaded => loaded;


    [SerializeField] private SerializableInt points = new SerializableInt(0);
    public int Points => points.Value;
    public void SetPoints(int newValue) { points.SetValue(newValue); }

    private PermanentUpgrade[] permanentUpgrades;
    public PermanentUpgrade[] PermanentUpgrades => permanentUpgrades;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        init();
    }

    public void init()
    {
        filePath = Application.dataPath + "/Saves/save.json";
        permanentUpgrades = new PermanentUpgrade[3];
        InitializeDefaultValues();

        if (File.Exists(filePath))
        {
            Load();
            loaded = true;
        }
    }

    /// <summary>
    /// Initialize the permanentUpgrades values
    /// </summary>
    public void InitializeDefaultValues()
    {
        points.SetValue(0);

        // Balliste
        permanentUpgrades[0] = new PermanentUpgrade(Towers.Balliste, true,
            1f, 0.5f, 25, 35,     // Attackspeed
            10, 2, 40, 50,        // Damage
            7.5f, 2.5f, 30, 40,   // Range
            20, 1.5f, 40, 50);    // ProjectileSpeed

        // Minigun
        permanentUpgrades[1] = new PermanentUpgrade(Towers.Minigun, false,
            10f, 1.5f, 25, 35,      // Attackspeed
            5, 1, 50, 60,      // Damage
            5, 1.5f, 40, 50,      // Range
            10, 1.5f, 30, 40);     // ProjectileSpeed

        // Sniper
        permanentUpgrades[2] = new PermanentUpgrade(Towers.Sniper, false,
            0.8f, 0.5f, 50, 60,      // Attackspeed
            20, 5, 30, 40,      // Damage
            15, 5f, 25, 35,      // Range
            15, 2, 30, 40);     // ProjectileSpeed
    }

    /// <summary>
    /// Saves the Points and the PermanentUpgrades
    /// </summary>
    public void Save()
    {
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        string jsonString = string.Empty;
        jsonString += JsonUtility.ToJson(points, true) + ";";
        foreach (PermanentUpgrade towerUpgrades in permanentUpgrades)
        {
            jsonString += JsonUtility.ToJson(towerUpgrades, true) + ";";
        }
        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        File.WriteAllText(filePath, jsonString);
    }

    /// <summary>
    /// Loades the Points and PermanentUpgrades from the file
    /// </summary>
    private void Load()
    {
        if (File.Exists(filePath))
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

}
