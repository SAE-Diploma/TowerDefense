using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform coinsParent;
    [SerializeField] GameObject checkpointsParent;
    [SerializeField] Tresor tresor;

    /// <summary>
    /// Spawns an enemy and sets its start values
    /// </summary>
    /// <param name="boss">if a boss should be spawned</param>
    public void SpawnEnemy(bool boss = false)
    {
        GameObject enemy;
        if (boss)
        {
            enemy = Instantiate(bossPrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {
            enemy = Instantiate(enemyPrefab, transform.position,Quaternion.identity,transform);
        }
        Enemy enemyClass = enemy.GetComponent<Enemy>();
        enemyClass.SetCheckPoints(checkpointsParent);
        enemyClass.SetTresor(tresor);
        enemyClass.SetCoinParent(coinsParent);
    }

}
