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

    [Header("Visual Settings")]
    public float spinSpeed = 90f;        // Degrees per second
    public float bobAmplitude = 0.25f;   // Up-down floating height
    public float bobFrequency = 2f;      // Speed of floating motion

    private PlayerHealth playerHealth;
    private Transform player;
    private Vector3 startPos;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        // --- Spin animation ---
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);

        // --- Floating bob animation ---
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

        // --- Auto-pickup when close ---
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectHealth();
        }
    }
}
