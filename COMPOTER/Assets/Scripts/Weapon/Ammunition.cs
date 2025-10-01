using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int ammoAmount = 15; // Amount of ammo to add
    public AudioClip pickupSound;
    public float soundVolume = 1f;

    [Header("Pickup Attraction Settings")]
    public float attractionRange = 5f;   // Distance at which pickup starts moving toward player
    public float attractionSpeed = 8f;   // Speed of movement toward player
    public float pickupDistance = 1f;    // Distance to auto-pickup if close

    private GunController rifle;
    private Transform player;

    private void Start()
    {
        rifle = FindObjectOfType<GunController>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // If within attraction range, move toward player
        if (distance <= attractionRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                attractionSpeed * Time.deltaTime
            );
        }

        // Auto-pickup when close enough
        if (distance <= pickupDistance)
        {
            CollectAmmo();
        }
    }

    private void CollectAmmo()
    {
        if (rifle != null)
        {
            rifle.AddReserveAmmo(ammoAmount);
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
        }

        Destroy(gameObject);
    }

    // Optional: keep trigger support for walking over it
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectAmmo();
        }
    }
}
