using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [Header("TowerUpgradePanel")]
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
        towerName.text = tower.Tower.name;
        attackSpeedValue.text = $"Level {tower.AttackSpeedLevel}: {tower.AttackSpeed} rps";
        if (tower.AttackSpeedLevel == tower.AttackSpeedMaxLevel && attackSpeedButton.interactable) DisableUpgradeButton(attackSpeedButton);
        attackSpeedCost.text = ((tower.AttackSpeedLevel) * tower.Tower.AttackspeedUpgradeCost).ToString();
        damageValue.text = $"Level {tower.DamageLevel}: {tower.Damage} hp";
        if (tower.DamageLevel == tower.DamageMaxLevel && damageButton.interactable) DisableUpgradeButton(damageButton);
        damageCost.text = ((tower.DamageLevel) * tower.Tower.DamageUpgradeCost).ToString();
        rangeValue.text = $"Level {tower.RangeLevel}: {tower.Range} m";
        if (tower.RangeLevel == tower.RangeMaxLevel && rangeButton.interactable) DisableUpgradeButton(rangeButton);
        rangeCost.text = ((tower.RangeLevel) * tower.Tower.RangeUpgradeCost).ToString();
        projectileSpeedValue.text = $"Level {tower.ProjectileSpeedLevel}: {tower.ProjectileSpeed} m/s";
        if (tower.ProjectileSpeedLevel == tower.ProjectileSpeedMaxLevel && projectileSpeedButton.interactable) DisableUpgradeButton(projectileSpeedButton);
        projectileSpeedCost.text = ((tower.ProjectileSpeedLevel) * tower.Tower.ProjectileSpeedUpgradeCost).ToString();
    }

    /// <summary>
    /// disable an upgrade button and show the max text
    /// </summary>
    /// <param name="upgradeButton">upgrade button to disable</param>
    private void DisableUpgradeButton(Button upgradeButton)
    {
        upgradeButton.interactable = false;
        upgradeButton.transform.GetChild(1).gameObject.SetActive(true);
        upgradeButton.transform.GetChild(0).gameObject.SetActive(false);
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
}
