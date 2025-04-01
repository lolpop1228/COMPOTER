using UnityEngine;
using UnityEngine.UI; // If you're working with UI elements

public class LookAtButton : MonoBehaviour
{
    public Camera playerCamera; // Reference to the player's camera
    public GameObject uiPanel;  // The UI panel that will appear
    public float maxDistance = 3f; // Maximum distance to detect the button
    public string buttonTag = "Button";

    private void Update()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        // Cast a ray from the camera's viewpoint to detect the button
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if the ray hit an object with the tag
            if (hit.collider.CompareTag(buttonTag))
            {
                // Show the UI panel if the player is looking at the object
                if (!uiPanel.activeSelf)
                {
                    uiPanel.SetActive(true);
                }
            }
            else
            {
                // Hide the UI panel if the player is not looking at the object
                if (uiPanel.activeSelf)
                {
                    uiPanel.SetActive(false);
                }
            }
        }
        else
        {
            // Hide the UI panel if no object is being looked at
            if (uiPanel.activeSelf)
            {
                uiPanel.SetActive(false);
            }
        }
    }
}
