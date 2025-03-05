using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TurnPCOn : MonoBehaviour, IInteractable
{
    public PlayableDirector playableDirector;
    public GameObject player;

    public void Interact()
    {
        playableDirector.Play();
        player.SetActive(false);
    }
}
