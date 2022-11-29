using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameManager manager;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform coinsParent;
    [SerializeField] private GameObject checkpointsParent;
    [SerializeField] private Tresor tresor;
    [SerializeField] private List<Transform> checkpoints;

    private EnemyCollection distribution;
    private Dictionary<string, Enemy> enemyPrefabs;

    private void Start()
    {
        enemyPrefabs = WaveController.Instance.GetPrefabs();
    }


    public void SetDistribution(EnemyCollection newDistribution)
    {
        distribution = newDistribution;
    }

    private IEnumerator SpawnEnemies(float spawnDelay)
    {
        foreach (string type in distribution.getList())
        {
            if (enemyPrefabs[type] != null)
            {
                Enemy enemy = Instantiate(enemyPrefabs[type], transform.position, Quaternion.identity);
                enemy.SetCheckPoints(checkpoints);
                enemy.SetTresor(tresor);
                enemy.SetCoinParent(coinsParent);
                enemy.SetGameManager(manager);

            }
            else Debug.Log("Prefab was null");
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void StartSpawning(float spawnDelay)
    {
        if (enemyPrefabs == null) enemyPrefabs = WaveController.Instance.GetPrefabs();
        StartCoroutine(SpawnEnemies(spawnDelay));
    }

}
