using UnityEngine;
using UnityEngine.Playables; // Needed for PlayableDirector

public class ButtonInteraction : MonoBehaviour
{
    public Transform player; // The player's transform
    public Camera playerCamera; // Reference to the player's camera
    public float maxDistance = 5f; // Maximum raycast distance
    public string buttonTag = "Button"; // Tag of the button
    public float interactionRange = 2f; // The range to trigger interaction

    public GameObject cube; // Reference to the cube you want to reset
    private Vector3 originalPosition; // Store the original position of the cube

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip interactionSound; // Sound to play when interaction occurs

    public PlayableDirector playableDirector; // Reference to the PlayableDirector component for Timeline

    private void Start()
    {
        // Store the original position of the cube at the start
        if (cube != null)
        {
            originalPosition = cube.transform.position;
        }

        // Ensure audioSource is set
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Ensure playableDirector is set
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
    }

    private void Update()
    {
        // Cast a ray from the camera's viewpoint to detect the button
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if the ray hit an object with the "Button" tag
            if (hit.collider.CompareTag(buttonTag))
            {
                // Now check if the player is within the interaction range and presses the 'E' key
                float distance = Vector3.Distance(player.position, transform.position);
                if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
                {
                    ResetCubePosition();
                    PlayInteractionSound();
                    PlayTimeline();
                }
            }
        }
    }

    // Function to reset the cube position to the original position
    void ResetCubePosition()
    {
        if (cube != null)
        {
            cube.transform.position = originalPosition; // Reset position of the cube
        }
    }

    // Function to play the interaction sound
    void PlayInteractionSound()
    {
        if (audioSource != null && interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound); // Play the sound once
        }
    }

    // Function to play the Timeline animation
    void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play(); // Play the Timeline from the start
        }
    }
}
