using UnityEngine;

public class PlayerDeadInBossRoom : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject objectToDisable;

    private void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            objectToDisable.SetActive(false);
        }
    }
}
