using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
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
        buildMode.Clear();
        for (int i = 0; i < allTowerStats.Length; i++)
        {
            TemplateContainer towerRoot = TowerComponent.CloneTree();
            towerRoot.Q<Label>("Name").text = allTowerStats[i].TowerName;
            towerRoot.Q<IMGUIContainer>("Image").style.backgroundImage = new StyleBackground(allTowerStats[i].Icon);
            towerRoot.AddToClassList("BuildMode_Tower");
            towerRoot.AddToClassList($"BuildMode_Tower_{i}");
            buildMode.Add(towerRoot);
        }
    }

    public void SelectTower(int newActive, int oldActive)
    {
        VisualElement newSelected = buildMode.Q<VisualElement>("", $"BuildMode_Tower_{newActive}");
        if (newSelected != null) newSelected.AddToClassList("BuildMode_Tower_Selected");

        VisualElement oldSelected = buildMode.Q<VisualElement>("", $"BuildMode_Tower_{oldActive}");
        if (oldSelected != null && oldActive != newActive) oldSelected.RemoveFromClassList("BuildMode_Tower_Selected");
    }


}
