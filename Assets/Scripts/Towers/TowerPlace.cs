using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerPlace : MonoBehaviour
{
    [SerializeField] Transform place;
    public Transform Place => place;

    GameObject tower;
    public GameObject Tower => tower;

    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI towerName;
    [SerializeField] TextMeshProUGUI attackSpeedValue;
    [SerializeField] TextMeshProUGUI attackSpeedLevel;
    [SerializeField] TextMeshProUGUI damageValue;
    [SerializeField] TextMeshProUGUI damageLevel;
    [SerializeField] TextMeshProUGUI rangeValue;
    [SerializeField] TextMeshProUGUI rangeLevel;
    [SerializeField] TextMeshProUGUI projectileSpeedValue;
    [SerializeField] TextMeshProUGUI projectileSpeedLevel;

    /// <summary>
    /// Set the reference to the tower gameobject
    /// </summary>
    /// <param name="tower">tower gameobject</param>
    public void SetTower(GameObject tower)
    {
        this.tower = tower;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (tower == null)
        {
            canvas.gameObject.SetActive(false);
        }
        else
        {
            OLD_TowerBase towerBase = tower.GetComponent<OLD_TowerBase>();
            canvas.gameObject.SetActive(true);
            towerName.text = towerBase.Tower.TowerType.ToString();
            attackSpeedValue.text = $"{towerBase.AttackSpeed} rps";
            attackSpeedLevel.text = $"Lv. {towerBase.AttackSpeedLevel + 1}";
            damageValue.text = $"{towerBase.Damage} hp";
            damageLevel.text = $"Lv. {towerBase.DamageLevel + 1}";
            rangeValue.text = $"{towerBase.Range} m";
            rangeLevel.text = $"Lv. {towerBase.RangeLevel + 1}";
            projectileSpeedValue.text = $"{towerBase.ProjectileSpeed} m/s";
            projectileSpeedLevel.text = $"Lv. {towerBase.ProjectileSpeedLevel + 1}";
        }
    }
}
