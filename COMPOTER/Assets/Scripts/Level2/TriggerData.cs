using UnityEngine;
using UnityEngine.Playables;

public class TriggerData : MonoBehaviour, IInteractable
{
    public PlayableDirector playableDirector;
    public AudioClip soundEffect;
    private bool hasTriggered = false;
    public AudioSource audioSource;

    public void Interact()
    {
        if (!hasTriggered)
        {
            playableDirector.Play();
            audioSource.PlayOneShot(soundEffect);
            hasTriggered = true;
            Destroy(gameObject);
        }
    }
}
