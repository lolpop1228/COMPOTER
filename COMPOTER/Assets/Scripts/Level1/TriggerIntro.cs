using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerIntro : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject cutsceneCam;
    public PlayableDirector playableDirector;

    void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        cutsceneCam.SetActive(true);
        thePlayer.SetActive(false);
        playableDirector.Play();
        StartCoroutine(FinishCut());
    }

    IEnumerator FinishCut()
    {
        yield return new WaitForSeconds(9);
        thePlayer.SetActive(true);
        cutsceneCam.SetActive(false);
}
}