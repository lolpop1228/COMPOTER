using UnityEngine;

public class CubeInteractDialogue : MonoBehaviour
{
    public GameObject objectToActivate;
    public float activationRange = 3f;
    private Transform playerTransform;

    private bool hasActivated = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= activationRange && !hasActivated)
            {
                SetActiveObject();
                hasActivated = true;
            }
        }
    }

    void SetActiveObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
