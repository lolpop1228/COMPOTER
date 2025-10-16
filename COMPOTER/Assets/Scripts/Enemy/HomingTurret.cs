using System.Collections;
using UnityEngine;

public class HomingTurret : MonoBehaviour
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

    [Header("Audios")]
    private AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Health")]
    public float maxHealth = 1000f;
    public float currentHealth;
    public BossHealthBar healthBar;
    public GameObject turretHealthBar; // ✅ reference to the UI object

    [Header("Explosion Settings")]
    public Transform explosionPoint;
    public GameObject explosionEffect;
    public AudioClip explosionSound;

    [Header("Particle")]
    public GameObject particleEffect;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        particleEffect.SetActive(false);

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    private void Update()
    {
        FindTarget();

        if (target != null)
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
            target = distance <= detectionRange ? player.transform : null;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= maxHealth * 0.5f)
        {
            if (particleEffect != null)
                particleEffect.SetActive(true);
        }

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        // ✅ Disable turret health bar before destruction
        if (turretHealthBar != null)
            turretHealthBar.SetActive(false);

        Vector3 spawnPosition = explosionPoint != null ? explosionPoint.position : transform.position;

        if (explosionEffect != null)
            Instantiate(explosionEffect, spawnPosition, Quaternion.identity);

        if (explosionSound != null)
        {
            GameObject tempAudio = new GameObject("TempExplosionSound");
            tempAudio.transform.position = spawnPosition;

            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = explosionSound;
            tempSource.Play();
            Destroy(tempAudio, explosionSound.length);
        }

        Destroy(gameObject);
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
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        if (audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
