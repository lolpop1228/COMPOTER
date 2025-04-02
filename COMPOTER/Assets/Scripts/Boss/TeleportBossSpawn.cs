using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBossSpawn : MonoBehaviour, IInteractable
{
    public Transform teleportPoint;
    public Transform player;
    public void Interact()
    {
        BossTeleport();
    }

    public void BossTeleport()
    {
        if (player != null && teleportPoint != null)
        {
            player.position = teleportPoint.position;
        }
    }
}
