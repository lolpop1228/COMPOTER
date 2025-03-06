using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TurnPCOn : MonoBehaviour, IInteractable
{
    public VideoPlayer videoPlayer;
    bool hasPlayed = false;
    public GameObject interactLayer;

    // timer
    public float timeDuration;
    private float timeRemaining;
    bool isRunning = false;

    private void Start()
    {
        interactLayer.SetActive(false);
    }

    public void Interact()
    {
        if (videoPlayer != null && !hasPlayed)
        {
            videoPlayer.Play();
            hasPlayed = true;          
        }

        StartTimer(timeDuration);
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isRunning = true;
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        isRunning = false;
        TimerFinished();
    }

    private void TimerFinished()
    {
        interactLayer.SetActive(true);
        Debug.Log("Timer Finished");
    }
}
