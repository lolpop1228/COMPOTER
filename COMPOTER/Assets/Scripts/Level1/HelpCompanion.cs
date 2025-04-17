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

    private bool hasInteracted = false; // Add this line

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (hasInteracted) return; // Prevent multiple interactions

        hasInteracted = true; // Set flag so it doesn't happen again

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
