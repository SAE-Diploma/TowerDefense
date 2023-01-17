using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerManagerViewModel : ViewModelBase
{
    private readonly TowerStats towerStats;
    private IMGUIContainer iconImage;
    private List<TowerStats> allStats;
    private Label levelIndexLabel;
    private VisualElement levelStatsContainer;

    private int levelIndex;
    public int LevelIndex
    {
        get { return levelIndex; }
        set
        {
            levelIndex = value;
            levelIndexLabel.text = levelIndex.ToString();
            if (towerStats.LevelList.Count > 0 )
            {
                ShowLevel(towerStats.LevelList[levelIndex - 1]);
            }

        }
    }

    public TowerManagerViewModel(TowerManager manager, VisualElement root, TowerStats towerStats) : base(manager, root)
    {
        this.towerStats = towerStats;
        this.viewName = "TowerManager";
    }

    public override void AfterShow()
    {
        iconImage = root.Q<IMGUIContainer>("IconImage");
        levelIndexLabel = root.Q<Label>("SelectedLevel");
        levelStatsContainer = root.Q<VisualElement>("LevelFields");

        // Get guids of all TowerStat scripts
        allStats = new List<TowerStats>(); 
        string[] allStatsGUIDs = AssetDatabase.FindAssets("t:TowerStats");
        foreach (string guid in allStatsGUIDs)
        {
            allStats.Add(AssetDatabase.LoadAssetAtPath<TowerStats>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        BindFields(towerStats, allStatsGUIDs);
        BindButtons();
        LevelIndex = 1;
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (iconImage != null)
        {
            iconImage.style.backgroundImage = new StyleBackground(towerStats.Icon);
        }
    }


    private void BindFields(TowerStats stats, string[] allStatsGUIDs)
    {
        SerializedObject so = new SerializedObject(stats);

        Label label = root.Q<Label>("TowerName");
        label.text = towerStats.TowerName;

        ObjectField iconField = root.Q<ObjectField>("Icon");
        iconField.BindProperty(so.FindProperty("icon"));
        iconField.value = towerStats.Icon;

        Toggle isUnlockedToggle = root.Q<Toggle>("isUnlocked");
        isUnlockedToggle.BindProperty(so.FindProperty("isUnlocked"));
        isUnlockedToggle.value = towerStats.IsUnlocked;

        IntegerField unlockCost = root.Q<IntegerField>("UnlockCost");
        unlockCost.BindProperty(so.FindProperty("unlockCost"));
        unlockCost.value = towerStats.UnlockCost;

        IntegerField buildCost = root.Q<IntegerField>("BuildCost");
        buildCost.BindProperty(so.FindProperty("buildCost"));
        buildCost.value = towerStats.BuildCost;

        ObjectField prefab = root.Q<ObjectField>("Prefab");
        prefab.BindProperty(so.FindProperty("towerPrefab"));
        prefab.value = towerStats.TowerPrefab;

        ObjectField blueprintPrefab = root.Q<ObjectField>("BlueprintPrefab");
        blueprintPrefab.BindProperty(so.FindProperty("towerBlueprintPrefab"));
        blueprintPrefab.value = towerStats.TowerBlueprintPrefab;

        DropdownField typeDropDown = root.Q<DropdownField>("TowerType");
        //typeDropDown.BindProperty(so.FindProperty("levelType"));
        typeDropDown.choices = GetAllTypeOptions(allStatsGUIDs);
        typeDropDown.index = 0;


        LevelIndex = 1;

    }

    private void ShowLevel(TowerLevel level)
    {
        SerializedObject so = new SerializedObject(level);

        // get the fields
        FieldInfo[] baseFields = level.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo[] fields = level.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        // combine ranges
        List<FieldInfo> allFields = new List<FieldInfo>(baseFields);
        allFields.AddRange(fields);

        // add Floatfield and bind it to the SerializedObject
        levelStatsContainer.Clear();
        for (int i = 0; i < allFields.Count; i++)
        {
            FloatField newField = new FloatField();
            newField.BindProperty(so.FindProperty(allFields[i].Name));
            newField.name = $"LevelField_{i}";
            newField.label = allFields[i].Name;
            newField.value = (float)allFields[i].GetValue(level);
            levelStatsContainer.Add(newField);
        }
    }

    private void BindButtons()
    {
        // Back Buttons
        root.Q<Button>("BackButton").clicked += OnBackButton;

        // NextLevel
        root.Q<Button>("NextLevel").clicked += () =>
        {
            if (levelIndex < towerStats.LevelList.Count) LevelIndex++;
        };

        // Previous Level
        root.Q<Button>("PreviousLevel").clicked += () =>
        {
            if (levelIndex > 1) LevelIndex--;
        };
    }

    private void OnBackButton()
    {
        manager.CurrentViewModel = new TowerListViewModel(manager, root);
    }

    private List<string> GetAllTypeOptions(string[] guids)
    {
        List<string> types = new List<string>();
        foreach (string guid in guids)
        {
            types.Add(Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)));
        }
        return types;
    }

}