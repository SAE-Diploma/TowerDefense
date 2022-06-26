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

    [SerializeField] UnityEvent onAgreed = new UnityEvent();
    public UnityEvent OnAgreed => onAgreed;

    [SerializeField] UnityEvent onDisagreed = new UnityEvent();
    public UnityEvent OnDisagreed => onDisagreed;

    void Start()
    {
        agreeButton.onClick.AddListener(OnAgreeButtonClicked);
        disagreeButton.onClick.AddListener(OnDisgreeButtonClicked);
    }

    /// <summary>
    /// Set the title and content text
    /// </summary>
    /// <param name="title">title text</param>
    /// <param name="content">content text</param>
    public void SetValues(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
    }

    /// <summary>
    /// Set the texts of the buttons
    /// </summary>
    /// <param name="disagreeText">text for disagree button</param>
    /// <param name="agreeText">text for agree text</param>
    public void SetButtonTexts(string disagreeText, string agreeText)
    {
        disagreeButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = disagreeText;
        agreeButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = agreeText;
    }

    /// <summary>
    /// Invoke Agreed event
    /// </summary>
    private void OnAgreeButtonClicked() { OnAgreed.Invoke(); }

    /// <summary>
    /// Invoke Disagree event
    /// </summary>
    private void OnDisgreeButtonClicked() { onDisagreed.Invoke(); }


}
