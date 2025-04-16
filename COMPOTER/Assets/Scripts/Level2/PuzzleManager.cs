using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public GameObject altar;
    public PlayableDirector playableDirector;
    private string binaryInput = "";
    private readonly string[] targetSequence = { "01010000", "01010011", "01010101" };
    // "01010000", "01010011", "01010101"
    private int currentTargetIndex = 0;

    // Method to update the UI when a cube collides with the button
    public void AddBinaryDigit(string digit)
    {
        binaryInput += digit;
        uiText.text = binaryInput;

        // Check if the current binary input matches the target sequence
        if (binaryInput == targetSequence[currentTargetIndex])
        {
            currentTargetIndex++; // Move to the next sequence
            binaryInput = ""; // Clear input for the next sequence
            uiText.text = "No Input"; // Reset UI text

            // If we've completed all three sequences, activate the altar
            if (currentTargetIndex == targetSequence.Length)
            {
                altar.SetActive(true);
                playableDirector.Play();
                uiText.text = "Puzzle complete!";
            }
            else
            {
                uiText.text = "Next puzzle!";
            }
        }
    }

    // Public method to reset the binary input
    public void ResetBinaryInput()
    {
        binaryInput = "";
        uiText.text = "No Input"; // Optionally reset the UI text here too
    }
}
