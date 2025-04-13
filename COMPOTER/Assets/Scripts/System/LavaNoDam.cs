using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaNoDam : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Teleport();
        }
    }

    void Teleport()
    {
        if (player != null && spawnPoint != null)
        {
            player.position = spawnPoint.position;
        }
    }
}
