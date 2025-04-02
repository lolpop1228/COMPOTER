using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    public float delay = 1.5f;
    public float radius = 5f;
    public float force = 700f;
    public GameObject explosionEffect;
    public float damage = 50f; // Amount of damage to apply
    public AudioClip explosionSound; // Reference to the explosion sound

    private float countdown;
    private bool hasExploded = false;

    private PlayerHealth playerHealth; // Reference to PlayerHealth script
    private AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;

        // Find PlayerHealth script in the scene dynamically
        playerHealth = FindObjectOfType<PlayerHealth>(); 

        // Get the AudioSource component on the same GameObject
        audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Thunder();
            hasExploded = true;
        }
    }

    void Thunder()
    {
        // Instantiate explosion effect at the thunder strike's position
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

        // Play the explosion sound
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound); // Play explosion sound once
        }

        // Find all nearby colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply explosion force to nearby objects with Rigidbody
                rb.AddExplosionForce(force, transform.position, radius);
            }

            // Check if the nearby object is the player and apply damage
            if (nearbyObject.CompareTag("Player") && playerHealth != null)
            {
                // Apply damage to the player only once
                playerHealth.PlayerTakeDamage(damage);
            }
        }

        // Destroy the ThunderStrike object after it explodes
        Destroy(gameObject, 1f);

        // Destroy the explosion effect after it finishes (you can adjust the lifetime as needed)
        Destroy(explosion, 2f); // Assuming the particle effect lasts 2 seconds
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // Set color of the gizmo
        Gizmos.DrawWireSphere(transform.position, radius); // Draw the explosion radius
    }
}
