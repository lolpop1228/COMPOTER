using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineAutoPlayer : MonoBehaviour
{
    public PlayableDirector director;

    void OnEnable()
    {
        director.Play();
        Time.timeScale = 1.0f;
    }
}
