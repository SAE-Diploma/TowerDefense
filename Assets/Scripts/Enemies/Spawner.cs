using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject checkpointsParent;
    [SerializeField] Tresor tresor;
    [SerializeField] float spawnIntervall;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave(spawnIntervall, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab, transform.position,Quaternion.identity,transform);
        Enemy enemyClass = enemy.GetComponent<Enemy>();
        enemyClass.SetCheckPoints(checkpointsParent);
        enemyClass.SetTresor(tresor);
    }

    public IEnumerator SpawnWave(float spawnDelay, int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy(enemyPrefab);
        }
    }

}
