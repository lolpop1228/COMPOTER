using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TurnPCOn : MonoBehaviour, IInteractable
{
    public VideoPlayer videoPlayer;

    public void Interact()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
}
