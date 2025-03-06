using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public float lineDelay = 1.5f; // Delay before the next line starts
    public PlayerMovement playerMovement;
    public AudioSource audioSource;
    public AudioClip typingSound;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;

            if (typingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        // Wait for a moment before showing the next line
        yield return new WaitForSeconds(lineDelay);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
