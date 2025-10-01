using UnityEngine;

public class TeleportBossSpawn : MonoBehaviour
{
    public Transform teleportPoint;
    public Transform player;
    public GameObject[] objectToEnable;

    private CharacterController playerController;

    void Start()
    {
        if (player != null)
            playerController = player.GetComponent<CharacterController>();

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossTeleport();
        }
    }

    public void BossTeleport()
    {
        if (player != null && teleportPoint != null)
        {
            if (playerController != null)
            {
                // Disable CharacterController before teleport
                playerController.enabled = false;
                player.position = teleportPoint.position;
                playerController.enabled = true;
            }
            else
            {
                // If no CharacterController, just move normally
                player.position = teleportPoint.position;
            }

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