using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Menus : int
{
    ChooseTower = 0,
}


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactionPanel;
    [SerializeField] List<Image> menus;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMenuVisibility(Menus menu,bool visibility)
    {
        GameObject obj = menus[(int)menu].gameObject;
        obj.SetActive(visibility);
    }

    public void SetInteractionVisibility(bool visibility)
    {
        interactionPanel.SetActive(visibility);
    }

}
