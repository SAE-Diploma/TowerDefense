using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;
    int _coins = 0;
    public int Coins
    {
        get { return _coins; }
        private set
        {
            _coins = value;
            if (coinsText != null) { coinsText.text = _coins.ToString(); }
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Add coins to the coins counter
    /// </summary>
    /// <param name="amount">amount of coins</param>
    public void AddCoins(int amount)
    {
        Coins += amount;
    }

}
