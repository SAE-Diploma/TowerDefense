using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class TowerDetailsViewModel : ViewModelBase
{
    private readonly TowerStats towerStats;
    private IMGUIContainer iconImage;
    private Label levelIndexLabel;
    private VisualElement levelStatsContainer;
    private VisualElement levelRightContainer;
    private VisualElement levelLeftContainer;
    private VisualElement fieldsContainer;

    private Button nextLevelBtn;
    private Button previousLevelBtn;
    private Button removeLevelBtn;

    private int _selectedLevel;
    public int SelectedLevel
    {
        get { return _selectedLevel; }
        set
        {
            _selectedLevel = value;
            levelIndexLabel.text = _selectedLevel.ToString();
            if (_selectedLevel == 1) previousLevelBtn.SetEnabled(false);
            else previousLevelBtn.SetEnabled(true);

            if (_selectedLevel == towerStats.LevelList.Count) nextLevelBtn.SetEnabled(false);
            else nextLevelBtn.SetEnabled(true);
        }
    }

    // timings for transitions
    private List<TimeValue> timeValue;
    private TimeValue standardTimeValue;
    private TimeValue instantTimeValue;

    public TowerDetailsViewModel(TowerManager manager, VisualElement root, TowerStats towerStats) : base(manager, root)
    {
        this.towerStats = towerStats;
        this.viewName = "TowerDetails";
        timeValue = new List<TimeValue>();
        instantTimeValue = new TimeValue() { value = 0, unit = TimeUnit.Millisecond };
        standardTimeValue = new TimeValue() { value = 300, unit = TimeUnit.Millisecond };
    }

    public override void Show()
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

        BindButtons();
        BindFields(towerStats);
        SelectedLevel = 1;
        ShowLevel(levelStatsContainer, towerStats.LevelList[SelectedLevel - 1]);
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (iconImage != null)
        {
            iconImage.style.backgroundImage = new StyleBackground(towerStats.Icon);
        }
    }


    private void BindFields(TowerStats stats)
    {
        SerializedObject so = new SerializedObject(stats);

        Label label = root.Q<Label>("TowerName");
        label.text = towerStats.TowerName;

        root.Q<Label>("TowerType").text = towerStats.LevelType == null ? "null" : towerStats.LevelType.Name;

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
        blueprintPrefab.BindProperty(so.FindProperty("blueprintMesh"));
        blueprintPrefab.value = towerStats.BlueprintMesh;

        SelectedLevel = 1;

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
            Type type = allFields[i].FieldType;
            if (type == typeof(int))
            {
                IntegerField newField = new IntegerField();
                newField.BindProperty(so.FindProperty(allFields[i].Name));
                newField.name = $"LevelField_{i}";
                newField.label = allFields[i].Name;
                newField.value = (int)allFields[i].GetValue(level);
                newField.AddToClassList("Field");
                container.Add(newField);
            }
            else if (type == typeof(float))
            {
                FloatField newField = new FloatField();
                newField.BindProperty(so.FindProperty(allFields[i].Name));
                newField.name = $"LevelField_{i}";
                newField.label = allFields[i].Name;
                newField.value = (float)allFields[i].GetValue(level);
                newField.AddToClassList("Field");
                container.Add(newField);
            }
        }
        fieldsContainer.style.minHeight = new StyleLength(allFields.Count * 28f + 20f);
    }

    private void BindButtons()
    {
        // Back Buttons
        root.Q<Button>("BackButton").clicked += OnBackButton;

        // NextLevel
        nextLevelBtn = root.Q<Button>("NextLevel");
        nextLevelBtn.clicked += () =>
        {
            if (SelectedLevel < towerStats.LevelList.Count)
            {
                SelectedLevel++;
                NextLevel();
                if (SelectedLevel == towerStats.LevelList.Count) nextLevelBtn.SetEnabled(false);
                else previousLevelBtn.SetEnabled(true);
            }
        };

        // Previous Level
        previousLevelBtn = root.Q<Button>("PreviousLevel");
        previousLevelBtn.SetEnabled(false);
        previousLevelBtn.clicked += () =>
        {
            if (SelectedLevel > 1)
            {
                SelectedLevel--;
                PreviousLevel();
                if (SelectedLevel == 1) previousLevelBtn.SetEnabled(false);
                else nextLevelBtn.SetEnabled(true);
            }
        };

        Button addLevelBtn = root.Q<Button>("AddAfterBtn");
        addLevelBtn.clicked += OnAddLevel;

        removeLevelBtn = root.Q<Button>("RemoveLevelBtn");
        removeLevelBtn.clicked += OnRemoveLevel;
        if (towerStats.LevelList.Count == 1) removeLevelBtn.SetEnabled(false);
    }



    private List<TimeValue> getTransitionDration(TransitionType type)
    {
        timeValue.Clear();
        if (type == TransitionType.instant) timeValue.Add(instantTimeValue);
        else timeValue.Add(standardTimeValue);
        return timeValue;
    }

    #region EventHandlers

    // Transitions
    private void OnLevelStatsContainerTransitionEnd()
    {
        levelStatsContainer.style.transitionDuration = getTransitionDration(TransitionType.instant);
        levelStatsContainer.RemoveFromClassList("LevelList_FadeOut");
        levelStatsContainer.RemoveFromClassList("LevelList_FadeIn");
        ShowLevel(levelStatsContainer, towerStats.LevelList[SelectedLevel - 1]);
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
        ShowLevel(levelRightContainer, towerStats.LevelList[SelectedLevel - 1]);

        levelStatsContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelStatsContainer.AddToClassList("LevelList_FadeOut");

        levelRightContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelRightContainer.RemoveFromClassList("LevelList_FadeIn");

    }

    private void PreviousLevel()
    {
        ShowLevel(levelLeftContainer, towerStats.LevelList[SelectedLevel - 1]);

        levelStatsContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelStatsContainer.AddToClassList("LevelList_FadeIn");

        levelLeftContainer.style.transitionDuration = getTransitionDration(TransitionType.standard);
        levelLeftContainer.RemoveFromClassList("LevelList_FadeOut");
    }

    private void OnAddLevel()
    {
        // rename higher levels
        if (SelectedLevel < towerStats.LevelList.Count)
        {
            int diff = towerStats.LevelList.Count - SelectedLevel;
            TowerLevel[] towerLevels = towerStats.LevelList.GetRange(SelectedLevel, diff).ToArray();
            int counter = 0;
            for (int i = towerLevels.Length - 1; i >= 0; i--)
            {
                // i = 1
                // levelnumber: 3 (count - counter)
                int currentIndex = towerStats.LevelList.Count - counter;
                string p = AssetDatabase.GetAssetPath(towerLevels[i]);
                AssetDatabase.RenameAsset(p, $"{towerStats.LevelType.Name}_{currentIndex + 1}.asset");
                counter++;
            }
        }

        // copy level 
        string path = AssetDatabase.GetAssetPath(towerStats.LevelList[SelectedLevel - 1]);
        string oldFileName = Path.GetFileName(path);
        string newPath = path.Replace(oldFileName, $"{towerStats.LevelType?.Name}_{SelectedLevel + 1}.asset");
        if (AssetDatabase.CopyAsset(path, newPath))
        {
            towerStats.LevelList.Insert(SelectedLevel, AssetDatabase.LoadAssetAtPath<TowerLevel>(newPath));
        }
        removeLevelBtn.SetEnabled(true);
        SelectedLevel++;
        NextLevel();
    }

    private void OnRemoveLevel()
    {
        TowerLevel selectedLevel = towerStats.LevelList[SelectedLevel - 1];
        string path = AssetDatabase.GetAssetPath(selectedLevel);
        towerStats.LevelList.Remove(selectedLevel);
        AssetDatabase.DeleteAsset(path);

        if (towerStats.LevelList.Count == 1)
        {
            removeLevelBtn.SetEnabled(false);
            nextLevelBtn.SetEnabled(false);
            previousLevelBtn.SetEnabled(false);
        }

        if (SelectedLevel > 1)
        {
            SelectedLevel--;
            PreviousLevel();
        }
        else
        {
            NextLevel();
        }
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