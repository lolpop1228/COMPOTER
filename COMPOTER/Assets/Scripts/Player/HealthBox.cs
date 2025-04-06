using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public float healthAmount = 20;
    public AudioClip healingSound; // Add this in the Inspector
    public float soundVolume = 1f;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.HealPlayer(healthAmount);
            }

            // Play healing sound at this position, allow it to finish
            if (healingSound != null)
            {
                AudioSource.PlayClipAtPoint(healingSound, transform.position, soundVolume);
            }

            Destroy(gameObject);
        }
    }
}
