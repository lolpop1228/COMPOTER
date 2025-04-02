using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUBoss : MonoBehaviour
{
    public float detectionRange = 25f;
    public LayerMask playerLayer;
    private Transform player;
    private bool playerDetected;
    
    // Health
    public float maxHealth = 6000;
    public float currentHealth;
    public BossHealthBar bossHealthBar;

    // Child Object Monitoring
    public GameObject turretHolder;

    // Attacks
    public MonoBehaviour[] attackScripts;
    public float attackDuration = 10f;
    private MonoBehaviour currentAttack;
    private bool isAttacking = false; // Prevent multiple activations

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        DetectPlayer();
        CheckChildObject();
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
    }

    void OnPlayerLost()
    {
        Debug.Log("Player out of range. Boss disengaging.");
    }

    void CheckChildObject()
    {
        if (turretHolder != null && turretHolder.transform.childCount <= 0 && !isAttacking)
        {
            BossActivate();
        }
    }

    void BossActivate()
    {
        if (attackScripts.Length == 0) return;
        
        isAttacking = true; // Ensure the coroutine runs only once
        StartCoroutine(ActivateRandomAttacks());
    }

    IEnumerator ActivateRandomAttacks()
    {
        while (true)
        {
            if (attackScripts.Length == 0) yield break;

            if (currentAttack != null)
            {
                currentAttack.enabled = false;
            }

            currentAttack = attackScripts[Random.Range(0, attackScripts.Length)];
            currentAttack.enabled = true;

            Debug.Log($"Boss activated: {currentAttack.GetType().Name}");

            yield return new WaitForSeconds(attackDuration);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
