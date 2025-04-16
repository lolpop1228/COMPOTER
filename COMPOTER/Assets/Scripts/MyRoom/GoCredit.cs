using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoCredit : MonoBehaviour, IInteractable
{
    public string creditScene;
    public Animator animator;
    public string animToPlay;

    public void Interact()
    {
        animator.Play(animToPlay);
        StartCoroutine(LoadCredit());
    }

    IEnumerator LoadCredit()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(creditScene);
    }
}
