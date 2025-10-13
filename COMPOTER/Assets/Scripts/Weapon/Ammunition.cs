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

    [Header("Visual Settings")]
    public float spinSpeed = 90f;        // Degrees per second
    public float bobAmplitude = 0.25f;   // Optional: floating up-down motion
    public float bobFrequency = 2f;

    private GunController rifle;
    private Transform player;
    private Vector3 startPos;

    private void Start()
    {
        rifle = FindObjectOfType<GunController>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        startPos = transform.position;
    }

    private void Update()
    {
        if (player == null) return;

        // --- Spin effect ---
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);

        // --- Floating bob effect (optional) ---
        float newY = startPos.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // --- Attraction toward player ---
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attractionRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                attractionSpeed * Time.deltaTime
            );
        }

        // --- Auto-pickup ---
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectAmmo();
        }
    }
}
