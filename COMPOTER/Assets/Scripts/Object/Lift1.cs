using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift1 : MonoBehaviour, IInteractable
{
    public Animator animator1;
    public Animator animator2;
    public string animToPlay1;
    public string animToPlay2;

    public void Interact()
    {
        if (animator1 != null)
        {
            animator1.Play(animToPlay1);
        }
        if (animator2 != null)
        {
            animator2.Play(animToPlay2);
        }
    }
}
