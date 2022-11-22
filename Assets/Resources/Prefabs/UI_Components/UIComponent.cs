using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public abstract class UIComponent : MonoBehaviour, IUIComponent
{
    [SerializeField] protected string id;
    public string Id
    {
        get { return id; }
    }

    protected Dictionary<string, Action<string>> actions = new Dictionary<string, Action<string>>();

    protected virtual void Start()
    {
        actions.Add("Visible", SetVisibility);
    }

    public virtual void Refresh(Dictionary<string, string> request)
    {
        foreach (string key in request.Keys)
        {
            if (actions.Keys.Contains(key))
            {
                actions[key].Invoke(request[key]);
            }
        }
    }

    private void SetVisibility(string value)
    {
        gameObject.SetActive(value == "true" ? true : false);
    }

}
