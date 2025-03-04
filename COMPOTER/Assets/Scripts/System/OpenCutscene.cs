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

        playableDirector.stopped -= OnTimelineEnd;
    }
}
