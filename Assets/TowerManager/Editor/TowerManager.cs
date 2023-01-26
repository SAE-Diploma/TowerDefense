using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class TowerManager : EditorWindow
{
    public static readonly string ViewsPath = "Assets/TowerManager/Editor/Views";
    public static readonly string ComponentsPath = "Assets/TowerManager/Editor/Components";
    public static readonly string TowersPath = "Assets/TowerManager/Towers";
    public static readonly string LevelsPath = "Assets/TowerManager/Levels";

    private static List<Char> invalidFileNameChars;
    public static List<Char> InvalidFileNameChars { get { return invalidFileNameChars; } set { invalidFileNameChars = value; } }

    private ViewModelBase currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get { return currentViewModel; }
        set
        {
            currentViewModel = value;
            currentViewModel.Build();
        }
    }

    [MenuItem("Tools/TowerManager")]
    public static void ShowWindow()
    {
        TowerManager wnd = GetWindow<TowerManager>();
        wnd.titleContent = new GUIContent("TowerManager");
    }

    public void CreateGUI()
    {
        InvalidFileNameChars = Path.GetInvalidFileNameChars().ToList().GetRange(33, 8);
        VisualElement root = rootVisualElement;

        CurrentViewModel = new TowerListViewModel(this, root);
    }

    private void OnGUI()
    {
        if (CurrentViewModel != null) CurrentViewModel.OnGUI();
    }

}