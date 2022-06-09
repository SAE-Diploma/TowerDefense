using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;

    [Header("Movement")]
    [SerializeField] float moveSpeed;

    [Header("Checkpoints")]
    [SerializeField] float sqrDistToNextPoint;
    [SerializeField] float progress = 0;
    [SerializeField] GameObject checkpointsParent; // only for testing 
    List<Vector3> checkpoints;
    [SerializeField] int currentCheckpointIndex = 0;

    [Header("Attacking")]
    [SerializeField] float attackSpeed;
    [SerializeField] int damage;
    [SerializeField] Tresor tresor;
    private bool isAttacking = false;

    [Header("On Death")]
    [SerializeField] int coinsDropped;
    [SerializeField] GameObject coinPrefab;
    float randomDropDistance = 0.5f;


    void Start()
    {
        checkpoints = new List<Vector3>();
        SetCheckPoints(checkpointsParent);

        transform.position = checkpoints[currentCheckpointIndex];
    }

    void Update()
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
            checkpoints.Add(new Vector3(t.position.x, transform.position.y, t.position.z));
        }
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
            GameObject coinObject = Instantiate(coinPrefab, position, Quaternion.identity);
            Coin coin = coinObject.GetComponent<Coin>();
            coin.SetValue(1);
        }
        Destroy(gameObject);
    }
}