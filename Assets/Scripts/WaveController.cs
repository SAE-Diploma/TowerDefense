using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;


[System.Serializable]
public struct EnemyCollection
{
    public int Standard;
    public int Fast;
    public int Tank;
    public int Warrior;
    public int Flying;

    public int GetSum()
    {
        return Standard + Fast + Tank + Warrior + Flying;
    }

    public void Print(string prefix = "")
    {
        Debug.Log($"{prefix + (prefix != string.Empty ? " -> " : "")}Standard:{Standard}, Fast:{Fast}, Tank:{Tank}, Warrior:{Warrior}, Flying:{Flying} [{GetSum()}]");
    }

    public static EnemyCollection operator -(EnemyCollection a, EnemyCollection b)
    {
        return new EnemyCollection()
        {
            Standard = a.Standard - b.Standard,
            Fast = a.Fast - b.Fast,
            Tank = a.Tank - b.Tank,
            Warrior = a.Warrior - b.Warrior,
            Flying = a.Flying - b.Flying
        };
    }
    public static EnemyCollection operator /(EnemyCollection a, int b)
    {
        return new EnemyCollection()
        {
            Standard = Mathf.FloorToInt(a.Standard / b),
            Fast = Mathf.FloorToInt(a.Fast / b),
            Tank = Mathf.FloorToInt(a.Tank / b),
            Warrior = Mathf.FloorToInt(a.Warrior / b),
            Flying = Mathf.FloorToInt(a.Flying / b)
        };
    }
    public static EnemyCollection operator *(EnemyCollection a, int b)
    {
        return new EnemyCollection()
        {
            Standard = a.Standard * b,
            Fast = a.Fast * b,
            Tank = a.Tank * b,
            Warrior = a.Warrior * b,
            Flying = a.Flying * b
        };
    }
    public static EnemyCollection operator *(EnemyCollection a, EnemyCollection b)
    {
        return new EnemyCollection()
        {
            Standard = a.Standard * b.Standard,
            Fast = a.Fast * b.Fast,
            Tank = a.Tank * b.Tank,
            Warrior = a.Warrior * b.Warrior,
            Flying = a.Flying * b.Flying
        };
    }

    public static string[] GetNames()
    {
        return new string[]
        {
            "Standard",
            "Fast",
            "Tank",
            "Warrior",
            "Flying"
        };
    }

    public string[] getList()
    {
        List<string> result = new List<string>();
        string[] names = GetNames();
        foreach (string name in names)
        {
            switch (name)
            {
                case "Standard":
                    result.AddRange(Enumerable.Repeat(name, Standard));
                    break;
                case "Fast":
                    result.AddRange(Enumerable.Repeat(name, Fast));
                    break;
                case "Tank":
                    result.AddRange(Enumerable.Repeat(name, Tank));
                    break;
                case "Warrior":
                    result.AddRange(Enumerable.Repeat(name, Warrior));
                    break;
                case "Flying":
                    result.AddRange(Enumerable.Repeat(name, Flying));
                    break;
            }
        }
        return result.ToArray();
    }
}

public class WaveController
{
    private Dictionary<int, EnemyCollection> fixedRatios = new Dictionary<int, EnemyCollection>()
    {
        {1, new EnemyCollection(){ Standard=100, Fast=0, Tank=0, Warrior=0,Flying=0 } },
        {5, new EnemyCollection(){ Standard=80, Fast=20, Tank=0, Warrior=0,Flying=0 } },
        {10, new EnemyCollection(){ Standard=60, Fast=20, Tank=20, Warrior=0,Flying=0 } },
        {15, new EnemyCollection(){ Standard=60, Fast=10, Tank=15, Warrior=10,Flying=5 } },
        {20, new EnemyCollection(){ Standard=40, Fast=15, Tank=20, Warrior=15,Flying=10 } },
    };

    private int currentWave = 0;
    public int CurrentWave
    {
        get { return currentWave; }
        set
        {
            currentWave = value;
            PrepareNextWave();
        }
    }

    private int currentDifficulty = 200;

    private Spawner[] spawners;
    private Dictionary<string, Enemy> enemyPrefabs;

    private static WaveController instance;
    public static WaveController Instance
    {
        get
        {
            if (instance == null) instance = new WaveController();
            return instance;
        }
    }

    // Temporary stats
    private EnemyCollection enemyDifficulty = new EnemyCollection() { Standard = 1, Fast = 2, Tank = 3, Warrior = 4, Flying = 5 };

    private WaveController()
    {

    }

    private EnemyCollection InterpolateWaveRatio(EnemyCollection from, EnemyCollection to, float percentace)
    {
        return new EnemyCollection()
        {
            Standard = Mathf.RoundToInt(from.Standard - (from.Standard - to.Standard) * percentace),
            Fast = Mathf.RoundToInt(from.Fast - (from.Fast - to.Fast) * percentace),
            Tank = Mathf.RoundToInt(from.Tank - (from.Tank - to.Tank) * percentace),
            Warrior = Mathf.RoundToInt(from.Warrior - (from.Warrior - to.Warrior) * percentace),
            Flying = Mathf.RoundToInt(from.Flying - (from.Flying - to.Flying) * percentace)
        };
    }

