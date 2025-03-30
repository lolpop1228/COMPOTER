using UnityEngine;
using UnityEngine.Playables;

public class ButtonTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject newButton;
    private Renderer buttonRenderer;

    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>(); // Get the renderer of the button (optional, if you want to make it disappear visually)
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with the button is the cube
        if (collision.gameObject.CompareTag("ThrowObject"))
        {
            // Change the tag of the cube after it collides with the button to "Breakable"
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
        }
    }
}
