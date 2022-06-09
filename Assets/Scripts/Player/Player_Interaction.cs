using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum Interactions
{
    None,
    PlaceTower
}

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] GameManager manager;
    [SerializeField] UIManager uiManager;
    Player_Movement movement;

    [Header("Coins")]
    [SerializeField] float coinAttractionRange;
    [SerializeField] float coinAttractionSpeed;
    private Interactions currentInteraction;

    void Start()
    {
        movement = gameObject.GetComponent<Player_Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
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

    private void Interact()
    {
        switch (currentInteraction)
        {
            case Interactions.PlaceTower:
                uiManager.TogglePanel(Panels.ChooseTower);
                movement.CanPlayerMove(!movement.CanMove);
                break;
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
        if (other.gameObject.tag == "towerPlace")
        {
            uiManager.TogglePanel(Panels.Interaction);
            currentInteraction = Interactions.PlaceTower;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "towerPlace")
        {
            uiManager.TogglePanel(Panels.Interaction);
            currentInteraction = Interactions.None;
        }
    }


}
