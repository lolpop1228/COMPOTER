using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableToPlayAnimm : MonoBehaviour
{
    public Animator animator;
    public string animToPlay;

    void OnEnable()
    {
        if (animator != null)
        {
            animator.Play(animToPlay);
        }
    }
}
