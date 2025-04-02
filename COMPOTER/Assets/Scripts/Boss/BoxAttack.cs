using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAttack : MonoBehaviour
{
    public float delay = 1.5f;
    public Vector3 boxSize = new Vector3(5f, 2f, 5f); // Width, height, depth of the explosion area
    public float force = 700f;
    public GameObject explosionEffect;
    public float damage = 50f; 
    public AudioClip explosionSound;

    private float countdown;
    private bool hasExploded = false;

    private AudioSource audioSource;

    void Start()
    {
        countdown = delay;
        audioSource = GetComponent<AudioSource>(); 
    }

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
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Play the explosion sound
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Find all nearby colliders within the box area
        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply explosion force to nearby objects with Rigidbody
                rb.AddExplosionForce(force, transform.position, boxSize.magnitude);
            }

            // Check if the nearby object is the player and apply damage
            if (nearbyObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.PlayerTakeDamage(damage);
                }
            }
        }

        // Destroy the ThunderStrike object after it explodes
        Destroy(gameObject, 1f);
    }

    // Draw the rectangular explosion area in the Unity Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set gizmo color
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize); // Draw explosion box
    }
}
