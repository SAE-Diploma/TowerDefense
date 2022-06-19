using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float randomOffset;


    [Header("Attacking")]
    [SerializeField] float attackSpeed;
    [SerializeField] int damage;
    Tresor tresor;
    private bool isAttacking = false;

    [Header("On Death")]
    [SerializeField] int coinsDropped;
    [SerializeField] GameObject coinPrefab;
    GameManager gameManager;
    Transform coinsParent;
    float randomDropDistance = 0.5f;

    // Checkpoints
    float sqrDistToNextPoint;
    float progress = 0;

    List<Vector3> checkpoints = new List<Vector3>();
    int currentCheckpointIndex = 0;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x + Random.Range(-randomOffset, randomOffset), transform.position.y, transform.position.z + Random.Range(-randomOffset, randomOffset));
    }

    void Update()
    {
        if (checkpoints.Count > 0)
        {
            if (currentCheckpointIndex < checkpoints.Count)
            {
                MoveToCheckpoint(checkpoints[currentCheckpointIndex]);
            }
            else
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackTresor(attackSpeed));
                    isAttacking = true;
                }
            }
        }
    }

    /// <summary>
    /// Move to the next checkpoint
    /// </summary>
    /// <param name="checkpoint">current ceckpoint</param>
    private void MoveToCheckpoint(Vector3 checkpoint)
    {
        transform.LookAt(checkpoint);
        transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        Vector3 direction = checkpoint - transform.position;
        float squareDist = Vector3.SqrMagnitude(direction);
        CalculateProgress(squareDist);
        if (squareDist < 1f)
        {
            currentCheckpointIndex++;
            if (currentCheckpointIndex < checkpoints.Count) sqrDistToNextPoint = (checkpoints[currentCheckpointIndex] - checkpoints[currentCheckpointIndex - 1]).sqrMagnitude;
        }
    }

    /// <summary>
    /// Calculate the percent of the track walked
    /// </summary>
    /// <param name="distance">distance to next checkpoint</param>
    private void CalculateProgress(float distance)
    {
        float percToNextPoint = 1 / sqrDistToNextPoint * (sqrDistToNextPoint - distance);
        progress = 1f / (checkpoints.Count - 1) * (currentCheckpointIndex - 1 + percToNextPoint);
    }

    /// <summary>
    /// Set the checkpoint positions from the enemyspawner
    /// </summary>
    /// <param name="cpParent"></param>
    public void SetCheckPoints(GameObject cpParent)
    {
        foreach (Transform t in cpParent.transform)
        {
            checkpoints.Add(new Vector3(t.position.x + Random.Range(-randomOffset, randomOffset), transform.position.y, t.position.z + Random.Range(-randomOffset, randomOffset)));
        }
    }

    /// <summary>
    /// Set the reference to the gamemanager
    /// </summary>
    /// <param name="refernce">gamanager</param>
    public void SetGameManager(GameManager refernce)
    {
        gameManager = refernce;
    }

    /// <summary>
    /// Set the tresor reference
    /// </summary>
    /// <param name="tresor"></param>
    public void SetTresor(Tresor tresor)
    {
        this.tresor = tresor;
    }

    /// <summary>
    /// set the coinsparent refernce form outside
    /// </summary>
    /// <param name="parent">transfrom</param>
    public void SetCoinParent(Transform parent)
    {
        coinsParent = parent;
    }

    /// <summary>
    /// Decrease health or die if low on health
    /// </summary>
    /// <param name="damage">incomming damage</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    /// <summary>
    /// Coroutine for attacking the tresor
    /// </summary>
    /// <param name="attackspeed">time between attacks</param>
    /// <returns></returns>
    private IEnumerator AttackTresor(float attackspeed)
    {
        while (true)
        {
            yield return new WaitForSeconds(attackspeed);
            if (tresor != null) tresor.TakeDamage(damage);
            else { break; }
        }
    }

    /// <summary>
    /// On Enemy Death
    /// </summary>
    private void Die()
    {
        for (int i = 0; i < coinsDropped; i++)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-randomDropDistance, randomDropDistance), 0, transform.position.z + Random.Range(-randomDropDistance, randomDropDistance));
            GameObject coinObject = Instantiate(coinPrefab, position, Quaternion.identity, coinsParent);
            Coin coin = coinObject.GetComponent<Coin>();
            coin.SetValue(1);
        }
        gameManager.AddEnemyKilled();
        Destroy(gameObject);
    }

}
