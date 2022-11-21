using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIComponent : MonoBehaviour, IUIComponent
{
    [SerializeField]
    protected string id;
    public string Id
    {
        get { return id; }
    }

    public virtual void Refresh(Dictionary<string, string> request)
    {
        throw new System.NotImplementedException();
    }

}
