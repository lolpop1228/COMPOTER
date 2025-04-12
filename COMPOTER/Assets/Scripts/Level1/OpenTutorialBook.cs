using UnityEngine;
using System.Collections;

public class LookAtTutorialBook : MonoBehaviour
{
    public Camera playerCamera;          
    public GameObject uiPanel;           // The UI panel that will appear when the player looks at the object
    public GameObject secondUIPanel;     // The new UI panel that will appear when the player presses E
    public CameraMovement moveCam;
    public PlayerMovement movePlayer;
    public float maxDistance = 3f;       
    public string objectTag = "TutorialBook"; 
    private bool isLookingAtObject = false; // Flag to check if the player is looking at the object

    public TutorialDialogueTrigger tutorialDialogueTrigger;  // Reference to the TutorialDialogueTrigger
    public TutorialDialogueManager tutorialDialogueManager;  // Reference to the TutorialDialogueManager to trigger EndDialogue
    public Animator animator;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        // Cast the ray to detect objects within maxDistance
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag(objectTag))
            {
                // Show the UI panel only if the second UI panel isn't active
                if (!secondUIPanel.activeSelf)
                {
                    uiPanel.SetActive(true);  // Show the first UI panel
                }
                isLookingAtObject = true;
            }
            else
            {
                // Hide UI panel if not looking at the object
                uiPanel.SetActive(false);
                isLookingAtObject = false;
            }
        }
        else
        {
            // Hide the UI panel if no object is being looked at
            uiPanel.SetActive(false);
            isLookingAtObject = false;
        }

        if (isLookingAtObject && Input.GetKeyDown(KeyCode.E))
        {
            uiPanel.SetActive(false);
            secondUIPanel.SetActive(true);

            if (moveCam != null && movePlayer != null)
            {
                moveCam.enabled = false;
                movePlayer.enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            tutorialDialogueTrigger.TriggerDialogue();
        }

        if (secondUIPanel.activeSelf && Input.GetKeyDown(KeyCode.Backspace))
        {
            if (tutorialDialogueManager != null)
            {
                tutorialDialogueManager.EndDialogue();
            }

            // Start the coroutine to handle the animation and panel deactivation.
            StartCoroutine(WaitForAnimationToFinish());
        }
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        // Set the animator to trigger the closing animation.
        animator.SetBool("IsOpen", false);

        // Wait until the animation has finished.
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait until the animation is done (you can also adjust this based on specific conditions like animation events).
        yield return new WaitForSeconds(animationDuration);

        // Now perform the panel deactivation after the animation finishes.
        if (moveCam != null && movePlayer != null)
        {
            moveCam.enabled = true;
            movePlayer.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        secondUIPanel.SetActive(false);

        if (isLookingAtObject)
        {
            uiPanel.SetActive(true);
        }
    }
}