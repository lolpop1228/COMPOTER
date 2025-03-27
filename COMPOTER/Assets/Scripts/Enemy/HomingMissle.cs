using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    public Transform target;
    public float speed = 5f; 
    public float rotateSpeed = 0.1f;
    public AudioClip explosionSound;
    public float damage = 20f;

    public GameObject impactEffect;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the explosionSound is assigned
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
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            // Rotate the missile using a smooth lerp between its current forward direction and the target direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            // Move the missile forward
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
            if (impactAudioSource != null && explosionSound != null)
            {
                impactAudioSource.PlayOneShot(explosionSound);
            }
            else
            {
                Debug.LogWarning("No AudioSource or explosionSound on impact effect.");
            }

            // Destroy the impact effect after some time
            Destroy(effect, 5f);
        }

        // Destroy the missile object after the collision
        Destroy(gameObject);
    }
}
