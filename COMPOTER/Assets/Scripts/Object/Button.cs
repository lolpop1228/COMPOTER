using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Animator doorAnim;
    public string animToPlay1;
    public string animToPlay2;
    public GameObject[] objectsToEnable;
    private AudioSource audioSource;
    public AudioClip pressSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (doorAnim != null)
            {
                doorAnim.Play(animToPlay1);
            }

            if (audioSource != null)
            {
                audioSource.PlayOneShot(pressSound);
            }

            if (objectsToEnable[0] != null)
                objectsToEnable[0].SetActive(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (doorAnim != null)
            {
                doorAnim.Play(animToPlay2);
            }
    }
}
