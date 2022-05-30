using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] GameManager manager;

    [Header("Coins")]
    [SerializeField] float coinAttractionRange;
    [SerializeField] float coinAttractionSpeed;

    void Start()
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
            Coin coin = other.transform.parent.parent.GetComponent<Coin>();
            manager.AddCoins(coin.Value);
            Destroy(other.gameObject);
        }
    }


}
