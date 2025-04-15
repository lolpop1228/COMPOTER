using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class OpenLevel1Scene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Camera targetCamera;
    public GameObject objectToEnable1;
    public GameObject objectToEnable3;
    public GameObject objectToEnable4;

    private void Start()
    {
        // Disable objects initially
        if (objectToEnable1 != null) objectToEnable1.SetActive(false);
        if (objectToEnable3 != null) objectToEnable3.SetActive(false);
        if (objectToEnable4 != null) objectToEnable4.SetActive(false);

        // Start timeline with a slight delay to ensure it's initialized
        if (playableDirector != null)
        {
            StartCoroutine(PlayTimelineAfterFrame());
        }
    }

    private IEnumerator PlayTimelineAfterFrame()
    {
        yield return null; // wait one frame
        playableDirector.Play();
        playableDirector.stopped += OnTimelineEnd;
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        // Enable other objects after the cutscene ends
        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(true);
        }

        if (objectToEnable3 != null)
        {
            objectToEnable3.SetActive(true);
        }

        if (objectToEnable4 != null)
        {
            objectToEnable4.SetActive(false);
        }

        // Unsubscribe from the event to prevent multiple calls
        playableDirector.stopped -= OnTimelineEnd;
    }
}
