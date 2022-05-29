using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] float coinAttractionRange;
    [SerializeField] float coinAttractionSpeed;
    [SerializeField] UnityEvent coinCollected;

    void Start()
    {
        if (coinCollected == null) coinCollected = new UnityEvent();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, coinAttractionRange);
        foreach (Collider overlap in overlaps)
        {
            // attract all coins in range
            if (overlap.gameObject.tag == "coin")
            {
                Transform obj = overlap.transform.parent.parent; // Top most parent of a coin
                Vector3 dir = transform.position - obj.position;
                obj.position += dir.normalized * coinAttractionSpeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            coinCollected.Invoke();
            Destroy(other.gameObject);
        }
    }


}
