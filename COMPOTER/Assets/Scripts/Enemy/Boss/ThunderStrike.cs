using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    public float delay = 1.5f;
    public float radius = 5f;
    public float force = 700f;
    public GameObject explosionEffect;
    public float damage = 50f;
    public AudioClip explosionSound;

    private float countdown;
    private bool hasExploded = false;
    private PlayerHealth playerHealth;

    void Start()
    {
        countdown = delay;
        playerHealth = FindObjectOfType<PlayerHealth>(); 
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
        // Instantiate explosion effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

        // Play explosion sound without cutting it off
        PlayExplosionSound();

        // Apply explosion force and damage
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            if (nearbyObject.CompareTag("Player") && playerHealth != null)
            {
                playerHealth.PlayerTakeDamage(damage);
            }
        }

        // Destroy the ThunderStrike object after explosion
        Destroy(gameObject);
        Destroy(explosion, 2f);
    }

    void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            GameObject soundObject = new GameObject("ExplosionSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.Play();

            // Destroy the sound object after the clip finishes
            Destroy(soundObject, explosionSound.length);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
