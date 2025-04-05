using UnityEngine;

public class CubeInteractDialogue : MonoBehaviour
{
    public GameObject objectToActivate;
    public float activationRange = 3f;
    private Transform playerTransform;

    private bool hasActivated = false;  // Flag to track if the object has been activated

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Only activate the object if the player is close enough and the object hasn't been activated
            if (Vector3.Distance(transform.position, playerTransform.position) <= activationRange && !hasActivated)
            {
                SetActiveObject();
                hasActivated = true;  // Prevent further activations
            }
        }
    }

    void SetActiveObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);  // Activate the object
        }
    }
}
