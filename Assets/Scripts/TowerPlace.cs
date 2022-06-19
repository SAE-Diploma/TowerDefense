using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlace : MonoBehaviour
{
    [SerializeField] Transform place;
    public Transform Place => place;

    GameObject tower;
    public GameObject Tower => tower;

    /// <summary>
    /// Set the reference to the tower gameobject
    /// </summary>
    /// <param name="tower">tower gameobject</param>
    public void SetTower(GameObject tower)
    {
        this.tower = tower;
    }
}
