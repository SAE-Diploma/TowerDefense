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

    [Header("Towers")]
    [SerializeField] Transform towerParent;
    [SerializeField] Tower[] towers;

    private int _coins = 0;
    public int Coins
    {
        get { return _coins; }
        private set
        {
            _coins = value;
            if (coinsText != null) { coinsText.text = _coins.ToString(); }
        }
    }

    private Stack<Menus> currentOpenMenu = new Stack<Menus>();

    void Start()
    {
        Cursor.visible = defaultCursorVisibility;
        Coins += 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Menus topMostMenu = currentOpenMenu.Pop();
            CloseMenu(topMostMenu);
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

    public void PlaceTower(Tower towerToSpawn)
    {
        if (Player.PlaceTower != null && towerToSpawn != null)
        {
            if (Player.PlaceTower.Tower == null)
            {
                if (Coins >= towerToSpawn.Cost)
                {
                    GameObject tower = Instantiate(towerToSpawn.TowerPrefab, Player.PlaceTower.Place.position, Quaternion.identity, towerParent);
                    TowerBase towerBase = tower.GetComponent<TowerBase>();
                    towerBase.SetTower(towerToSpawn);
                    Player.PlaceTower.SetTower(tower);
                    Coins -= towerToSpawn.Cost;
                    CloseMenu(Menus.ChooseTower);
                }
            }
        }
    }

    public void DeleteTower()
    {
        TowerPlace towerPlace = Player.PlaceTower;
        Coins += Mathf.RoundToInt(towerPlace.Tower.GetComponent<TowerBase>().Tower.Cost * 0.75f);
        Destroy(towerPlace.Tower);
        towerPlace.SetTower(null);
        CloseMenu(Menus.TowerUpgrades);
    }

    public void UpgradeTower(int towerStatIndex)
    {
        TowerStat stat = (TowerStat)towerStatIndex;
        TowerBase towerBase = Player.PlaceTower.Tower.GetComponent<TowerBase>();
        int newCoinValue = towerBase.UpgradeStat(stat,Coins);
        if (newCoinValue != -1)
        {
            Coins = newCoinValue;
            uiManager.UpdateTowerUpgradePanel(towerBase);
        }
        else
        {
            uiManager.UpdateTowerMenuCoinError(stat);
        }
    }

    #region UI Stuff

    /// <summary>
    /// Shows the menu
    /// </summary>
    /// <param name="menu">the menu to show</param>
    /// <param name="movement">can the player move when the menu is open</param>
    /// <param name="showCursor">should the cursor be visiblie in the menu</param>
    public void OpenMenu(Menus menu, bool movement = false, bool showCursor = true)
    {
        uiManager.SetMenuVisibility(menu, true);
        Player.SetMovability(movement);
        currentOpenMenu.Push(menu);
        Cursor.visible = true;

        switch (menu)
        {
            case Menus.TowerUpgrades:
                uiManager.UpdateTowerUpgradePanel(Player.PlaceTower.Tower.GetComponent<TowerBase>());
                break;
        }
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
        if (menu == currentOpenMenu.Peek())
        {
            currentOpenMenu.Pop();
        }
        else
        {
            Menus topMostMenu = currentOpenMenu.Pop();
            CloseMenu(topMostMenu);
        }
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
