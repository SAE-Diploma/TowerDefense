using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public enum MainMenus
{
    MenuButtons = 0,
    Upgrades = 1
}


public class MainMenu : MonoBehaviour
{
    SaveFile saveFile;
    [SerializeField] GameObject saveManagerPrefab;

    [SerializeField] List<GameObject> menus;

    [SerializeField] RuntimeAnimatorController errorAnimationController;

    [SerializeField] GameObject modalPrefab;

    [Header("Main Menu Buttons")]
    [SerializeField] Button continueButton;
    [SerializeField] Button upgradesButton;
    [SerializeField] Toggle tutorial;

    [Header("Upgrade Panel References")]
    [SerializeField] GameObject tabButtons;
    [SerializeField] TextMeshProUGUI pointsText;

    [SerializeField] Tower defaultTower;
    [SerializeField] TextMeshProUGUI towerTypeText;

    [Header("Start values")]
    [SerializeField] TextMeshProUGUI attackspeedValue;
    [SerializeField] TextMeshProUGUI attackspeedButtonText;
    [SerializeField] TextMeshProUGUI damageValue;
    [SerializeField] TextMeshProUGUI damageValueButtonText;
    [SerializeField] TextMeshProUGUI rangeValue;
    [SerializeField] TextMeshProUGUI rangeValueButtonText;
    [SerializeField] TextMeshProUGUI projectileSpeedValue;
    [SerializeField] TextMeshProUGUI projectileSpeedValueButtonText;

    [Header("Levels")]
    [SerializeField] TextMeshProUGUI attackspeedLevel;
    [SerializeField] TextMeshProUGUI attackspeedLevelButtonText;
    [SerializeField] TextMeshProUGUI damageLevel;
    [SerializeField] TextMeshProUGUI damageLevelButtonText;
    [SerializeField] TextMeshProUGUI rangeLevel;
    [SerializeField] TextMeshProUGUI rangeLevelButtonText;
    [SerializeField] TextMeshProUGUI projectileSpeedLevel;
    [SerializeField] TextMeshProUGUI projectileSpeedLevelButtonText;

    [Header("Upgrade sub menus")]
    [SerializeField] GameObject startValues;
    [SerializeField] GameObject levels;
    [SerializeField] GameObject locked;
    [SerializeField] TextMeshProUGUI lockedButtonText;
    private Tower currentTower;

    private TextMeshProUGUI[] buttonTextArray; // just makes it easier to refernce button text by enumindex

    private void Start()
    {
        GameObject saveManager = GameObject.Find("SaveManager(Clone)");
        if (saveManager != null) saveFile = saveManager.GetComponent<SaveFile>();
        else
        {
            saveManager = Instantiate(saveManagerPrefab);
            saveFile = saveManager.GetComponent<SaveFile>();
        }

        if (saveFile.Loaded)
        {
            continueButton.interactable = true;
            upgradesButton.interactable = true;
            tutorial.isOn = false;
        }
        else
        {
            tutorial.isOn = true;
        }

        buttonTextArray = new TextMeshProUGUI[]
        {
            attackspeedButtonText,
            damageValueButtonText,
            rangeValueButtonText,
            projectileSpeedValueButtonText,
            attackspeedLevelButtonText,
            damageLevelButtonText,
            rangeLevelButtonText,
            projectileSpeedLevelButtonText
        };
        ShowMenu((int)MainMenus.MenuButtons);
        UpdatePointsText();

    }

