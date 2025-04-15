using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public AudioClip doorSound;
    public float triggerDistance = 10f;
    private Transform player;
    private bool hasPlayed = false;
    private AudioSource audioSource;

    void Start()
    {
        player = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!hasPlayed && Vector3.Distance(player.position, transform.position) <= triggerDistance)
        {
            PlayAnimation();
            PlaySound();
        }
    }

    private void PlayAnimation()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            hasPlayed = true;
        }
    }

    private void PlaySound()
    {
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
    }
}
