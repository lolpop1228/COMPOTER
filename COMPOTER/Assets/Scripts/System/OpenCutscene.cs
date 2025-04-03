using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class OpenCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Camera targetCamera;
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f);
    public GameObject objectToEnable1;
    public GameObject objectToEnable2; // UI Text
    public GameObject objectToEnable3;
    public float delayBeforeActivate = 1f; // Delay before making objectToEnable2 active
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            playableDirector.stopped += OnTimelineEnd;
        }

        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(false);
        }

        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(false); // Initially hide it
        }

        if (objectToEnable3 != null)
        {
            objectToEnable3.SetActive(false);
        }
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        // Rotate the camera if it's assigned
        if (targetCamera != null)
        {
            targetCamera.transform.rotation = Quaternion.Euler(targetRotation);
        }

        // Enable other objects after the cutscene ends
        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(true);
        }

        if (objectToEnable2 != null)
        {
            StartCoroutine(ActivateAndFadeIn(objectToEnable2, delayBeforeActivate, fadeDuration));
        }

        if (objectToEnable3 != null)
        {
            objectToEnable3.SetActive(true);
        }

        // Unsubscribe from the event to prevent multiple calls
        playableDirector.stopped -= OnTimelineEnd;
    }

    // Coroutine to wait before enabling and fading in UI text
    private IEnumerator ActivateAndFadeIn(GameObject uiObject, float delayBeforeActivate, float fadeDuration)
    {
        // Wait for the specified delay before activating the object
        yield return new WaitForSeconds(delayBeforeActivate);

        // Activate the object after the delay
        uiObject.SetActive(true);

        // Start fading in the UI text after it is active
        yield return StartCoroutine(FadeInUI(uiObject, fadeDuration));
    }

    // Coroutine for fading in UI text using CanvasGroup
    private IEnumerator FadeInUI(GameObject uiObject, float duration)
    {
        // Add CanvasGroup if not already present
        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiObject.AddComponent<CanvasGroup>();
        }

        // Start with fully transparent
        canvasGroup.alpha = 0f;

        // Fade in over the specified duration
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);  // Smoothly transition from 0 to 1
            yield return null;
        }

        // Ensure it ends at fully visible
        canvasGroup.alpha = 1f;
    }
}
