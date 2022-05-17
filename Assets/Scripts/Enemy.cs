using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Vector3 movement;
    [SerializeField] float speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(movement*speed*Time.deltaTime);
    }
}
