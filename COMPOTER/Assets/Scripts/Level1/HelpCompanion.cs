using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class HelpCompanion : MonoBehaviour, IInteractable
{
    public PlayableDirector playableDirector;
    public AudioClip soundEffect;
    private AudioSource audioSource;
    public string animationToPlay;
    public Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        playableDirector.Play();
        audioSource.PlayOneShot(soundEffect);
        StartCoroutine(PlayAnimationAfterDelay(40f));
    }

    private IEnumerator PlayAnimationAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.Play(animationToPlay);
    }
}
