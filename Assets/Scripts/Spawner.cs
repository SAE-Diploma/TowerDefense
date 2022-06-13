using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform coinsParent;
    [SerializeField] GameObject checkpointsParent;
    [SerializeField] Tresor tresor;

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position,Quaternion.identity,transform);
        Enemy enemyClass = enemy.GetComponent<Enemy>();
        enemyClass.SetCheckPoints(checkpointsParent);
        enemyClass.SetTresor(tresor);
        enemyClass.SetCoinParent(coinsParent);
    }

}
