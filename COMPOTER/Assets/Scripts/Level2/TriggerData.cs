using UnityEngine;
using UnityEngine.Playables;

public class TriggerData : MonoBehaviour, IInteractable
{
    public PlayableDirector playableDirector;
    public AudioClip soundEffect;
    public AudioClip moreSoundEffect;
    private bool hasTriggered = false;
    public AudioSource audioSource;

    public void Interact()
    {
        if (!hasTriggered)
        {
            playableDirector.Play();
            audioSource.PlayOneShot(soundEffect);
            audioSource.PlayOneShot(moreSoundEffect);
            hasTriggered = true;
            Destroy(gameObject);
        }
    }
}
