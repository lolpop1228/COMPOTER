using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public PlayerMovement playerMovement;
    public AudioSource audioSource; // Audio source for typing sound
    public AudioClip typingSound; // Audio clip reference

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                audioSource.Stop(); // Stop sound when skipping
            }
        }
    }

    void StartDialogue()
    {
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
                audioSource.PlayOneShot(typingSound); // Play sound for each letter
            }

            yield return new WaitForSeconds(textSpeed);
        }
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
        playerMovement.enabled = true;
        audioSource.Stop(); // Ensure the sound stops when dialogue is closed
    }
}