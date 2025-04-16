using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip doorSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(doorSound);
            }
        }
    }
}
