using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineAutoPlayer : MonoBehaviour
{
    public PlayableDirector director;

    void OnEnable()
    {
        Time.timeScale = 1.0f;
    }
}
