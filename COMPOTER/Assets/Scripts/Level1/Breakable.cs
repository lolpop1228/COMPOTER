using UnityEngine;
using System.Collections; 

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intactStuff;
    [SerializeField] GameObject brokenStuff;
    [SerializeField] AudioClip breakSound;
    private AudioSource audioSource;

    BoxCollider bc;

    private void Awake()
    {
        intactStuff.SetActive(true);
        brokenStuff.SetActive(false);

        bc = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Breaker"))
        {
            Break();
        }
    }

    private void Break()
    {
        intactStuff.SetActive(false);
        brokenStuff.SetActive(true);

        bc.enabled = false;

        if (audioSource && breakSound)
        {
            audioSource.PlayOneShot(breakSound);
        }

        StartCoroutine(DisappearAfterTime(3f));
    }

    private IEnumerator DisappearAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(brokenStuff);
    }
}
