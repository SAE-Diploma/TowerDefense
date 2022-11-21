using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour 
{
    private static UI_Manager instance;

    private Dictionary<string, UIComponent> UIComponents;

    private UI_Manager()
    {
        // get ui components
        UIComponents = new Dictionary<string, UIComponent>();
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(UIComponent));
        foreach (UIComponent component in objects)
        {
            string id = component.Id;
            if (!string.IsNullOrEmpty(id))
            {
                UIComponents.Add(id, component);
            }
        }
    }
    /// <summary>
    /// Get singleton instance
    /// </summary>
    /// <returns></returns>
    public static UI_Manager Get()
    {
        if (instance == null) instance = new UI_Manager();
        return instance;
    }

    public void RefreshUIComponent(string id, Dictionary<string,string> request)
    {
        if (UIComponents.ContainsKey(id))
        {
            UIComponents[id].Refresh(request);
        }
    }
}
