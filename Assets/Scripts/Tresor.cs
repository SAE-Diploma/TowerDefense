using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tresor : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] Slider healthBar;
    private bool isDestroyed = false;

    public int Health
    {
        get { return health; }
        private set
        {
            health = value;
            if (healthBar != null)
            {
                healthBar.value = health;
            }
        }
    }

    [SerializeField] private UnityEvent destroyed = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if (healthBar != null) healthBar.value = Health;
    }

    /// <summary>
    /// reduce health of the tresor
    /// </summary>
    /// <param name="damage">the amount of damage</param>
    public void TakeDamage(int damage)
    {
        if (Health > damage)
        {
            Health -= damage;
        }
        else
        {
            Health -= health;
            if (!isDestroyed)
            {
                Destroyed();
                isDestroyed = true;
            }
        }
    }

    /// <summary>
    /// when the tresor is Destroyed
    /// </summary>
    private void Destroyed()
    {
        // Display Opened Tresor
        destroyed.Invoke();
    }

}
