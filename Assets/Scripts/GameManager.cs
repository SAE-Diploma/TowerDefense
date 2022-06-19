using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool defaultCursorVisibility;
    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerController Player;

    [Header("Towers")]
    [SerializeField] Transform towerParent;

    [Header("Waves")]
    [SerializeField] Spawner spawner;
    [SerializeField] int maxWaves;
    [SerializeField] int startEnemyCount;
    [SerializeField] float enemyCountMultiplier;
    [SerializeField, Tooltip("In Seconds")] int timeBetweenWaves;
    [SerializeField] float spawnInterval;
    [SerializeField] float spawnIntervalMutliplier;

    private SaveFile saveFile;
    public SaveFile SaveFile => saveFile;

    private int _coins = 0;
    public int Coins
    {
        get { return _coins; }
        private set
        {
            _coins = value;
            uiManager.SetCoins(_coins);
        }
    }

    private int _currentWave;
    public int CurrentWave
    {
        get { return _currentWave; }
        private set
        {
            _currentWave = value;
            UpdateWaveCounter();
        }
    }

    private bool lastWaveSpawned = false;

    private Stack<Menus> currentOpenMenu = new Stack<Menus>();

    void Start()
    {
        Cursor.visible = defaultCursorVisibility;
        if (!defaultCursorVisibility) Cursor.lockState = CursorLockMode.Locked;
        GameObject saveFileObject = GameObject.Find("SaveManager");
        if (saveFileObject != null)
        {
            saveFile = saveFileObject.GetComponent<SaveFile>();
        }

        Coins += 100;
        CurrentWave = 1;
        StartCoroutine(WaitForNextWave(timeBetweenWaves));
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentOpenMenu.Count > 0)
            {
                Menus topMostMenu = currentOpenMenu.Peek();
                if (topMostMenu == Menus.PauseMenu)
                {
                    ResumeGame();
                }
                else
                {
                    CloseMenu(topMostMenu);
                }
            }
            else
            {
                PauseGame();
            }
        }

        if (lastWaveSpawned)
        {
            if (spawner.transform.childCount == 0)
            {
                OpenMenu(Menus.WinMenu);
            }
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

    #region PauseMenu Logic

    /// <summary>
    /// Pause the game and show the pause overlay
    /// </summary>
    public void PauseGame()
    {
        OpenMenu(Menus.PauseMenu);
        Time.timeScale = 0;
    }

    /// <summary>
    /// resume the game and hide the play overlay
    /// </summary>
    public void ResumeGame()
    {
        CloseMenu(Menus.PauseMenu);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Load the main menu scene
    /// </summary>
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// reload the game scene
    /// </summary>
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// When the Game is lost ie. the tresor has 0 health
    /// </summary>
    public void LooseGame()
    {
        OpenMenu(Menus.LooseMenu);
    }

    #endregion

    #region WaveLogic

    /// <summary>
    /// Cooldown bevor next Wave is spawning
    /// </summary>
    /// <param name="timeInSeconds">time between waves</param>
    /// <returns></returns>
    private IEnumerator WaitForNextWave(int timeInSeconds)
    {
        int remaining = timeInSeconds;
        uiManager.ShowRemainingTime(remaining);

        while (remaining > 0)
        {
            yield return new WaitForSeconds(1);
            remaining--;
            uiManager.ShowRemainingTime(remaining);
        }
        StartCoroutine(SpawnWave(spawnInterval * Mathf.Pow(spawnIntervalMutliplier, CurrentWave - 1), Mathf.RoundToInt(startEnemyCount * Mathf.Pow(enemyCountMultiplier, CurrentWave - 1))));
    }

    /// <summary>
    /// Spawns a wave
    /// </summary>
    /// <param name="spawnDelay">time between spawns</param>
    /// <param name="enemyCount">how many enemies should be spawned</param>
    /// <returns></returns>
    private IEnumerator SpawnWave(float spawnDelay, int enemyCount)
    {
        bool isLastWave = CurrentWave == maxWaves;

        for (int i = 0; i < enemyCount; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            if (isLastWave && i == enemyCount - 1)
            {
                spawner.SpawnEnemy(true);
            }
            else
            {
                spawner.SpawnEnemy();
            }
        }
        if (CurrentWave < maxWaves)
        {
            StartCoroutine(WaitForNextWave(timeBetweenWaves));
            CurrentWave++;
        }
        else
        {
            lastWaveSpawned = true;
        }
    }

    #endregion

    #region Tower Logic

    /// <summary>
    /// Places a tower at the currently looking at position. Called by ui button
    /// </summary>
    /// <param name="towerToSpawn">Tower Object to spawn</param>
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
                    ApplyPermanentUpgrades(towerToSpawn);
                    towerBase.SetTower(towerToSpawn);
                    Player.PlaceTower.SetTower(tower);
                    Coins -= towerToSpawn.Cost;
                    CloseMenu(Menus.ChooseTower);
                }
            }
        }
    }

    /// <summary>
    /// Delete tower and give the refund. Called by ui button
    /// </summary>
    public void DeleteTower()
    {
        TowerPlace towerPlace = Player.PlaceTower;
        Coins += Mathf.RoundToInt(towerPlace.Tower.GetComponent<TowerBase>().TotalCoinsSpent * 0.75f);
        Destroy(towerPlace.Tower);
        towerPlace.SetTower(null);
        CloseMenu(Menus.TowerUpgrades);
    }

    /// <summary>
    /// Increase a tower stat. Called by ui Button
    /// </summary>
    /// <param name="towerStatIndex">tower stat enum index to increase</param>
    public void UpgradeTower(int towerStatIndex)
    {
        TowerStat stat = (TowerStat)towerStatIndex;
        TowerBase towerBase = Player.PlaceTower.Tower.GetComponent<TowerBase>();
        int newCoinValue = towerBase.UpgradeStat(stat, Coins);
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

    /// <summary>
    /// applies the permanent upgrade stats to a tower object
    /// </summary>
    /// <param name="towerObject">towerObejct</param>
    private void ApplyPermanentUpgrades(Tower towerObject)
    {
        if (saveFile != null)
        {
            PermanentUpgrade upgrade = saveFile.PermanentUpgrades[(int)towerObject.TowerType];
            towerObject.ApplyPermanentUpgrade(upgrade);
        }
    }

    #endregion

    #region UI Menu Logic

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
        Cursor.lockState = CursorLockMode.None;

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
        if (!defaultCursorVisibility) Cursor.lockState = CursorLockMode.Locked;

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

    /// <summary>
    /// Set the UI Wave Counter
    /// </summary>
    public void UpdateWaveCounter()
    {
        uiManager.UpdateWaveCounter(CurrentWave, maxWaves);
    }

    #endregion
}
