using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
