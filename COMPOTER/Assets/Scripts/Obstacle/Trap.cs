using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damagePerSecs = 5f;
    public float damageInterval = 1f; // Time in seconds between each damage tick
    private bool playerInZone = false;
    private PlayerHealth playerHealth;

    void OnCollisionEnter(Collision collision)
    {
        playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerInZone = true;
            StartCoroutine(ApplyDamageOverTime());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    IEnumerator ApplyDamageOverTime()
    {
        while (playerInZone && playerHealth != null)
        {
            playerHealth.PlayerTakeDamage(damagePerSecs);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
