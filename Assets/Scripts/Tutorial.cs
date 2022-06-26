using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField, Tooltip("in seconds")] float autoProgressTime;
    [SerializeField] private List<GameObject> states;
    private SceneTransition transition;

    private Coroutine autoProgressRoutine;

    private int state = 0;
    public int State
    {
        get { return state; }
        private set
        {
            state = value;
            ShowState(state);
            if (autoProgressRoutine != null)
            {
                StopCoroutine(autoProgressRoutine);
                autoProgressRoutine = StartCoroutine(AutoProgress(autoProgressTime));
            }
            
        }

    }

    private void Start()
    {
        transition = GetComponent<SceneTransition>();
        autoProgressRoutine = StartCoroutine(AutoProgress(autoProgressTime));
        ShowState(0);
    }

    /// <summary>
    /// increment the state by an amount
    /// </summary>
    /// <param name="increment">increment amount</param>
    public void NextState(int increment)
    {
        State += increment;
    }

    /// <summary>
    /// sets the stateIndex active and the others inactive
    /// </summary>
    /// <param name="stateIndex">stateIndex to show</param>
    private void ShowState(int stateIndex)
    {
        if (stateIndex < states.Count)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (i == stateIndex) states[i].SetActive(true);
                else states[i].SetActive(false);
            }
        }
        else
        {
            transition.TransitionTo(2);
        }
    }

    /// <summary>
    /// Coroutine for automaticly increment the state by one
    /// </summary>
    /// <param name="waitTime">time until next State</param>
    /// <returns></returns>
    private IEnumerator AutoProgress(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextState(1);
    }
}
