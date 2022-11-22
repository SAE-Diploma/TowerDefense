using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrades : UIComponent
{
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


    protected override void Start()
    {
        base.Start();
        actions.Add("TowerName", (string value) => towerName.text = value);
        actions.Add("SpeedValue", (string value) => attackSpeedValue.text = value);
        actions.Add("SpeedCost", (string value) => attackSpeedValue.text = value);
    }


}
