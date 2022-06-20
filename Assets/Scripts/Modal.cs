using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Modal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] Button agreeButton;
    [SerializeField] Button disagreeButton;

    [SerializeField] UnityEvent OnAgreed = new UnityEvent();
    [SerializeField] UnityEvent OnDisagreed = new UnityEvent();

    void Start()
    {
        SetValues("start new game", "All your progress will be overwritten!\nDo you still want to continue?");
        agreeButton.onClick.AddListener(OnAgreeButtonClicked);
        disagreeButton.onClick.AddListener(OnDisgreeButtonClicked);
    }

    void Update()
    {
        
    }

    public void SetValues(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
    }

    public void SetButtonTexts(string disagreeText, string agreeText)
    {
        disagreeButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = disagreeText;
        agreeButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = agreeText;
    }

    private void OnAgreeButtonClicked() { OnAgreed.Invoke(); }
    private void OnDisgreeButtonClicked() { OnDisagreed.Invoke(); }


}
