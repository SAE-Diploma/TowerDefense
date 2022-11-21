using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIComponent
{
    public void Refresh(Dictionary<string, string> request);

    public string Id { get; }
}
