using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 0.1f;
    public AudioClip explosionSound;
    public float damage = 20f;
    public GameObject impactEffect;

    private Transform target;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Find player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }

        if (explosionSound == null)
        {
            Debug.LogError("Explosion sound not assigned.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Rotate missile towards the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Smoothly rotate towards the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            // Move forward
            rb.velocity = transform.forward * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.PlayerTakeDamage(damage);
        }

        if (impactEffect != null)
        {
            // Instantiate impact effect
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);

            // Play the explosion sound from the impact effect
            AudioSource impactAudioSource = effect.GetComponent<AudioSource>();
            if (impactAudioSource == null)
            {
                impactAudioSource = effect.AddComponent<AudioSource>();  // Add AudioSource if missing
            }

            if (explosionSound != null)
            {
                impactAudioSource.spatialBlend = 1.0f;  // 3D sound
                impactAudioSource.volume = 1.0f;        // Max volume
                impactAudioSource.PlayOneShot(explosionSound);
            }
            else
            {
                Debug.LogWarning("No explosion sound assigned.");
            }

            Destroy(effect, 5f);
        }

        Destroy(gameObject);
    }
}
