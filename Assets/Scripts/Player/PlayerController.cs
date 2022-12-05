using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum Interactions
{
    None,
    TowerPlace,
    StartNextWave
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager manager;
    Player_Movement movement;
    Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] float rayDistance;

    [Header("Coins")]
    [SerializeField] float coinAttractionRange;
    [SerializeField] float coinAttractionSpeed;

    private Interactions _currentInteraction;
    public Interactions CurrentInteraction
    {
        get { return _currentInteraction; }
        private set
        {
            if (canInteract)
            {
                _currentInteraction = value;
                manager.ShowInteractionE();
            }
            else
            {
                _currentInteraction = Interactions.None;
            }
        }
    }

    private bool canInteract = true;
    public void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
        if (!canInteract) CurrentInteraction = Interactions.None;
    }

    private TowerPlace placeTower;
    public TowerPlace PlaceTower => placeTower;

    void Start()
    {
        movement = gameObject.GetComponent<Player_Movement>();
        playerCamera = gameObject.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (transform.position.y < -50)
        {
            transform.position = new Vector3(38, 1, 6);
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

        if (Physics.Raycast(new Ray(playerCamera.transform.position, playerCamera.transform.forward), out RaycastHit hit, rayDistance))
        {
            if (hit.transform.tag == "towerPlace")
            {
                CurrentInteraction = Interactions.TowerPlace;
                placeTower = hit.transform.GetComponent<TowerPlace>();
            }
            else if (hit.transform.tag == "InteractibleShield")
            {
                if (hit.transform.name == "StartNextWave")
                {
                    CurrentInteraction = Interactions.StartNextWave;
                }
            }
            else
            {
                CurrentInteraction = Interactions.None;
                placeTower = null;
            }
        }
        else
        {
            CurrentInteraction = Interactions.None;
            placeTower = null;
        }

        // ray for interactibles
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            Coin coin = other.transform.parent.parent.GetComponent<Coin>();
            manager.AddCoins(coin.Value);
            Destroy(other.transform.parent.parent.gameObject);
        }
    }

    /// <summary>
    /// when the player presses E
    /// </summary>
    private void Interact()
    {
        switch (CurrentInteraction)
        {
            case Interactions.TowerPlace:
                if (placeTower.Tower == null)
                {
                    manager.OpenMenu(Menus.ChooseTower);
                }
                else
                {
                    manager.OpenMenu(Menus.TowerUpgrades);
                }
                break;
            case Interactions.StartNextWave:
                manager.StartNextWave();
                break;
        }
    }

    /// <summary>
    /// set the canMove field from outside
    /// </summary>
    /// <param name="canMove">true|false</param>
    public void SetMovability(bool canMove)
    {
        movement.CanPlayerMove(canMove);
    }


}
