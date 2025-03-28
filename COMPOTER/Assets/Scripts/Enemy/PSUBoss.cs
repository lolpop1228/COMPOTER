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
    public float timeBetweenAttacks = 1f;
    public GameObject projectile;
    public float bulletSpeed = 100f;

    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;

    public Animator animator;
    public string patrolAnim;
    public string attackAnim;
    public string chaseAnim;
    public string reloadAnim;

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

    //Drop
    public GameObject healthBox;
    public GameObject ammoBox;

    // Audio
    private AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip patrolSound;
    public AudioClip chaseSound;

    private string currentState = "";

    public GameObject Trap;

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

        Trap.SetActive(false);
    }

    private void Update()
    {
        if (isReloading) return;

        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInDetectRange && !playerInAttackRange)
            Patrol();
        else if (playerInDetectRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange)
            AttackPlayer();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (bossHealthBar != null)
        {
            bossHealthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= maxHealth * 0.5f)
        {
            Trap.SetActive(true);
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
        if (firePoint == null) return; // Prevents errors if firePoint is not assigned

        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();

        currentAmmo--;
        alreadyAttacked = true;
        
        // Play the fire sound here
        audioSource.PlayOneShot(fireSound);

        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        animator.Play(reloadAnim);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
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

        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
        }
    }
}
