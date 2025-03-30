using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public float health = 100f; // Health of the object
    public GameObject intactModel; // Model of the object in intact state
    public GameObject brokenModel; // Model of the object in broken state
    public GameObject explosionEffect; // Explosion effect to instantiate when destroyed
    public AudioSource audioSource;
    public AudioClip destructionSound; // Sound to play when destroyed
    public float explosionForce = 700f; // Force of the explosion
    public float explosionRadius = 5f; // Radius of the explosion
    public float upwardsModifier = 1f; // How much force is applied upwards during explosion

    private bool isDestroyed = false; // Prevent multiple destruction triggers

    // Method to apply damage to the object
    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;

        health -= damage;

        if (health <= 0f)
        {
            DestroyObject();
        }
    }

    // Method to destroy the object and trigger explosion
    private void DestroyObject()
    {
        isDestroyed = true;

        // Switch to broken model
        if (intactModel != null && brokenModel != null)
        {
            intactModel.SetActive(false); // Disable intact model
            brokenModel.SetActive(true);  // Enable broken model
            Destroy(brokenModel, 2f);
        }

        // Play explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(destructionSound);
        }

        // Apply explosion force to surrounding objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }
        }

        // Destroy the object itself after the explosion
        Destroy(gameObject);
    }

    // Gizmos for visualizing explosion radius in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
