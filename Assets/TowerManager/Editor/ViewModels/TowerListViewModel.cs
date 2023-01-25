﻿using Codice.CM.SEIDInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class TowerListViewModel : ViewModelBase
{
    private List<TowerStats> allTowerStats;
    private List<Type> levelTypes;
    private ListView listView;
    private VisualElement addForm;
    private DropdownField typesDropDown;
    private Button removeBtn;
    private bool isRemoving = false;

    public TowerListViewModel(TowerManager manager, VisualElement root) : base(manager, root)
    {
        this.viewName = "TowerList";
        allTowerStats = new List<TowerStats>();
    }

    public override void AfterShow()
    {
        AddNewTowerForm();

        root.Q<Button>("RefreshBtn").clicked += OnRefresh;
        root.Q<Button>("AddBtn").clicked += OnAdd;
        removeBtn = root.Q<Button>("RemoveBtn");
        removeBtn.clicked += OnRemove;

        // prepare list view
        allTowerStats = GetAllTowerStats();

        listView = root.Q<ListView>("ListView");
        listView.makeItem = MakeItem;
        listView.bindItem = BindItem;
        listView.itemsSource = allTowerStats;
        listView.fixedItemHeight = 60;
    }

    private List<TowerStats> GetAllTowerStats()
    {
        List<TowerStats> towerStats = new List<TowerStats>();
        string[] guids = AssetDatabase.FindAssets("t:TowerStats");
        foreach (string guid in guids)
        {
            TowerStats stats = AssetDatabase.LoadAssetAtPath<TowerStats>(AssetDatabase.GUIDToAssetPath(guid));
            if (stats != null) towerStats.Add(stats);
        }
        return towerStats;
    }

    private List<Type> GetAllLevelTypes()
    {
        // fill levelType Dropdown
        List<Type> levels = new List<Type>();
        string[] guids = AssetDatabase.FindAssets("", new string[] { "Assets/TowerManager/Towers/Levels" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string name = Path.GetFileNameWithoutExtension(path);
            Type type = Type.GetType($"{name}, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            if (type != null) levels.Add(type);
        }
        return levels;
    }

    private List<string> GetLevelNames(List<Type> types)
    {
        List<string> names = new List<string>();
        foreach (Type type in types)
        {
            names.Add(type.Name);
        }
        return names;
    }
    
    private void AddNewTowerForm()
    {
        // adding AddTowerForm component
        addForm = root.Q<VisualElement>("AddForm");
        VisualTreeAsset nameForm = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{TowerManager.ComponentsPath}/AddTowerForm.uxml");
        addForm.Add(nameForm.CloneTree());
        addForm.Q<Button>("CancelBtn").clicked += OnCancel;
        addForm.Q<Button>("NewBtn").clicked += OnCreate;
        addForm.Q<Button>("RefreshTypesBtn").clicked += OnRefreshTypes;

        // Setting type dropdown
        levelTypes = GetAllLevelTypes();
        SetLevelTypeOptions();
    }

    private void SetLevelTypeOptions()
    {
        if (typesDropDown == null) typesDropDown = addForm.Q<DropdownField>("LevelType");
        typesDropDown.choices = GetLevelNames(levelTypes);
    }


    #region ListView Methods

    private VisualElement MakeItem()
    {
        VisualElement listItem = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{TowerManager.ComponentsPath}/TowerListItem.uxml").CloneTree();
        return listItem;
    }

    private void BindItem(VisualElement element, int index)
    {
        // name
        Label name = element.Q<Label>("Name");
        name.text = allTowerStats[index].TowerName;

        // type
        if (allTowerStats[index].LevelType != null)
        {
            element.Q<Label>("Type").text = allTowerStats[index].LevelType.Name;
        }
        // icon
        IMGUIContainer icon = element.Q<IMGUIContainer>("Icon");
        element.Q<IMGUIContainer>("Icon").style.backgroundImage = new StyleBackground(allTowerStats[index].Icon);

        // index
        IntegerField indexField = element.Q<IntegerField>("Index");
        indexField.value = index;

        // handle click event
        element.Q<Button>("ClickHandler").clicked += () =>
        {
            int index = element.Q<IntegerField>("Index").value;
            if (isRemoving)
            {
                RemoveTower(index);
            }
            else
            {
                if (index >= 0 && index < allTowerStats.Count)
                {
                    manager.CurrentViewModel = new TowerManagerViewModel(manager, root, allTowerStats[index]);
                }
            }
        };

        if (isRemoving)
        {
            element.AddToClassList("RemovingEffect");
            removeBtn.text = "CANCEL";
        }
        else
        {
            element.RemoveFromClassList("RemovingEffect");
            removeBtn.text = "REMOVE";
        }


    }

    private void RebuildListView()
    {
        allTowerStats = GetAllTowerStats();
        listView.itemsSource = allTowerStats;
        listView.Rebuild();
    }

    private void RemoveTower(int index)
    {
        TowerStats stats = allTowerStats[index];
        string path = TowerManager.TowersPath + "/" + stats.TowerName;
        if (AssetDatabase.IsValidFolder(path) && !string.IsNullOrEmpty(stats.TowerName))
        {
            Debug.Log($"Removing {path}");
            AssetDatabase.DeleteAsset(path);
        }
        else
        {
            Debug.Log($"Cant remove that");
        }
        isRemoving = false;
        RebuildListView();
    }

    #endregion


    #region event handlers

    private void OnRemove()
    {
        isRemoving = !isRemoving;
        RebuildListView();
    }

    private void OnAdd()
    {
        addForm.ToggleInClassList("NameFormHide");
    }

    private void OnRefresh()
    {
        RebuildListView();
    }

    // NameForm Event Handlers
    private void OnCreate()
    {
        TextField nameField = addForm.Q<TextField>("NewName");
        DropdownField typeDropdown = addForm.Q<DropdownField>("LevelType");
        ObjectField icon = addForm.Q<ObjectField>("NewIcon");
        if (string.IsNullOrEmpty(nameField.value))
        {
            nameField.AddToClassList("TextFieldError");
        }
        else if (typeDropdown.index < 0 || typeDropdown.index > levelTypes.Count - 1)
        {
            nameField.RemoveFromClassList("TextFieldError");
            typeDropdown.AddToClassList("TextFieldError");
        }
        else
        {
            nameField.RemoveFromClassList("TextFieldError");
            typeDropdown.RemoveFromClassList("TextFieldError");

            AssetDatabase.CreateFolder(TowerManager.TowersPath, nameField.value);

            Type type = levelTypes[typeDropdown.index];
            string levelpath = $"{TowerManager.TowersPath}/{nameField.value}/{type.Name}_1.asset";
            AssetDatabase.CreateAsset((TowerLevel)Activator.CreateInstance(type), levelpath);

            TowerLevel level = AssetDatabase.LoadAssetAtPath<TowerLevel>(levelpath);

            AssetDatabase.CreateAsset(new TowerStats()
            {
                TowerName = nameField.value,
                LevelList = new List<TowerLevel>() { level },
                LevelType = type,
                Icon = (Sprite)icon.value
            }, $"{TowerManager.TowersPath}/{nameField.value}/{nameField.value}Stats.asset");

            addForm.AddToClassList("NameFormHide");
            RebuildListView();
        }
    }

    private void OnCancel()
    {
        addForm.AddToClassList("NameFormHide");
        TextField nameField = root.Q<TextField>("NewName");
        nameField.value = "";
        nameField.RemoveFromClassList("TextFieldError");
        DropdownField typeDropdown = root.Q<DropdownField>("LevelType");
        typeDropdown.index = -1;
    }

    private void OnRefreshTypes()
    {
        SetLevelTypeOptions();
    }

    #endregion

}