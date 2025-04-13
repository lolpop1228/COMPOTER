using UnityEngine;
using UnityEngine.Playables;

public class ButtonInteraction : MonoBehaviour, IInteractable
{
    public Transform player;
    public Camera playerCamera;
    public float maxDistance = 5f;
    public string buttonTag = "Button";
    public float interactionRange = 2f;

    public GameObject cube;
    private Vector3 originalPosition;

    public AudioSource audioSource;
    public AudioClip interactionSound;

    public PlayableDirector playableDirector;

    private void Start()
    {
        if (cube != null)
        {
            originalPosition = cube.transform.position;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Interact()
    {
        {
            
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                ResetCubePosition();
                PlayInteractionSound();
                PlayTimeline();
            }
        }
    }

    void ResetCubePosition()
    {
        if (cube != null)
        {
            cube.transform.position = originalPosition;
        }
    }

    void PlayInteractionSound()
    {
        if (audioSource != null && interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
    }

    void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }
}