    /// <summary>
    /// Start a round
    /// </summary>
    public void StartGame(bool newGame)
    {
        if (newGame)
        {
            if (saveFile.Loaded) ShowNewGameModal();
            else
            {
                saveFile.InitializeDefaultValues();
                saveFile.Save();
                if (tutorial.isOn) SceneManager.LoadScene(1);
                else SceneManager.LoadScene(2);
            }
        }
        else
        {
            if (tutorial.isOn) SceneManager.LoadScene(1);
            else SceneManager.LoadScene(2);
        }
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// shows a menu and hides all others
    /// </summary>
    /// <param name="enumIndex"></param>
    public void ShowMenu(int enumIndex)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i != enumIndex) menus[i].SetActive(false);
            else menus[i].SetActive(true);
        }
        if (enumIndex == 1)
        {
            UpdateUpgradePanel(defaultTower);
        }

    }

    /// <summary>
    /// updates the points text
    /// </summary>
    private void UpdatePointsText()
    {
        pointsText.text = $"{saveFile.Points.ToString()} pts";
    }

    /// <summary>
    /// updates all references in the upgrades panel with the infos in the tower and savefile
    /// </summary>
    /// <param name="tower">tower reference</param>
    public void UpdateUpgradePanel(Tower tower)
    {
        PermanentUpgrade upgrades = saveFile.PermanentUpgrades[(int)tower.TowerType];
        currentTower = tower;

        SelectTabButton((int)tower.TowerType);

        towerTypeText.text = upgrades.TowerType.ToString();

        if (upgrades.Unlocked)
        {
            startValues.SetActive(true);
            levels.SetActive(true);
            locked.SetActive(false);
            attackspeedValue.text = $"{upgrades.AttackSpeedStartValue} rps";
            attackspeedButtonText.text = $"{upgrades.AttackSpeedValueCost} pts (+{upgrades.AttackSpeedIncrementValue})";
            damageValue.text = $"{upgrades.DamageStartValue} hp";
            damageValueButtonText.text = $"{upgrades.DamageValueCost} pts (+{upgrades.DamageIncrementValue})";
            rangeValue.text = $"{upgrades.RangeStartValue} m";
            rangeValueButtonText.text = $"{upgrades.RangeValueCost} pts (+{upgrades.RangeIncrementValue})";
            projectileSpeedValue.text = $"{upgrades.ProjectileSpeedStartValue} m/s";
            projectileSpeedValueButtonText.text = $"{upgrades.ProjectileSpeedValueCost} pts (+{upgrades.ProjectileSpeedIncrementValue})";

            attackspeedLevel.text = $"Level {upgrades.AttackSpeedMaxLevel}";
            attackspeedLevelButtonText.text = $"{upgrades.AttackSpeedLevelCost} pts (+1)";
            damageLevel.text = $"Level {upgrades.DamageMaxLevel}";
            damageLevelButtonText.text = $"{upgrades.DamageLevelCost} pts (+1)";
            rangeLevel.text = $"Level {upgrades.RangeMaxLevel}";
            rangeLevelButtonText.text = $"{upgrades.RangeLevelCost} pts (+1)";
            projectileSpeedLevel.text = $"Level {upgrades.ProjectileSpeedMaxLevel}";
            projectileSpeedLevelButtonText.text = $"{upgrades.ProjectileSpeedLevelCost} pts (+1)";
        }
        else
        {
            lockedButtonText.text = tower.UnlockCost.ToString() + " pts";
            ShowLocked(tower);
        }
    }

    /// <summary>
    /// shows the locked tower panel
    /// </summary>
    /// <param name="tower">tower object</param>
    private void ShowLocked(Tower tower)
    {
        startValues.SetActive(false);
        levels.SetActive(false);
        locked.SetActive(true);
        locked.transform.GetChild(0).GetComponent<Image>().sprite = tower.Icon;
    }

    /// <summary>
    /// sets the colors of the tab buttons right
    /// </summary>
    /// <param name="index">Towers Enum index</param>
    private void SelectTabButton(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            Button btn = tabButtons.transform.GetChild(i).GetComponent<Button>();
            ColorBlock colorBlock = btn.colors;
            if (i == index)
            {
                colorBlock.normalColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                colorBlock.selectedColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                colorBlock.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                colorBlock.pressedColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            }
            else
            {
                colorBlock.normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                colorBlock.selectedColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                colorBlock.highlightedColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);
                colorBlock.pressedColor = new Color(0.25f, 0.25f, 0.25f, 0.8f);
            }
            btn.colors = colorBlock;
        }

    }

    /// <summary>
    /// Unlocks the currently showing tower. If not enough points -> error animation
    /// </summary>
    public void UnlockTower()
    {
        if (currentTower != null)
        {
            if (saveFile.Points >= currentTower.UnlockCost)
            {
                saveFile.PermanentUpgrades[(int)currentTower.TowerType].Unlock();
                saveFile.SetPoints(saveFile.Points - 1000);
                UpdatePointsText();
                saveFile.Save();
                UpdateUpgradePanel(currentTower);
            }
            else
            {
                Animator animator = lockedButtonText.GetComponent<Animator>();
                if (animator == null) animator = lockedButtonText.gameObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = errorAnimationController;
                animator.SetTrigger("Play");
            }
        }
    }

    /// <summary>
    /// Button Method. Tries to purchase an upgrade. Shows error animation when not enough points
    /// </summary>
    /// <param name="enumIndex">Upgrade index</param>
    public void UpgradeStat(int enumIndex)
    {
        PermanentUpgrade upgrade = saveFile.PermanentUpgrades[(int)currentTower.TowerType];
        int cost = upgrade.GetCostArray()[enumIndex];
        if (saveFile.Points >= cost)
        {
            upgrade.IncrementStat((PermanentStats)enumIndex);
            saveFile.SetPoints(saveFile.Points - cost);
            UpdatePointsText();
            saveFile.Save();
            UpdateUpgradePanel(currentTower);
        }
        else
        {
            Animator animator = buttonTextArray[enumIndex].GetComponent<Animator>();
            if (animator == null) animator = buttonTextArray[enumIndex].gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = errorAnimationController;
            animator.SetTrigger("Play");
        }
    }

    /// <summary>
    /// Loads the Modal to warn about overwriting the existing save
    /// </summary>
    public void ShowNewGameModal()
    {
        GameObject modal = Instantiate(modalPrefab, transform);
        Modal modalClass = modal.GetComponent<Modal>();
        modalClass.SetValues("New save", "With starting a new game all your progress will be lost.\nDo you still want to continue?");
        modalClass.OnAgreed.AddListener(() =>
        {
            saveFile.InitializeDefaultValues();
            saveFile.Save();
            StartGame(false);
        });
        modalClass.OnDisagreed.AddListener(() => Destroy(modal));
    }
}
