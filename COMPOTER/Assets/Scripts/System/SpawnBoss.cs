using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Animator doorAnim;
    public string animToPlay;

    void Start()
    {
        DisableObjects();
    }

    void OnDestroy()
    {
        EnableObjects();
        if (doorAnim != null)
        {
            doorAnim.Play(animToPlay);
        }
    }

    void DisableObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    void EnableObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
