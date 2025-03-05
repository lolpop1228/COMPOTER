using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TurnPCOn : MonoBehaviour, IInteractable
{
    public VideoPlayer videoPlayer;

    public void Interact()
    {
        videoPlayer.Play();
        Debug.Log("Kuy");
    }
}
