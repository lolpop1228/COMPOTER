using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OpenCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public PlayerMovement playerMovement;
    public Camera targetCamera;
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f);
    public GameObject objectToEnable1;
    public GameObject objectToEnable2;

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            playableDirector.stopped += OnTimelineEnd;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(false);
        }

        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(false);
        }
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (targetCamera != null)
        {
            targetCamera.transform.rotation = Quaternion.Euler(targetRotation);
        }

        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(true);
        }

        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(true);
        }

        playableDirector.stopped -= OnTimelineEnd;
    }
}