    private EnemyCollection GetEnemiesToSpawn(int currentWave, int waveDifficulty)
    {
        EnemyCollection ratio = GetWaveRatio(currentWave);

        return new EnemyCollection()
        {
            Standard = Mathf.FloorToInt(currentDifficulty * (ratio.Standard / 100f) / enemyDifficulty.Standard),
            Fast = Mathf.FloorToInt(currentDifficulty * (ratio.Fast / 100f) / enemyDifficulty.Fast),
            Tank = Mathf.FloorToInt(currentDifficulty * (ratio.Tank / 100f) / enemyDifficulty.Tank),
            Warrior = Mathf.FloorToInt(currentDifficulty * (ratio.Warrior / 100f) / enemyDifficulty.Warrior),
            Flying = Mathf.FloorToInt(currentDifficulty * (ratio.Flying / 100f) / enemyDifficulty.Flying)
        };
    }

    private EnemyCollection[] DistributeToSpawners(EnemyCollection enemyCounts, int spawnerCount)
    {
        EnemyCollection[] distribution = new EnemyCollection[spawnerCount];
        int sum = enemyCounts.GetSum();
        int spawnerIndex = 0;
        for (int i = 0; i < sum; i++)
        {
            spawnerIndex = Random.Range(0, spawnerCount);
            if (enemyCounts.Standard > 0)
            {
                distribution[spawnerIndex].Standard++;
                enemyCounts.Standard--;
            }
            else if (enemyCounts.Fast > 0)
            {
                distribution[spawnerIndex].Fast++;
                enemyCounts.Fast--;
            }
            else if (enemyCounts.Tank > 0)
            {
                distribution[spawnerIndex].Tank++;
                enemyCounts.Tank--;
            }
            else if (enemyCounts.Warrior > 0)
            {
                distribution[spawnerIndex].Warrior++;
                enemyCounts.Warrior--;
            }
            else if (enemyCounts.Flying > 0)
            {
                distribution[spawnerIndex].Flying++;
                enemyCounts.Flying--;
            }
            else
            {
                break;
            }
        }
        return distribution;
    }

    private EnemyCollection GetWaveRatio(int wave)
    {
        EnemyCollection ratio = new EnemyCollection();
        if (fixedRatios.Keys.Contains(wave))
        {
            ratio = fixedRatios[wave];
        }
        else if (wave > fixedRatios.Keys.Max())
        {
            ratio = fixedRatios[fixedRatios.Keys.Max()];
        }
        else
        {
            List<int> keys = fixedRatios.Keys.ToList();
            keys.Sort();
            for (int i = 0; i < keys.Count - 1; i++)
            {
                if (keys[i + 1] > wave)
                {
                    float diff = keys[i + 1] - keys[i];
                    ratio = InterpolateWaveRatio(fixedRatios[keys[i]], fixedRatios[keys[i + 1]], 1 / diff * (wave - keys[i]));
                    break;
                }
            }
        }
        return ratio;
    }

    private EnemyCollection[] GetDistribution(int spawnerCount)
    {
        return DistributeToSpawners(GetEnemiesToSpawn(currentWave, currentDifficulty), spawnerCount);
    }

    public void PrepareNextWave()
    {
        Spawner[] spawners = GetAllSpawners();
        EnemyCollection[] newDistribution = GetDistribution(spawners.Length);
        for (int i = 0; i < newDistribution.Length; i++)
        {
            spawners[i].SetDistribution(newDistribution[i]);
        }
        Debug.Log($"Wave {CurrentWave} prepared");
        StartWave();

    }

    public void StartWave()
    {
        foreach(Spawner spawner in GetAllSpawners())
        {
            spawner.StartSpawning(2f);
        }
    }

    public Spawner[] GetAllSpawners()
    {
        if (spawners == null)
        {
            spawners = GameObject.FindObjectsOfType<Spawner>();
        }
        return spawners;
    }

    public Dictionary<string, Enemy> GetPrefabs()
    {
        if (enemyPrefabs == null)
        {
            enemyPrefabs = new Dictionary<string, Enemy>();
            foreach(string type in EnemyCollection.GetNames())
            {
                Enemy prefab = Resources.Load<Enemy>($"Prefabs/Enemies/{type}");
                if (prefab != null)
                {
                    enemyPrefabs.Add(type, prefab);
                }
                else
                {
                    enemyPrefabs.Add(type, Resources.Load<Enemy>("Prefabs/Enemies/Standard"));
                }
            }
        }
        return enemyPrefabs;
    }


}
