using UnityEngine;

public class LookAtTutorialBook : MonoBehaviour
{
    public Camera playerCamera;          // Reference to the player's camera
    public GameObject uiPanel;           // The UI panel that will appear when the player looks at the object
    public GameObject secondUIPanel;     // The new UI panel that will appear when the player presses E
    public CameraMovement moveCam;
    public PlayerMovement movePlayer;
    public float maxDistance = 3f;       // Maximum distance to detect the TutorialBook
    public string objectTag = "TutorialBook"; // The tag of the object to look at

    private bool isLookingAtObject = false; // Flag to check if the player is looking at the object

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); // Cast a ray from the camera's position forward

        // Cast the ray to detect objects within maxDistance
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if the ray hit an object with the "TutorialBook" tag
            if (hit.collider.CompareTag(objectTag))
            {
                // Only show the UI panel if the second UI panel is not active
                if (!secondUIPanel.activeSelf && !uiPanel.activeSelf)
                {
                    uiPanel.SetActive(true);  // Show the first UI panel
                }
                isLookingAtObject = true;
            }
            else
            {
                // Hide the UI panel if the player is not looking at the object
                if (uiPanel.activeSelf)
                {
                    uiPanel.SetActive(false);
                }
                isLookingAtObject = false;
            }
        }
        else
        {
            // Hide the UI panel if no object is being looked at
            if (uiPanel.activeSelf)
            {
                uiPanel.SetActive(false);
            }
            isLookingAtObject = false;
        }

        // Check if the player presses the "E" key and they are looking at the object
        if (isLookingAtObject && Input.GetKeyDown(KeyCode.E))
        {
            // Hide the current UI panel when the player presses "E"
            if (uiPanel.activeSelf)
            {
                uiPanel.SetActive(false);  // Hide the first UI panel immediately
            }

            // Show the second UI panel
            if (!secondUIPanel.activeSelf)
            {
                secondUIPanel.SetActive(true);

                // Disable player and camera movement
                if (moveCam != null && movePlayer != null)
                {
                    moveCam.enabled = false;
                    movePlayer.enabled = false;
                }

                // Unlock the cursor and make it visible
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // Check if the second UI panel is active and player presses "Backspace" to close it
        if (secondUIPanel.activeSelf && Input.GetKeyDown(KeyCode.Backspace))
        {
            // Close the second UI panel
            secondUIPanel.SetActive(false);

            // Re-enable player and camera movement
            if (moveCam != null && movePlayer != null)
            {
                moveCam.enabled = true;
                movePlayer.enabled = true;
            }

            // Lock the cursor again
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
