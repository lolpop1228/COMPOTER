using UnityEngine;
using TMPro;

public class ResetButton : MonoBehaviour, IInteractable
{
    public TextMeshProUGUI uiText;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        // Reset the binary input in GameManager
        gameManager.ResetBinaryInput();
    }
}
