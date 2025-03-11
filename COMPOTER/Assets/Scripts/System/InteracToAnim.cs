using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracToAnim : MonoBehaviour, IInteractable
{
    public string animToPlay;
    public string objectAnim;
    public Animator targetAnimator;
    public Animator animator;

    public void Interact()
    {
        if (animator != null)
        {
            animator.Play(objectAnim);
        }

        if (targetAnimator != null)
        {
            targetAnimator.Play(animToPlay);
        }
    }
}
