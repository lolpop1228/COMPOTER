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

    public void AddBinaryDigit(string digit)
    {
        binaryInput += digit;
        uiText.text = binaryInput;

        if (binaryInput == targetSequence[currentTargetIndex])
        {
            currentTargetIndex++;
            binaryInput = "";
            uiText.text = "NO INPUT";

            if (currentTargetIndex == targetSequence.Length)
            {
                altar.SetActive(true);
                playableDirector.Play();
                uiText.text = "COMPLETE";
            }
            else
            {
                uiText.text = "NEXT";
            }
        }
    }

    public void ResetBinaryInput()
    {
        binaryInput = "";
        uiText.text = "RESET";
    }
}
