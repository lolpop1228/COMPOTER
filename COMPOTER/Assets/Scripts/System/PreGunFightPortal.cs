using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class PreGunFightPortal : MonoBehaviour
{
    public string animationToPlay;
    public Animator animator;

    void OnEnable()
    {
        if (animator != null)
        {
            animator.Play(animationToPlay);
        }
    }
}
