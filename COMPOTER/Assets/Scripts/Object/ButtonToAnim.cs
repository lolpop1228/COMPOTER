using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToAnim : MonoBehaviour
{
    public Animator animator;
    public string animToPlay;
    private AudioSource audioSource;
    public AudioClip pressSound;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (animator != null)
            {
                animator.Play(animToPlay);
            }

            if (audioSource != null)
            {
                audioSource.PlayOneShot(pressSound);
            }
        }
    }
}
