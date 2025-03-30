using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public Transform player; // The player's transform
    public Camera playerCamera; // Reference to the player's camera
    public float maxDistance = 5f; // Maximum raycast distance
    public string buttonTag = "Button"; // Tag of the button
    public float interactionRange = 2f; // The range to trigger interaction

    public GameObject cube; // Reference to the cube you want to reset
    private Vector3 originalPosition; // Store the original position of the cube

    private void Start()
    {
        // Store the original position of the cube at the start
        if (cube != null)
        {
            originalPosition = cube.transform.position;
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
}
