using UnityEngine;

public class HealthBox : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthAmount = 20f;
    public AudioClip healingSound;
    public float soundVolume = 1f;

    [Header("Pickup Attraction Settings")]
    public float attractionRange = 5f;   // Distance at which pickup starts moving toward player
    public float attractionSpeed = 8f;   // Speed of movement toward player
    public float pickupDistance = 1f;    // Distance to auto-pickup if close

    private PlayerHealth playerHealth;
    private Transform player;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
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
            CollectHealth();
        }
    }

    private void CollectHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.HealPlayer(healthAmount);
        }

        if (healingSound != null)
        {
            AudioSource.PlayClipAtPoint(healingSound, transform.position, soundVolume);
        }

        Destroy(gameObject);
    }

    // Optional: also allow pickup via trigger (if using colliders set as trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectHealth();
        }
    }
}
