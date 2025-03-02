using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class InteractionSystem : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public LayerMask InteractableLayer;
    public GameObject interactUI;

    private void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, InteractableLayer))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                if (interactUI != null)
                {
                    interactUI.gameObject.SetActive(true);
                }
            }    
        }
        else
        {
            if (interactUI != null)
            {
                interactUI.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(r, out RaycastHit hitInfoInteract, InteractRange, InteractableLayer))
            {
                if (hitInfoInteract.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
