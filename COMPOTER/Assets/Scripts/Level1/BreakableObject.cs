using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public AudioClip breakSound;
    public float breakForce = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BreakObject();
        }
    }

    private void BreakObject()
    {
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }

        Destroy(gameObject);
    }
}
