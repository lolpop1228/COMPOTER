using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour, IInteractable
{
    public Animator animator;
    public string animToPlay;

    public void Interact()
    {
        if (animator != null)
        {
            animator.Play(animToPlay);
        }
    }
}
