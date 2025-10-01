using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ProjectileEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public NavMeshAgent agent;
    public float patrolRange = 10f;
    public float detectRange = 15f;
    public float attackRange = 5f;
    public float chaseSpeed = 6f;
    public float patrolSpeed = 3f;

    [Header("Attack Settings")]
    public float timeBetweenAttacks = 1f;
    public GameObject projectile;
    public float bulletSpeed = 100f;
    public int maxAmmo = 10;
    public float reloadTime = 2f;
    public float aimOffset = 0.5f; // Vertical offset for shooting

    [Header("References")]
    public Transform player;
    public Transform firePoint;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask obstacleMask;

    [Header("Visual Effects")]
    public Animator animator;
    public string patrolAnim = "Patrol";
    public string attackAnim = "Attack";
    public string chaseAnim = "Chase";
    public string reloadAnim = "Reload";

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip patrolSound;
    public AudioClip chaseSound;
    public AudioClip reloadSound;

    [Header("Drops")]
    public GameObject healthBox;
    public GameObject ammoBox;
    [Range(0f, 1f)] public float dropChance = 0.3f;

    [Header("Explosion Settings")]
    public GameObject explosionEffect;
    public AudioClip explosionSound;


    // Private variables
    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;
    private int currentAmmo;
    private bool isReloading;
    private string currentState;
    private float lastLOSCheckTime;
    private bool hasLineOfSight;
    private const float LOSCheckInterval = 0.2f;

    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        currentAmmo = maxAmmo;
        if (!audioSource) audioSource = GetComponent<AudioSource>();

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned in " + gameObject.name);
            firePoint = transform;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj) player = playerObj.transform;
        }

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isReloading) return;

        // Optimized LOS checking with interval
        if (Time.time - lastLOSCheckTime > LOSCheckInterval)
        {
            hasLineOfSight = HasLineOfSight();
            lastLOSCheckTime = Time.time;
        }

        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInDetectRange && !playerInAttackRange)
        {
            Patrol();
        }
        else if (playerInDetectRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInAttackRange && hasLineOfSight)
        {
            AttackPlayer();
        }
    }

    private bool HasLineOfSight()
    {
        if (player == null) return false;

        Vector3 rayOrigin = transform.position + Vector3.up * aimOffset;
        Vector3 targetPosition = player.position + Vector3.up * aimOffset;
        Vector3 direction = (targetPosition - rayOrigin).normalized;
        float distance = Vector3.Distance(rayOrigin, targetPosition);

        Debug.DrawRay(rayOrigin, direction * distance, Color.green, LOSCheckInterval);

        if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, distance, obstacleMask))
        {
            return false; // Obstacle in the way
        }
        return true;
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;
        if (!patrolPointSet) SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
            ChangeState(patrolAnim, patrolSound);

            if (Vector3.Distance(transform.position, patrolPoint) < 1f)
                patrolPointSet = false;
        }
    }

    private void SearchPatrolPoint()
    {
        float randomZ = Random.Range(-patrolRange, patrolRange);
        float randomX = Random.Range(-patrolRange, patrolRange);
        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(patrolPoint, -Vector3.up, 2f, groundLayer))
            patrolPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        ChangeState(chaseAnim, chaseSound);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        ChangeState(attackAnim, null);
        FaceTarget();

        if (!alreadyAttacked && currentAmmo > 0)
        {
            Shoot();
        }
        else if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void FaceTarget()
    {
        if (player == null) return;
        
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void Shoot()
    {
        if (firePoint == null || !hasLineOfSight) return;

        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            Vector3 aimDirection = (player.position + Vector3.up * aimOffset - firePoint.position).normalized;
            rb.AddForce(aimDirection * bulletSpeed, ForceMode.Impulse);
        }

        currentAmmo--;
        alreadyAttacked = true;
        
        audioSource.PlayOneShot(fireSound);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        ChangeState(reloadAnim, reloadSound);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f) 
        {
            Die();
        }
    }

    private void Die()
    {
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Drop items
        if (Random.value <= dropChance) DropItem(healthBox);
        if (Random.value <= dropChance) DropItem(ammoBox);

        // Play explosion sound from a temporary object
        if (explosionSound != null)
        {
            GameObject tempAudio = new GameObject("TempExplosionSound");
            tempAudio.transform.position = transform.position;

            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = explosionSound;
            tempSource.Play();

            Destroy(tempAudio, explosionSound.length);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
    
    private void DropItem(GameObject item)
    {
        if (item != null)
        {
            // Random offset around enemy
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));

            // Raycast downward to find ground
            if (Physics.Raycast(dropPosition, Vector3.down, out RaycastHit hit, 10f, groundLayer))
            {
                dropPosition = hit.point; // Place on ground
            }
            else
            {
                dropPosition.y = transform.position.y; // Fallback
            }

            Instantiate(item, dropPosition, Quaternion.identity);
        }
    }

    private void ChangeState(string newState, AudioClip sound)
    {
        if (currentState != newState)
        {
            currentState = newState;
            animator.Play(newState);
            PlaySound(sound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}