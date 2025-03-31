using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToAnim : MonoBehaviour
{
    public Animator animator;
    public string animToPlay;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (animator != null)
            {
                animator.Play(animToPlay);
            }
        }
    }
}
