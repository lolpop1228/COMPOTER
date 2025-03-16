using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 15; // Amount of ammo to add
    private GunController rifle; // Reference to the gun

    private void Update()
    {
        rifle = FindObjectOfType<GunController>(); // Find the gun in the scene
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the object has the Player tag
        {
            if (rifle != null)
            {
                rifle.AddReserveAmmo(ammoAmount); // Apply ammo change directly to GunController
            }
            Destroy(gameObject); // Destroy the pickup after collecting
        }
    }
}
