using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public Camera playerCamera;
    public float grabRange = 5f;
    public float throwForce = 500f;
    public Transform holdPoint;
    public LayerMask grabbableLayer;
    
    private Rigidbody grabbedObject;
    private bool isHolding = false;
    private float holdDistance = 3f; // Default distance
    private float minDistance = 1f;  // Minimum hold distance
    private float maxDistance = 10f; // Maximum hold distance
    private float scrollSpeed = 2f;  // Speed of scrolling movement

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Left-click to grab/drop
        {
            if (isHolding) DropObject();
            else TryGrabObject();
        }

        if (isHolding && grabbedObject != null)
        {
            MoveObject();

            // Scroll to adjust hold distance
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            holdDistance = Mathf.Clamp(holdDistance + scroll * scrollSpeed, minDistance, maxDistance);
        }

        if (Input.GetMouseButtonDown(1) && isHolding) // Right-click to throw
        {
            ThrowObject();
        }
    }

    void TryGrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabRange, grabbableLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;
                grabbedObject.drag = 10f; // Increase drag for smooth movement
                isHolding = true;
                holdDistance = Vector3.Distance(playerCamera.transform.position, grabbedObject.position); // Set hold distance
            }
        }
    }

    void MoveObject()
    {
        if (grabbedObject == null) return;
        
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        Vector3 direction = targetPosition - grabbedObject.position;
        grabbedObject.velocity = direction * 10f; // Smooth movement
    }

    void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.drag = 0;
            grabbedObject = null;
            isHolding = false;
        }
    }

    void ThrowObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.drag = 0;
            grabbedObject.AddForce(playerCamera.transform.forward * throwForce);
            grabbedObject = null;
            isHolding = false;
        }
    }
}
