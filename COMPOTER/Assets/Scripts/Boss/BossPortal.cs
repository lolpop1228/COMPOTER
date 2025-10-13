using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public Transform teleportTarget;
    public CharacterController playerController;

    void TeleportPlayer()
    {
        playerController.enabled = false;

        playerController.transform.position = teleportTarget.position;
        playerController.transform.rotation = teleportTarget.rotation;

        playerController.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerController != null)
            {
                TeleportPlayer();
            }
        }
    }
}
