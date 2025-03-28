using UnityEngine;
using UnityEngine.Playables;  // Required to use PlayableDirector
using TMPro;  // For handling TextMeshProUGUI components
using System.Collections;   // For using coroutines

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector;  // Reference to the Playable Director (Timeline)
    public GameObject tutorialUI;             // Reference to the tutorial UI GameObject
    public TextMeshProUGUI tutorialText;      // Reference to the TextMeshProUGUI component for tutorial messages
    public float delayBeforeUI = 2f;          // Delay before the UI appears, in seconds

    private bool hasMoved = false;  // Flag to check if the player has pressed WASD
    private bool hasJumped = false;  // Flag to check if the player has pressed Spacebar

    void Start()
    {
        // Ensure the tutorial UI is initially hidden
        tutorialUI.SetActive(false); // Hide UI initially

        // Start playing the cutscene
        playableDirector.Play();
    }

    void Update()
    {
        // Check if the cutscene has finished playing
        if (playableDirector.state == PlayState.Paused && !tutorialUI.activeSelf)
        {
            // Once the cutscene finishes, show the tutorial UI with fade-in after a delay
            ShowTutorialUI();
        }

        // Handle player input and update tutorial text based on the player's actions
        if (!hasMoved && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || 
                          Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            hasMoved = true;
            tutorialText.text = "Press Spacebar to jump";
        }

        if (!hasJumped && Input.GetKeyDown(KeyCode.Space))
        {
            hasJumped = true;
            tutorialText.text = "Well done! Tutorial Complete";
        }
    }

    // Method to start fading in the tutorial UI
    void ShowTutorialUI()
    {
        StartCoroutine(FadeInTutorialUI());
    }

    // Coroutine to fade in the tutorial UI with a delay
    IEnumerator FadeInTutorialUI()
    {
        // Wait for the specified delay before showing the UI
        yield return new WaitForSeconds(delayBeforeUI);

        // Ensure the tutorial UI has a CanvasGroup for alpha manipulation
        CanvasGroup canvasGroup = tutorialUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tutorialUI.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;  // Start with the UI completely transparent
        tutorialUI.SetActive(true);  // Ensure the UI is active

        float fadeDuration = 1f;  // Duration of the fade-in effect (in seconds)
        float startTime = Time.time;

        // Gradually increase alpha to 1 over the fade duration
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;  // Ensure the alpha is fully 1 after fading
    }
}
