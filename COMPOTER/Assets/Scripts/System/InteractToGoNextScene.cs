using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractToGoNextScene : MonoBehaviour, IInteractable
{
    public string sceneToGo;
    public Animator fadeAnimator; // Assign the Animator from your fade UI
    public string fadeAnimationName = "FadeIn"; // Name of your fade animation state
    public float fadeDuration = .5f; // Duration of the fade animation

    public void Interact()
    {
        StartCoroutine(PlayFadeAndLoad());
    }

    private IEnumerator PlayFadeAndLoad()
    {
        // Play the fade animation directly by name
        if (fadeAnimator != null)
        {
            fadeAnimator.Play(fadeAnimationName);
        }

        // Wait for the fade to complete
        yield return new WaitForSeconds(fadeDuration);

        // Load the next scene
        SceneManager.LoadScene(sceneToGo);
    }
}
