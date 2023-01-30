using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Toolkit_Manager : MonoBehaviour
{
    private VisualElement root;

    private VisualElement buildMode;

    private static UI_Toolkit_Manager instance;
    public static UI_Toolkit_Manager Instance => instance;

    [SerializeField] VisualTreeAsset TowerComponent;

    private void OnEnable()
    {
        instance = this;
        root = GetComponent<UIDocument>().rootVisualElement;
        buildMode = root.Q<VisualElement>("BuildMode");

    }

    public void ToggleBuildMode()
    {
        buildMode.ToggleInClassList("BuildMode_show");
    }

    public void RefreshTowers(TowerStats[] allTowerStats)
    {
        if (TowerComponent == null) return;
        foreach (TowerStats ts in allTowerStats)
        {
            TemplateContainer towerRoot = TowerComponent.CloneTree();
            towerRoot.Q<Label>("Name").text = ts.TowerName;
            towerRoot.Q<IMGUIContainer>("Image").style.backgroundImage = new StyleBackground(ts.Icon);
            buildMode.Add(towerRoot);
        }
    }

}
