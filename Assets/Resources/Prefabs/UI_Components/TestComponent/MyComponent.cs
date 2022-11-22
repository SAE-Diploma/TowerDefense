using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyComponent : UIComponent
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Image image;

    protected override void Start()
    {
        base.Start();
        actions.Add("Title", SetTitle);
    }

    public override void Refresh(Dictionary<string, string> request)
    {
        base.Refresh(request);
    }

    private void SetTitle(string title)
    {
        titleText.text = title;
    }
}
