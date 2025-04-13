using UnityEngine;
using System.Collections;

public class LookAtTutorialBook : MonoBehaviour
{
    public Camera playerCamera;          
    public GameObject uiPanel;
    public GameObject secondUIPanel;
    public CameraMovement moveCam;
    public PlayerMovement movePlayer;
    public float maxDistance = 3f;       
    public string objectTag = "TutorialBook"; 
    private bool isLookingAtObject = false;

    public TutorialDialogueTrigger tutorialDialogueTrigger;
    public TutorialDialogueManager tutorialDialogueManager;
    public Animator animator;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag(objectTag))
            {
                if (!secondUIPanel.activeSelf)
                {
                    uiPanel.SetActive(true);
                }
                isLookingAtObject = true;
            }
            else
            {
                uiPanel.SetActive(false);
                isLookingAtObject = false;
            }
        }
        else
        {
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

            StartCoroutine(WaitForAnimationToFinish());
        }
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        animator.SetBool("IsOpen", false);

        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationDuration);

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