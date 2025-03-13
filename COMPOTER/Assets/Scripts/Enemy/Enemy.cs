using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;          // NavMeshAgent for movement
    public Transform player;           // Reference to the player
    public LayerMask groundLayer, playerLayer; // LayerMasks for ground and player
    public float patrolRange = 10f;    // Range for random patrol points
    public float detectRange = 15f;    // Range to detect the player
    public float attackRange = 5f;     // Range to attack the player
    public float timeBetweenAttacks = 1f; // Time between attacks
    public GameObject projectile;      // Projectile to attack the player
    public float bulletSpeed = 100f;

    private Vector3 patrolPoint;       // Current patrol point
    private bool patrolPointSet;       // Flag to check if patrol point is set
    private bool alreadyAttacked;      // Flag to check if the enemy already attacked

    public Animator animator;
    public string patrolAnim;
    public string attackAnim;
    public string chaseAnim;

    //Health
    public float health = 100f;

    private void Start()
    {
        // Ensure the NavMeshAgent is assigned
        if (!agent) agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check for player within detection and attack ranges
        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInDetectRange && !playerInAttackRange) Patrol();
        if (playerInDetectRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void Patrol()
    {
        if (!patrolPointSet) SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
            animator.Play(patrolAnim);

            // Check if the enemy reached the patrol point
            if (Vector3.Distance(transform.position, patrolPoint) < 1f)
            {
                patrolPointSet = false;
            }
        }
    }

    private void SearchPatrolPoint()
    {
        // Generate a random patrol point within the patrol range
        float randomZ = Random.Range(-patrolRange, patrolRange);
        float randomX = Random.Range(-patrolRange, patrolRange);

        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check if the point is on the ground
        if (Physics.Raycast(patrolPoint, -Vector3.up, 2f, groundLayer))
        {
            patrolPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        // Set the destination to the player's position
        agent.SetDestination(player.position);
        animator.Play(chaseAnim);
    }

    private void AttackPlayer()
    {
        // Stop moving
        agent.SetDestination(transform.position);
        animator.Play(attackAnim);

        // Face the player
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack logic (e.g., shoot a projectile)
            Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the patrol range in blue
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);

        // Draw the detect range in yellow
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Draw the attack range in red
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
