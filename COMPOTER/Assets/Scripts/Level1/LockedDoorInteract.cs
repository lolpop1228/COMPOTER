using UnityEngine;

public class LockedDoorInteract : MonoBehaviour, IInteractable
{
    public GameObject lockedDialogue;
    private bool hasActivated = false;

    public void Interact()
    {
        if ((lockedDialogue != null) && !hasActivated)
        {
            lockedDialogue.SetActive(true);
            hasActivated = true;
        }
    }
}
