using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyComponent : UIComponent
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image image;

    private void Start()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            Debug.Log(Id);
        }
    }

    public override void Refresh(Dictionary<string, string> request)
    {
        foreach (string key in request.Keys)
        {
            switch (key)
            {
                case "Title":
                    title.text = request[key];
                    break;
                case "Sprite":
                    Sprite sprite = Resources.Load<Sprite>(request[key]);
                    if (sprite != null) image.sprite = sprite; 
                    break;
            }
        }
    }
}
