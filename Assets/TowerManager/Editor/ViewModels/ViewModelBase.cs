using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ViewModelBase
{
    protected readonly TowerManager manager;
    protected readonly VisualElement root;
    protected string viewName = ""; // must be set in constructor

    public ViewModelBase(TowerManager manager,VisualElement root)
    {
        this.manager = manager;
        this.root = root;
    }

    public virtual void Build() 
    {
        VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{TowerManager.ViewsPath}/{viewName}.uxml");
        if (asset != null)
        {
            root.Clear();
            root.Add(asset.Instantiate());
            Show();
        }
    }
    public virtual void Show() { }
    public virtual void OnGUI() { }
}
