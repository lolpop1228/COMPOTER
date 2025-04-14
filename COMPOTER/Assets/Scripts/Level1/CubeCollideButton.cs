using UnityEngine;
using UnityEngine.Playables;
using TMPro;  // Make sure to include this for TextMeshPro

public class ButtonTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject newButton;
    private Renderer buttonRenderer;
    private AudioSource audioSource;
    public AudioClip collisionSound1;
    public AudioClip collisionSound2;
    public GameObject textToDeactivate;
    public GameObject textToActivate;

    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ThrowObject"))
        {
            if (audioSource != null && collisionSound1 != null)
            {
                audioSource.PlayOneShot(collisionSound1);
            }

            if (audioSource != null && collisionSound2 != null)
            {
                audioSource.PlayOneShot(collisionSound2);
            }

            collision.gameObject.tag = "Breaker";
            playableDirector.Play();
            buttonRenderer.enabled = false;
            newButton.SetActive(true);
            textToDeactivate.SetActive(false);
            textToActivate.SetActive(true);

            GetComponent<Collider>().enabled = false;
        }
    }
}
