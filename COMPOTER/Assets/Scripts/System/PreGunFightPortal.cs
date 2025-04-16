using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class PreGunFightPortal : MonoBehaviour
{
    public string animationToPlay;
    public Animator animator;

    void Start()
    {
        StartCoroutine(PlayAnimationAfterDelay(18f));
    }

    private IEnumerator PlayAnimationAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.Play(animationToPlay);
    }
}
