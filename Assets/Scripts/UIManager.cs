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
    [SerializeField] TextMeshProUGUI coinsText;

    [Header("TowerUpgradePanel")]
    [SerializeField] TextMeshProUGUI towerName;
    [SerializeField] TextMeshProUGUI attackSpeedValue;
    [SerializeField] TextMeshProUGUI attackSpeedCost;
    [SerializeField] TextMeshProUGUI damageValue;
    [SerializeField] TextMeshProUGUI damageCost;
    [SerializeField] TextMeshProUGUI rangeValue;
    [SerializeField] TextMeshProUGUI rangeCost;
    [SerializeField] TextMeshProUGUI projectileSpeedValue;
    [SerializeField] TextMeshProUGUI projectileSpeedCost;

    [Header("Waves")]
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
        attackSpeedValue.text = $"{tower.AttackSpeed} rps";
        attackSpeedCost.text = ((tower.AttackSpeedLevel) * tower.Tower.AttackspeedUpgradeCost).ToString();
        damageValue.text = $"{tower.Damage} hp";
        damageCost.text = ((tower.DamageLevel) * tower.Tower.DamageUpgradeCost).ToString();
        rangeValue.text = $"{tower.Range} meters";
        rangeCost.text = ((tower.RangeLevel) * tower.Tower.RangeUpgradeCost).ToString();
        projectileSpeedValue.text = $"{tower.ProjectileSpeed} m/s";
        projectileSpeedCost.text = ((tower.ProjectileSpeedLevel) * tower.Tower.ProjectileSpeedUpgradeCost).ToString();
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
