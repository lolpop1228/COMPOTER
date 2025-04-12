using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBossSpawn : MonoBehaviour, IInteractable
{
    public Transform teleportPoint;
    public Transform player;
    public GameObject[] objectToEnable;

    public void Interact()
    {
        BossTeleport();
    }

    void Start()
    {
        foreach (GameObject obj in objectToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void BossTeleport()
    {
        if (player != null && teleportPoint != null)
        {
            player.position = teleportPoint.position;

            foreach (GameObject obj in objectToEnable)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
