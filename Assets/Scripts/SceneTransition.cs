using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Image sceneTransition;
    [SerializeField] float transitionTime;
    private Animator animator;

    private void Awake()
    {
        animator = sceneTransition.GetComponent<Animator>();
    }

    /// <summary>
    /// Transitions out and loads the scene by index
    /// </summary>
    /// <param name="sceneIndex">index of the scene to load</param>
    public void TransitionTo(int sceneIndex)
    {
        sceneTransition.transform.SetAsLastSibling();
        StartCoroutine(Transition(() => SceneManager.LoadScene(sceneIndex)));
    }

    /// <summary>
    /// Transitions out and quits the Game
    /// </summary>
    public void ExitGame()
    {
        sceneTransition.transform.SetAsLastSibling();
        StartCoroutine(Transition(() => Application.Quit()));
    }

    /// <summary>
    /// Plays animation and runs the function
    /// </summary>
    /// <param name="function">function run after transition</param>
    /// <returns></returns>
    private IEnumerator Transition(UnityAction function)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(transitionTime);
        function();
    }

}
