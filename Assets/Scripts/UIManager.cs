using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public enum Menus : int
{
    ChooseTower = 0,
    TowerUpgrades = 1,
    PauseMenu = 2,
    WinMenu = 3,
    LooseMenu = 4,
}


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactionPanel;
    [SerializeField] List<Image> menus;
    [SerializeField] GameObject modalPrefab;

    [Header("TowerUpgrade Menu")]
    [SerializeField] TextMeshProUGUI towerName;
    [SerializeField] TextMeshProUGUI attackSpeedValue;
    [SerializeField] Button attackSpeedButton;
    [SerializeField] TextMeshProUGUI attackSpeedCost;
    [SerializeField] TextMeshProUGUI damageValue;
    [SerializeField] Button damageButton;
    [SerializeField] TextMeshProUGUI damageCost;
    [SerializeField] TextMeshProUGUI rangeValue;
    [SerializeField] Button rangeButton;
    [SerializeField] TextMeshProUGUI rangeCost;
    [SerializeField] TextMeshProUGUI projectileSpeedValue;
    [SerializeField] Button projectileSpeedButton;
    [SerializeField] TextMeshProUGUI projectileSpeedCost;

    [Header("InGameUI")]
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI currentWaveText;
    [SerializeField] TextMeshProUGUI timer;

    [Header("WinLoose Menus")]
    [SerializeField] TextMeshProUGUI winStats;
    [SerializeField] TextMeshProUGUI looseStats;

    /// <summary>
    /// Sets the visibility of the given Menu
    /// </summary>
    /// <param name="menu">menu to set the visibility of</param>
    /// <param name="visibility">should it be visible</param>
    public void SetMenuVisibility(Menus menu, bool visibility)
    {
        GameObject obj = menus[(int)menu].gameObject;
        obj.SetActive(visibility);
    }

    /// <summary>
    /// Sets the visiblity of the Interaction E
    /// </summary>
    /// <param name="visibility">the visibility</param>
    public void SetInteractionVisibility(bool visibility)
    {
        interactionPanel.SetActive(visibility);
    }

    /// <summary>
    /// Sets all the text and icons in the Tower Upgrade Menu
    /// </summary>
    /// <param name="tower">referenced tower</param>
    public void UpdateTowerUpgradePanel(TowerBase tower)
    {
        Debug.Log(tower.gameObject.name);
        towerName.text = tower.Tower.name;
        attackSpeedValue.text = $"Level {tower.AttackSpeedLevel}: {tower.AttackSpeed} rps";
        if (tower.AttackSpeedLevel == tower.AttackSpeedMaxLevel) SetButtonInteractible(attackSpeedButton, false);
        else SetButtonInteractible(attackSpeedButton, true);
        attackSpeedCost.text = ((tower.AttackSpeedLevel + 1) * tower.Tower.AttackspeedUpgradeCost).ToString();
        damageValue.text = $"Level {tower.DamageLevel}: {tower.Damage} hp";
        if (tower.DamageLevel == tower.DamageMaxLevel) SetButtonInteractible(damageButton, false);
        else SetButtonInteractible(damageButton, true);
        damageCost.text = ((tower.DamageLevel + 1) * tower.Tower.DamageUpgradeCost).ToString();
        rangeValue.text = $"Level {tower.RangeLevel}: {tower.Range} m";
        if (tower.RangeLevel == tower.RangeMaxLevel) SetButtonInteractible(rangeButton, false);
        else SetButtonInteractible(rangeButton, true);
        rangeCost.text = ((tower.RangeLevel + 1) * tower.Tower.RangeUpgradeCost).ToString();
        projectileSpeedValue.text = $"Level {tower.ProjectileSpeedLevel}: {tower.ProjectileSpeed} m/s";
        if (tower.ProjectileSpeedLevel == tower.ProjectileSpeedMaxLevel) SetButtonInteractible(projectileSpeedButton, false);
        else SetButtonInteractible(projectileSpeedButton, true);
        projectileSpeedCost.text = ((tower.ProjectileSpeedLevel + 1) * tower.Tower.ProjectileSpeedUpgradeCost).ToString();
    }

    /// <summary>
    /// disable an upgrade button and show the max text
    /// </summary>
    /// <param name="upgradeButton">upgrade button to disable</param>
    private void SetButtonInteractible(Button upgradeButton, bool interactable)
    {
        upgradeButton.interactable = interactable;
        upgradeButton.transform.GetChild(0).gameObject.SetActive(interactable);
        upgradeButton.transform.GetChild(1).gameObject.SetActive(!interactable);
    }

    /// <summary>
    /// Play error animation for a stat when not enough coins to upgrade a tower
    /// </summary>
    /// <param name="stat">stat that failed</param>
    public void UpdateTowerMenuCoinError(TowerStat stat)
    {
        Animator animator;
        switch (stat)
        {
            case TowerStat.Attackspeed:
                animator = attackSpeedCost.GetComponent<Animator>();
                animator.SetTrigger("NotEnoughCoins");
                break;
            case TowerStat.Damage:
                animator = damageCost.GetComponent<Animator>();
                animator.SetTrigger("NotEnoughCoins");
                break;
            case TowerStat.Range:
                animator = rangeCost.GetComponent<Animator>();
                animator.SetTrigger("NotEnoughCoins");
                break;
            case TowerStat.ProjectileSpeed:
                animator = projectileSpeedCost.GetComponent<Animator>();
                animator.SetTrigger("NotEnoughCoins");
                break;
        }
    }

    /// <summary>
    /// set the wave counter text
    /// </summary>
    /// <param name="currentWave">the current wave</param>
    /// <param name="maxWaveCount">the maximum wave count</param>
    public void UpdateWaveCounter(int currentWave, int maxWaveCount)
    {
        currentWaveText.text = $"Wave {currentWave}/{maxWaveCount}";
    }

    /// <summary>
    /// Show the remaining Time on the tresor
    /// </summary>
    /// <param name="remainingSeconds">remaining time in seconds</param>
    public void ShowRemainingTime(int remainingSeconds)
    {
        timer.text = SecondsToTime(remainingSeconds);
    }

    /// <summary>
    /// Formats seconds into a time string
    /// </summary>
    /// <param name="timeInSeconds">seconds</param>
    /// <returns>formatted string</returns>
    private string SecondsToTime(int timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = timeInSeconds - minutes * 60;
        string secondsString = seconds < 10 ? $"0{seconds}" : seconds.ToString();
        return $"{minutes}:{secondsString}";
    }

    /// <summary>
    /// Set the coins text
    /// </summary>
    /// <param name="coins">amount of coins</param>
    public void SetCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }

    /// <summary>
    /// Shows the modal before going back to the main menu without saving
    /// </summary>
    /// <param name="action">agreed function</param>
    public void ShowBackToMenuModal(UnityAction action)
    {
        GameObject modalObject = Instantiate(modalPrefab, transform);
        Modal modalClass = modalObject.GetComponent<Modal>();
        modalClass.SetValues("Quit the round", "The progress from this round will not be saved!\nNo points will be gained.");
        modalClass.SetButtonTexts("Back", "MainMenu");
        modalClass.OnAgreed.AddListener(action);
        modalClass.OnDisagreed.AddListener(() => Destroy(modalObject));
    }

    public void SetStatsText(int waves, int enemies, int points)
    {
        string stats = $"{waves}\n{enemies}\n{points}";
        winStats.text = stats;
        looseStats.text = stats;
    }
}
