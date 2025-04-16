using UnityEngine;
using TMPro;

public class ResetButton : MonoBehaviour, IInteractable
{
    public TextMeshProUGUI uiText;
    private GameManager gameManager;
    public AudioClip interactionSound;
    private AudioSource audioSource;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (interactionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
        
        gameManager.ResetBinaryInput();
    }
}
