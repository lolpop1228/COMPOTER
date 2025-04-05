using UnityEngine;
using UnityEngine.Playables;
using TMPro;  // Make sure to include this for TextMeshPro

public class ButtonTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject newButton;
    private Renderer buttonRenderer;
    private AudioSource audioSource; // Reference to AudioSource component
    public AudioClip collisionSound1; // First sound effect
    public AudioClip collisionSound2; // Second sound effect

    // Reference to TextMeshPro objects
    public GameObject textToDeactivate; // The TextMeshPro object to deactivate
    public GameObject textToActivate; // The TextMeshPro object to activate

    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>(); // Get the renderer of the button
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (newButton != null)
        {
            newButton.SetActive(false); // Ensure the new button is hidden initially
        }

        if (textToDeactivate != null)
        {
            textToDeactivate.SetActive(true); // Ensure the text to deactivate is initially active
        }

        if (textToActivate != null)
        {
            textToActivate.SetActive(false); // Ensure the text to activate is initially hidden
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with the button is the cube
        if (collision.gameObject.CompareTag("ThrowObject"))
        {
            // Play the first collision sound
            if (audioSource != null && collisionSound1 != null)
            {
                audioSource.PlayOneShot(collisionSound1);
            }

            // Play the second collision sound
            if (audioSource != null && collisionSound2 != null)
            {
                audioSource.PlayOneShot(collisionSound2);
            }

            // Change the tag of the cube after it collides with the button to "Breaker"
            collision.gameObject.tag = "Breaker";

            // Play the Timeline to open the door
            if (playableDirector != null)
            {
                playableDirector.Play();
            }

            // Make the current button disappear
            if (buttonRenderer != null)
            {
                buttonRenderer.enabled = false; // Hides the button renderer
            }

            // Enable the new button to appear
            if (newButton != null)
            {
                newButton.SetActive(true); // Makes the new button active
            }

            // Deactivate the old TextMeshPro object
            if (textToDeactivate != null)
            {
                textToDeactivate.SetActive(false);
            }

            // Activate the new TextMeshPro object
            if (textToActivate != null)
            {
                textToActivate.SetActive(true);
            }

            // Disable further collision with this button to prevent re-triggering
            GetComponent<Collider>().enabled = false;
        }
    }
}
