using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Toolkit_Manager : MonoBehaviour
{
    private VisualElement root;

    private static UI_Toolkit_Manager instance;
    public static UI_Toolkit_Manager Instance => instance;

    private List<VisualElement> elements;
    public List<VisualElement> Elements => elements;

    private void Awake()
    {
        instance = this;
        root = GetComponent<UIDocument>().rootVisualElement;
        elements = new List<VisualElement>();
    }

    public int AddToRoot(VisualElement child)
    {
        child.AddToClassList("absolut");
        root.Add(child);
        elements.Add(child);
        return elements.Count - 1;
    }

    public VisualElement GetViewByIndex(int index) { return elements[index]; }




}
