using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class TowerListViewModel : ViewModelBase
{
    private List<TowerStats> allTowerStats;
    private List<Type> towerLevels;
    private VisualElement listContainer;
    private ListView listView;

    public TowerListViewModel(TowerManager manager, VisualElement root) : base(manager, root)
    {
        this.viewName = "TowerList";
        allTowerStats = new List<TowerStats>();
    }

    public override void AfterShow()
    {
        listContainer = root.Q<VisualElement>("List");

        root.Q<Button>("RefreshBtn").clicked += OnRefresh;
        root.Q<Button>("AddBtn").clicked += OnAdd;
        root.Q<Button>("RemoveBtn").clicked += OnRemove;
        root.Q<Button>("CancelBtn").clicked += OnCancel;
        root.Q<Button>("NewBtn").clicked += OnCreate;

        allTowerStats = GetAllTowerStats();

        // prepare list view
        listView = root.Q<ListView>("ListView");
        listView.makeItem = MakeItem;
        listView.bindItem = BindItem;
        listView.itemsSource = allTowerStats;
        listView.fixedItemHeight = 60;

        towerLevels = GetAllLevelTypes();
        root.Q<DropdownField>("LevelType").choices = GetLevelNames(towerLevels);
    }

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
            if (index > 0 && index < allTowerStats.Count - 1)
            {
                manager.CurrentViewModel = new TowerManagerViewModel(manager, root, allTowerStats[index]);
            }
        };
    }

    public override void OnGUI()
    {

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
        string[] guids = AssetDatabase.FindAssets("",new string[] { "Assets/TowerManager/Towers/Levels" });
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
            Debug.Log(type.Name);
        }
        return names;
    }

    #region event handlers

    private void OnRemove()
    {
        throw new NotImplementedException();
    }

    private void OnAdd()
    {
        root.Q<VisualElement>("AddForm").RemoveFromClassList("NameFormHide");
    }

    private void OnRefresh()
    {
        allTowerStats = GetAllTowerStats();
        listView.itemsSource = allTowerStats;
        listView.Rebuild();
        Debug.Log("Rebuild");
    }

    private void OnCreate()
    {
        TextField nameField = root.Q<TextField>("NewName");
        if (string.IsNullOrEmpty(nameField.value))
        {
            nameField.AddToClassList("TextFieldError");
        }
        else
        {
            AssetDatabase.CreateFolder(TowerManager.TowersPath, nameField.value);
            AssetDatabase.CreateAsset(new TowerStats() { TowerName = nameField.value }, $"{TowerManager.TowersPath}/{nameField.value}/{nameField.value}Stats.asset");
        }
    }

    private void OnCancel()
    {
        root.Q<VisualElement>("AddForm").AddToClassList("NameFormHide");
        TextField nameField = root.Q<TextField>("NewName");
        nameField.value = "";
        nameField.RemoveFromClassList("TextFieldError");

    }

    #endregion

}