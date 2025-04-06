using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TeleportEnemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint;
    public LayerMask groundLayer, playerLayer;
    public float patrolRange = 10f;
    public float detectRange = 15f;
    public float attackRange = 5f;
    public float timeBetweenAttacks = 1f;
    public GameObject projectile;
    public float bulletSpeed = 100f;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip patrolSound;
    public AudioClip attackSound;
    public AudioClip reloadSound;
    public AudioClip teleportSound;
    [Range(0, 1)] public float patrolSoundVolume = 0.5f;
    [Range(0, 1)] public float teleportSoundVolume = 1f;
    public AudioClip explosionSound;
    public float explosionVolume = 1f;

    [Header("Animation")]
    public Animator animator;
    public string patrolAnim;
    public string attackAnim;
    public string chaseAnim;
    public string reloadAnim;

    [Header("Teleport Settings")]
    public float teleportCooldown = 5f;
    public float teleportRadius = 3f;
    public float minTeleportDistance = 1.5f;
    public float teleportDelay = 0.5f;
    private float lastTeleportTime = -5f;

    // Combat System
    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;
    private int currentAmmo;
    public int maxAmmo = 10;
    public float reloadTime = 2f;
    private bool isReloading = false;
    private float lastPatrolSoundTime;
    public float patrolSoundInterval = 3f;

    // Health
    public float health = 100f;

    [Header("Drops")]
    public GameObject healthBox;
    public GameObject ammoBox;
    [Range(0f, 1f)] public float dropChance = 0.3f;
    [Header("Effects")]
    public GameObject explosionEffect; // Assign a particle system prefab


    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!firePoint) Debug.LogError("FirePoint is not assigned in TeleportEnemy.");
        
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isReloading) return;

        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (playerInDetectRange && !playerInAttackRange && Time.time >= lastTeleportTime + teleportCooldown)
        {
            StartCoroutine(Teleport());
        }
        else if (playerInDetectRange && playerInAttackRange)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private IEnumerator Teleport()
    {
        lastTeleportTime = Time.time;
        agent.isStopped = true;
        
        audioSource.PlayOneShot(teleportSound, teleportSoundVolume);
        
        yield return new WaitForSeconds(teleportDelay);
        
        Vector3 randomDirection = Random.insideUnitSphere * teleportRadius;
        randomDirection.y = 0;
        Vector3 teleportPosition = player.position + randomDirection.normalized * Mathf.Max(minTeleportDistance, randomDirection.magnitude);
        
        if (NavMesh.SamplePosition(teleportPosition, out NavMeshHit hit, teleportRadius, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            transform.LookAt(player.position);
        }
        
        agent.isStopped = false;
        AttackPlayer();
    }

    private void Patrol()
    {
        if (!patrolPointSet) SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
            animator.Play(patrolAnim);

            if (Time.time - lastPatrolSoundTime > patrolSoundInterval)
            {
                audioSource.PlayOneShot(patrolSound, patrolSoundVolume);
                lastPatrolSoundTime = Time.time;
            }

            if (Vector3.Distance(transform.position, patrolPoint) < 1f)
                patrolPointSet = false;
        }
    }

    private void SearchPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRange;
        patrolPoint = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        
        if (NavMesh.SamplePosition(patrolPoint, out NavMeshHit hit, patrolRange, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
            patrolPointSet = true;
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        animator.Play(attackAnim);
        transform.LookAt(player);

        if (!alreadyAttacked && currentAmmo > 0)
        {
            Shoot();
        }
        else if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (!firePoint) return;
        
        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);

        audioSource.PlayOneShot(attackSound);
        currentAmmo--;
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        animator.Play(reloadAnim);
        audioSource.PlayOneShot(reloadSound);
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
        health -= amount;
        if (health <= 0f) Die();
    }

    private void Die()
    {
        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);
        }

        // Spawn explosion particle effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Drop items before destroying
        if (Random.value <= dropChance) DropItem(healthBox);
        if (Random.value <= dropChance) DropItem(ammoBox);

        Destroy(gameObject);
    }

    private void DropItem(GameObject item)
    {
        if (item != null)
        {
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            Instantiate(item, dropPosition, Quaternion.identity);
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
        
        if (player != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(player.position, teleportRadius);
        }
    }
}