using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;

    [Header("Movement")]
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 nextPoint;
    [SerializeField] GameObject checkpointsParent; // only for testing 
    List<Vector3> checkpoints;
    int currentCheckpointIndex = 0;
    [SerializeField] float speed;

    [Header("On Death")]
    [SerializeField] int coinsDropped;
    [SerializeField] GameObject coinPrefab;

    void Start()
    {
        checkpoints = new List<Vector3>();
        SetCheckPoints(checkpointsParent);
        currentCheckpointIndex = 0;
    }

    void Update()
    {
        //transform.Translate(movement*speed*Time.deltaTime);
        if (currentCheckpointIndex < checkpoints.Count)
        {
            MoveToCheckpoint(checkpoints[currentCheckpointIndex]);
        }
    }

    /// <summary>
    /// Move to the next checkpoint
    /// </summary>
    /// <param name="checkpoint">current ceckpoint</param>
    private void MoveToCheckpoint(Vector3 checkpoint)
    {
        Vector3 direction = checkpoint - transform.position;
        Debug.DrawLine(transform.position, transform.position+direction, Color.yellow, Time.deltaTime);
        Debug.DrawLine(transform.position, transform.position+transform.forward, Color.green, Time.deltaTime);
        transform.LookAt(checkpoint);
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
        float squareDist = Vector3.SqrMagnitude(direction);
        if (squareDist < 1f)
        {
            currentCheckpointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    /// <summary>
    /// On Enemy Death
    /// </summary>
    private void Die()
    {

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


}
