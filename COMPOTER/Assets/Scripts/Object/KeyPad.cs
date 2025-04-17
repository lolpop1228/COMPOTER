using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyPad : MonoBehaviour
{
    public Animator doorAnim;
    public string animToPlay;
    private AudioSource audioSource;
    public AudioClip activateSound;
    public AudioClip doorSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            if (doorAnim != null)
            {
                doorAnim.Play(animToPlay);
            }

            if (audioSource != null)
            {
                audioSource.PlayOneShot(activateSound);
                audioSource.PlayOneShot(doorSound);
            }
        }
    }
}
