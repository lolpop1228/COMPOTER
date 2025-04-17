using UnityEngine;

public class InteractUITrigger : MonoBehaviour, IInteractable
{
    public GameObject uiPanel;
    private bool hasActivated = false;

    public void Interact()
    {
        if (!hasActivated)
        {
            uiPanel.SetActive(true);
            hasActivated = true;
        }
    }

}
