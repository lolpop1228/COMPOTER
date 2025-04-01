using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogueManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text dialogueText;
    public Animator animator;
    private Queue<string> sentences;
    public CameraMovement moveCam;
    public PlayerMovement movePlayer;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(TutorialDialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        titleText.text = dialogue.title;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

        if (moveCam != null && movePlayer != null)
        {
            moveCam.enabled = true;
            movePlayer.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}