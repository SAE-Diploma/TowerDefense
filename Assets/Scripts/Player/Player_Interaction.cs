using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] GameObject towers; // Parent GameObject of all Towers
    [SerializeField] float threshold; // how exactly you must look at a tower
    [SerializeField, Range(0, 10)] float sqrRange; // how great the distance between the tower and the player can be

    List<Transform> allTowers;
    List<Transform> towersInRange;

    // The Tower object that is looked at and valid to interact with
    GameObject selectedTower;
    public GameObject SelectedTower { get { return selectedTower; } }

    void Start()
    {
        // get all towers
        allTowers = new List<Transform>();
        Transform[] towersChildren = towers.GetComponentsInChildren<Transform>();
        foreach (Transform tower in towersChildren)
        {
            if (tower != towers.transform)
            {
                allTowers.Add(tower);
            }
        }
    }

    void Update()
    {
        foreach (Transform tower in allTowers)
        {
            Vector3 dir = tower.position - transform.position;
            float distance = Vector3.SqrMagnitude(dir);
            if (distance < sqrRange)
            {
                float cross = Vector3.Dot(dir.normalized, transform.forward.normalized);
                if (cross > threshold)
                {
                    selectedTower = tower.gameObject;
                    break;
                }
            }
            else
            {
                selectedTower = null;
            }
        }
    }

}
