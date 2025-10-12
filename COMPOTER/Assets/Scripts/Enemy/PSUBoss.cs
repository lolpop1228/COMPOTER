using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PSUBoss : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    public float patrolRange = 10f;
    public float detectRange = 15f;
    public float attackRange = 5f;
    public float meleeRange = 2f;
    public float timeBetweenAttacks = 1f;
    public float meleeDamage = 20f;
    public float meleeCooldown = 2f;
    public GameObject projectile;
    public float bulletSpeed = 100f;

    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;
    private bool canMelee = true;

    public Animator animator;
    public string patrolAnim;
    public string attackAnim;
    public string chaseAnim;
    public string reloadAnim;
    public string meleeAnim;

    // Ammo System
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    // Health System
    public float maxHealth = 100f;
    private float currentHealth;
    public BossHealthBar bossHealthBar;

    // Bullet Spawn Point
    public Transform firePoint;

    // Drop
    public GameObject healthBox;
    public GameObject ammoBox;

    // Audio
    private AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip patrolSound;
    public AudioClip chaseSound;
    public AudioClip meleeSound;

    private string currentState = "";
    public GameObject BossHpBar;

    // Rapid Spawn Prefab
    public GameObject spawnPrefab;
    public float spawnInterval = 0.1f;
    private bool isSpawning = false;
    public Transform spawnTransform;

    // Knockback
    public float knockbackForce = 10f;
    public float knockbackUpwardForce = 5f;
    public float knockbackDuration = 0.5f;

    [Header("Explosion")]
    public GameObject explosionEffect;
    public Transform explosionPoint;
    public AudioClip explosionSound;

    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned in " + gameObject.name);
        }

        currentHealth = maxHealth;
        if (bossHealthBar != null)
        {
            bossHealthBar.SetMaxHealth(maxHealth);
        }
    }

    void OnEnable()
    {
        if (spawnTransform != null)
        {
            transform.position = spawnTransform.position;
            currentHealth = maxHealth;
        }
    }

    private void Update()
    {
        if (isReloading) return;

        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        bool playerInMeleeRange = Physics.CheckSphere(transform.position, meleeRange, playerLayer);

        if (!playerInDetectRange && !playerInAttackRange)
        {
            Patrol();
            ChangeState(patrolAnim, patrolSound);
        }
        else if (playerInDetectRange && !playerInAttackRange)
        {
            ChasePlayer();
            ChangeState(chaseAnim, chaseSound);
        }
        else if (playerInAttackRange && !playerInMeleeRange)
        {
            AttackPlayer();
        }
        else if (playerInMeleeRange)
        {
            MeleeAttack();
        }

        if (currentHealth < maxHealth / 2 && !isSpawning)
        {
            StartCoroutine(SpawnPrefabAtPlayer());
        }
    }

    private void MeleeAttack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (canMelee)
        {
            ChangeState(meleeAnim, meleeSound);
            canMelee = false;
            Invoke(nameof(ResetMelee), meleeCooldown);
            StartCoroutine(DelayedKnockback(0.2f));
        }
    }

    private IEnumerator DelayedKnockback(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (Physics.CheckSphere(transform.position, meleeRange, playerLayer))
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.PlayerTakeDamage(meleeDamage);
                ApplyKnockback();
            }
        }
    }

    private void ApplyKnockback()
    {
        Vector3 knockbackDirection = (player.position - transform.position).normalized;
        knockbackDirection.y = 0.2f;

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            StartCoroutine(HandleKnockbackCharacterController(controller, knockbackDirection));
        }
        else
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                StartCoroutine(HandleKnockbackRigidbody(playerRb));
            }
        }
    }

    private IEnumerator HandleKnockbackCharacterController(CharacterController controller, Vector3 direction)
    {
        PlayerMovement movement = controller.GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            controller.Move(direction * knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (movement != null) movement.enabled = true;
    }

    private IEnumerator HandleKnockbackRigidbody(Rigidbody playerRb)
    {
        yield return new WaitForSeconds(knockbackDuration);
        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
        }
    }

    private void ResetMelee()
    {
        canMelee = true;
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

    private void Patrol()
    {
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
        agent.SetDestination(player.position);
        ChangeState(chaseAnim, chaseSound);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(player.position);
        ChangeState(attackAnim, null);
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
        if (firePoint == null) return;

        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        currentAmmo--;
        alreadyAttacked = true;

        if (fireSound != null)
            audioSource.PlayOneShot(fireSound);

        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        ChangeState(reloadAnim, null);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;

        if (Physics.CheckSphere(transform.position, attackRange, playerLayer))
        {
            ChangeState(attackAnim, null);
        }
        else
        {
            ChangeState(chaseAnim, chaseSound);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Die()
    {
        DropItem(healthBox);
        DropItem(ammoBox);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (explosionEffect != null && explosionPoint != null)
        {
            Instantiate(explosionEffect, explosionPoint.position, Quaternion.identity);
        }

        if (explosionSound != null && explosionPoint != null)
        {
            GameObject soundObj = new GameObject("ExplosionSound");
            soundObj.transform.position = explosionPoint.position;

            AudioSource tempSource = soundObj.AddComponent<AudioSource>();
            tempSource.clip = explosionSound;
            tempSource.Play();

            Destroy(soundObj, explosionSound.length);
        }
    }

    private void DropItem(GameObject item)
    {
        if (item != null)
        {
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            GameObject droppedItem = Instantiate(item, dropPosition, Quaternion.identity);
            Destroy(droppedItem, 5f);
        }
    }

    private void ChangeState(string newState, AudioClip sound)
    {
        if (currentState != newState && !string.IsNullOrEmpty(newState))
        {
            currentState = newState;
            animator.Play(newState);

            if (sound != null && audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(sound);
            }
        }
    }

    private IEnumerator SpawnPrefabAtPlayer()
    {
        isSpawning = true;

        while (currentHealth < maxHealth / 2)
        {
            Instantiate(spawnPrefab, player.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
        }
    }
}
