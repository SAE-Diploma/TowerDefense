using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class TowerListViewModel : ViewModelBase
{
    private List<TowerStats> allTowerStats;
    private VisualElement listContainer;

    public TowerListViewModel(TowerManager manager, VisualElement root) : base(manager, root)
    {
        this.viewName = "TowerList";
        allTowerStats = new List<TowerStats>();
    }

    public override void AfterShow()
    {
        listContainer = root.Q<VisualElement>("List");
        string[] guids = AssetDatabase.FindAssets("t:TowerStats");

        foreach (string guid in guids)
        {
            TowerStats stats = AssetDatabase.LoadAssetAtPath<TowerStats>(AssetDatabase.GUIDToAssetPath(guid));
            if (stats != null) allTowerStats.Add(stats);
        }

        ShowList(listContainer, allTowerStats);


    }

    public override void OnGUI()
    {

    }

    private void ShowList(VisualElement listContainer, List<TowerStats> stats)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            // create component 
            VisualTreeAsset listItemAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{TowerManager.ComponentsPath}/TowerListItem.uxml");
            TemplateContainer listItem = listItemAsset.Instantiate();

            listItem.Q<Button>("ClickHandler").clicked += () =>
            {
                int index = listItem.Q<IntegerField>("Index").value;
                if (index > 0 && index < allTowerStats.Count - 1)
                {
                    manager.CurrentViewModel = new TowerManagerViewModel(manager, root, allTowerStats[index]);
                }

            };
            // fill component fields
            // index
            listItem.Q<IntegerField>("Index").value = i;

            // name
            listItem.Q<Label>("Name").text = stats[i].TowerName;

            // type
            listItem.Q<Label>("Type").text = "Type";

            // icon
            listItem.Q<IMGUIContainer>("Icon").style.backgroundImage = new StyleBackground(stats[i].Icon);

            listContainer.Add(listItem);
        }
    }

}