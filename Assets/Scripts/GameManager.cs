using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool defaultCursorVisibility;
    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerController Player;
    [SerializeField] TextMeshProUGUI coinsText;
    int _coins = 0;

    [Header("Towers")]
    [SerializeField] Transform towerParent;
    [SerializeField] GameObject[] towerPrefabs;

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
        Cursor.visible = defaultCursorVisibility;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CloseMenu(Menus.ChooseTower);
        }
    }

    /// <summary>
    /// Add coins to the coins counter
    /// </summary>
    /// <param name="amount">amount of coins</param>
    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void PlaceTower(int towerIndex)
    {
        if (Player.PlaceTower != null)
        {
            if (Player.PlaceTower.Tower == null)
            {
                GameObject tower = Instantiate(towerPrefabs[towerIndex],Player.PlaceTower.Place.position,Quaternion.identity,towerParent);
                Player.PlaceTower.SetTower(tower);
            }
        }
    }

    #region UI Stuff

    /// <summary>
    /// Shows the menu
    /// </summary>
    /// <param name="panel">the menu to show</param>
    /// <param name="movement">can the player move when the menu is open</param>
    /// <param name="showCursor">should the cursor be visiblie in the menu</param>
    public void OpenMenu(Menus panel, bool movement = false, bool showCursor = true)
    {
        uiManager.SetMenuVisibility(Menus.ChooseTower, true);
        Player.SetMovability(movement);
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes a Menu and resets movement and cursorVisibility
    /// </summary>
    /// <param name="menu">The Menu to close</param>
    public void CloseMenu(Menus menu)
    {
        uiManager.SetMenuVisibility(menu, false);
        Player.SetMovability(true);
        Cursor.visible = defaultCursorVisibility;
    }

    /// <summary>
    /// Close Menu via the enumIndex. For Button
    /// </summary>
    /// <param name="enumIndex">index of the Menu</param>
    public void CloseMenu(int enumIndex)
    {
        CloseMenu((Menus)enumIndex);
    }

    #endregion

}