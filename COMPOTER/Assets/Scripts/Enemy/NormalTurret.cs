using System.Collections;
using UnityEngine;

public class NormalTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    public float detectionRange = 15f;
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    private float nextFireTime;

    [Header("References")]
    public Transform target;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    private AudioSource audioSource;
    public AudioClip fireSound;

    [Header("Obstacle Detection")]
    public LayerMask obstacleMask; // Set this in inspector to include walls/obstacles
    public float lineOfSightOffset = 0.5f; // Raise the raycast origin to avoid ground collisions

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        FindTarget();

        if (target != null && HasLineOfSight())
        {
            RotateTowardsTarget();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                target = player.transform;
            }
            else
            {
                target = null;
            }
        }
    }

    bool HasLineOfSight()
    {
        if (target == null) return false;

        Vector3 rayOrigin = firePoint.position + Vector3.up * lineOfSightOffset;
        Vector3 direction = (target.position + Vector3.up * lineOfSightOffset) - rayOrigin;
        float distance = Vector3.Distance(rayOrigin, target.position);

        // Debug draw the ray
        Debug.DrawRay(rayOrigin, direction.normalized * distance, Color.red);

        // Check if there's an obstacle between turret and target
        if (!Physics.Raycast(rayOrigin, direction.normalized, distance, obstacleMask))
        {
            return true;
        }

        return false;
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }

            if (audioSource != null)
            {
                audioSource.PlayOneShot(fireSound);
            }
        }
    }

    // Visualize detection range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}