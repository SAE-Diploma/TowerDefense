using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TowerButton : MonoBehaviour
{

    //[SerializeField] private OLD_Tower tower;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image towerIcon;
    [SerializeField] TextMeshProUGUI towerCost;

    private void OnValidate()
    {
        /*
        if (nameText != null && towerIcon != null && towerCost != null)
        {
            if (tower != null)
            {
                nameText.text = tower.TowerType.ToString();
                towerIcon.sprite = tower.Icon;
                towerCost.text = tower.Cost.ToString();
            }
            else
            {
                nameText.text = "";
                towerIcon.sprite = null;
                towerCost.text = "";
            }
        }
        */
    }

}
