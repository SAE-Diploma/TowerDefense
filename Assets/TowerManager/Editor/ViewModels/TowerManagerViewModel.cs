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
    private VisualElement levelRightContainer;
    private VisualElement levelLeftContainer;
    private VisualElement fieldsContainer;

    private int levelIndex;
    public int LevelIndex
    {
        get { return levelIndex; }
        set
        {
            levelIndex = value;
            levelIndexLabel.text = levelIndex.ToString();
        }
    }

    private List<TimeValue> timeValue;
    private TimeValue standardTimeValue;
    private TimeValue instantTimeValue;

    public TowerManagerViewModel(TowerManager manager, VisualElement root, TowerStats towerStats) : base(manager, root)
    {
        this.towerStats = towerStats;
        this.viewName = "TowerManager";
        timeValue = new List<TimeValue>();
        instantTimeValue = new TimeValue() { value = 0, unit = TimeUnit.Millisecond };
        standardTimeValue = new TimeValue() { value = 300, unit = TimeUnit.Millisecond };
    }

    public override void AfterShow()
    {
        iconImage = root.Q<IMGUIContainer>("IconImage");
        levelIndexLabel = root.Q<Label>("SelectedLevel");
        levelStatsContainer = root.Q<VisualElement>("LevelFields");
        levelStatsContainer?.RegisterCallback<TransitionEndEvent>(ev => OnLevelStatsContainerTransitionEnd());

        levelRightContainer = root.Q<VisualElement>("LevelFields_Right");
        levelRightContainer?.RegisterCallback<TransitionEndEvent>(ev => OnLevelFadeContainerTransitionEnd());

        levelLeftContainer = root.Q<VisualElement>("LevelFields_Left");
        levelLeftContainer?.RegisterCallback<TransitionEndEvent>(ev => OnlevelLeftContainerTransitionEnd());

        fieldsContainer = root.Q<VisualElement>("Fields");

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
        ShowLevel(levelStatsContainer, towerStats.LevelList[levelIndex - 1]);
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

    private List<string> GetAllTypeOptions(string[] guids)
    {
        List<string> types = new List<string>();
        foreach (string guid in guids)
        {
            types.Add(Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)));
        }
        return types;
    }

    private void ShowLevel(VisualElement container, TowerLevel level)
    {
        SerializedObject so = new SerializedObject(level);

        // get the fields
        FieldInfo[] baseFields = level.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo[] fields = level.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        // combine ranges
        List<FieldInfo> allFields = new List<FieldInfo>(baseFields);
        allFields.AddRange(fields);

        // add Floatfield and bind it to the SerializedObject
        container.Clear();
        for (int i = 0; i < allFields.Count; i++)
        {
            FloatField newField = new FloatField();
            newField.BindProperty(so.FindProperty(allFields[i].Name));
            newField.name = $"LevelField_{i}";
            newField.label = allFields[i].Name;
            newField.value = (float)allFields[i].GetValue(level);
            newField.AddToClassList("Field");
            container.Add(newField);
        }
        fieldsContainer.style.minHeight = new StyleLength(allFields.Count * 28f + 20f);
    }

    private void BindButtons()
    {
        // Back Buttons
        root.Q<Button>("BackButton").clicked += OnBackButton;

        // NextLevel
        root.Q<Button>("NextLevel").clicked += () =>
        {
            if (levelIndex < towerStats.LevelList.Count)
            {
                LevelIndex++;
                NextLevel();

            }
        };

        // Previous Level
        root.Q<Button>("PreviousLevel").clicked += () =>
        {
            if (levelIndex > 1)
            {
                LevelIndex--;
                PreviousLevel();
            }
        };
    }

    private List<TimeValue> getTransitionDration(TransitionType type)
    {
        timeValue.Clear();
        if (type == TransitionType.instant) timeValue.Add(instantTimeValue);
        else timeValue.Add(standardTimeValue);
        return timeValue;
    }

    #region EventHandlers

    private void OnLevelStatsContainerTransitionEnd()
    {
        levelStatsContainer.style.transitionDuration = getTransitionDration(TransitionType.instant);
        levelStatsContainer.RemoveFromClassList("LevelList_FadeOut");
        levelStatsContainer.RemoveFromClassList("LevelList_FadeIn");
        ShowLevel(levelStatsContainer, towerStats.LevelList[levelIndex - 1]);
    }

    private void OnLevelFadeContainerTransitionEnd()
    {
        levelRightContainer.style.transitionDuration = getTransitionDration(TransitionType.instant);
        levelRightContainer.AddToClassList("LevelList_FadeIn");
    }

    private void OnlevelLeftContainerTransitionEnd()
    {
        levelLeftContainer.style.transitionDuration = getTransitionDration(TransitionType.instant);
        levelLeftContainer.AddToClassList("LevelList_FadeOut");
    }

    #endregion

    #region Button Functions

    private void NextLevel()
    {
        ShowLevel(levelRightContainer, towerStats.LevelList[levelIndex - 1]);

        levelStatsContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelStatsContainer.AddToClassList("LevelList_FadeOut");

        levelRightContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelRightContainer.RemoveFromClassList("LevelList_FadeIn");

    }

    private void PreviousLevel()
    {
        ShowLevel(levelLeftContainer, towerStats.LevelList[levelIndex - 1]);
        
        levelStatsContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelStatsContainer.AddToClassList("LevelList_FadeIn");

        levelLeftContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelLeftContainer.RemoveFromClassList("LevelList_FadeOut");
    }

    private void OnBackButton()
    {
        manager.CurrentViewModel = new TowerListViewModel(manager, root);
    }

    #endregion

}


public enum TransitionType
{
    standard,
    instant
}