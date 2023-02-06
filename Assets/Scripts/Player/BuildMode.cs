using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BuildMode : MonoBehaviour
{
    private PlayerController controller;
    [SerializeField] GameManager manager;
    [SerializeField] VisualTreeAsset view;
    [SerializeField] VisualTreeAsset TowerComponent;
    private VisualElement viewRoot;
    private int viewIndex;

    [SerializeField] MeshFilter target;


    [SerializeField] List<TowerStats> placable;

    private InputMaster inputMaster;

    private bool inBuildMode = false;
    private int selectedTowerIndex = 0;

    private VisualElement buildMode;

    private void Awake()
    {
        inputMaster = InputManager.GetMaster();
        inputMaster.BuildMode.Enable();
        inputMaster.BuildMode.Enter.performed += OnEnterBuildMode;
        inputMaster.BuildMode.TowerSelection.performed += OnTowerSelection;
        inputMaster.BuildMode.Place.performed += OnLeftClick;
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        viewIndex = UI_Toolkit_Manager.Instance.AddToRoot(view.CloneTree());
        viewRoot = UI_Toolkit_Manager.Instance.GetViewByIndex(viewIndex);
        buildMode = viewRoot.Q<VisualElement>("BuildMode");
        RefreshTowers(placable);
    }

    private void OnEnterBuildMode(InputAction.CallbackContext obj)
    {
        inBuildMode = !inBuildMode;
        buildMode.ToggleInClassList("BuildMode_show");
        if (inBuildMode )
        {
            target.mesh = placable[selectedTowerIndex].BlueprintMesh;
        }
        else
        {
            target.mesh = null;
        }
    }

    private void OnLeftClick(InputAction.CallbackContext obj)
    {
        if (inBuildMode)
        {
            Instantiate(placable[selectedTowerIndex].TowerPrefab, target.transform.position, target.transform.rotation);
        }
    }

    private void OnTowerSelection(InputAction.CallbackContext obj)
    {
        if (inBuildMode)
        {
            int oldIndex = selectedTowerIndex;
            selectedTowerIndex = Mathf.RoundToInt(obj.ReadValue<float>()) - 1;
            if (selectedTowerIndex < placable.Count)
            {
                SelectTower(selectedTowerIndex, oldIndex);
            }
        }
    }

    public void RefreshTowers(List<TowerStats> allTowerStats)
    {
        if (TowerComponent == null) return;
        buildMode.Clear();
        for (int i = 0; i < allTowerStats.Count; i++)
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

        target.mesh = placable[newActive].BlueprintMesh;
    }

}
