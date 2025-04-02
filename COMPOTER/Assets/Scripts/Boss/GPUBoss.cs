using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUBoss : MonoBehaviour
{
    public float detectionRange = 25f;
    public LayerMask playerLayer;
    private Transform player;
    bool playerDetected;
    //Health
    public float maxHealth = 6000;
    public float currentHealth;
    public BossHealthBar bossHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                OnPlayerDetected();
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;
                OnPlayerLost();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (bossHealthBar != null)
        {
            bossHealthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0f) Die();
    }
    void Die()
    {
        Destroy(gameObject);
    }

    void OnPlayerDetected()
    {
        Debug.Log("Player detected! Boss is engaging.");
        // Implement attack or chase behavior here
    }

    void OnPlayerLost()
    {
        Debug.Log("Player out of range. Boss disengaging.");
        // Implement disengage or idle behavior here
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
