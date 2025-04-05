using UnityEngine;

public class LookAtAndInteract : MonoBehaviour
{
    public Camera playerCamera;         // Reference to the player's camera
    public GameObject objectToEnable;   // The object to enable when "F" is pressed
    public float interactionRange = 5f; // The range of interaction (raycast distance)
    public float interactionAngle = 30f; // The angle in degrees the player needs to be looking at the object

    private RaycastHit hit;

    void Update()
    {
        // Perform a raycast from the center of the camera's view (where the player is looking)
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); 
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Check if the object hit by the ray is the object we want to interact with
            if (hit.collider.CompareTag("ThrowObject"))
            {
                // Check if the player is looking within the interaction angle
                Vector3 targetDirection = hit.transform.position - playerCamera.transform.position;
                float angle = Vector3.Angle(targetDirection, playerCamera.transform.forward);

                if (angle <= interactionAngle) // The player is looking at the object within the given angle
                {
                    // Check for the "F" key press to enable the object
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        EnableObject();
                    }
                }
            }
        }
    }

    // Enable the target object when "F" is pressed
    private void EnableObject()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}
