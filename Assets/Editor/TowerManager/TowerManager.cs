using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class TowerManager : EditorWindow
{
    [MenuItem("Window/UI Toolkit/TowerManager")]
    public static void ShowExample()
    {
        TowerManager wnd = GetWindow<TowerManager>();
        wnd.titleContent = new GUIContent("TowerManager");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/TowerManager/TowerManager.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
    }
}