using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBossSpawn : MonoBehaviour
{
    public Transform teleportPoint;
    public Transform player;
    public GameObject[] objectToEnable;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BossTeleport();
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
