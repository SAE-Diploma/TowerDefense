using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

public class TowerManager : EditorWindow
{
    public static readonly string ViewsPath = "Assets/TowerManager/Editor/Views";
    public static readonly string ComponentsPath = "Assets/TowerManager/Editor/Components";
    public static readonly string TowersPath = "Assets/TowerManager/Towers";


    private ViewModelBase currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get { return currentViewModel; }
        set
        {
            currentViewModel = value;
            currentViewModel.Show();
        }
    }


    [MenuItem("TowerDefense/TowerManager")]
    public static void ShowExample() 
    {
        TowerManager wnd = GetWindow<TowerManager>();
        wnd.titleContent = new GUIContent("TowerManager");
    }

    public void CreateGUI()
    {

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        CurrentViewModel = new TowerListViewModel(this, root);
    }

    private void OnGUI()
    {
        if (CurrentViewModel != null) CurrentViewModel.OnGUI();
    }

}