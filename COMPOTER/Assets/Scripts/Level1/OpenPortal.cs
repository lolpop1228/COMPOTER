using UnityEngine;

public class OpenPortal : MonoBehaviour
{
    public string animationToPlay;
    public Animator animator;

    void Start()
    {
        animator.Play(animationToPlay);
    }
}
