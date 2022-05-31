using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Panels : int
{
    Interaction = 0,
    ChooseTower = 1
}


public class UIManager : MonoBehaviour
{

    [SerializeField] List<Image> panels;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePanel(Panels panel)
    {
        //panels[(int)panel].enabled = !panels[(int)panel].enabled;
        GameObject obj = panels[(int)panel].gameObject;
        obj.SetActive(!obj.activeSelf);
    }


}
