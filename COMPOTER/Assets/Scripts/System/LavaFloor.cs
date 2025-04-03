using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    public Transform player;
    public PlayerHealth playerHealth;
    public Transform spawnPoint;
    public int damageAmount = 20; // Adjust damage as needed

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.PlayerTakeDamage(damageAmount);
            }

            Teleport();
        }
    }

    public void Teleport()
    {
        if (player != null && spawnPoint != null)
        {
            player.position = spawnPoint.position;
        }
    }
}
