using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    public float patrolRange = 10f;
    public float detectRange = 15f;
    public float attackRange = 2f;
    public float timeBetweenAttacks = 1f;
    public float attackDamage = 20f;

    private Vector3 patrolPoint;
    private bool patrolPointSet;
    private bool alreadyAttacked;

    public Animator animator;
    public string patrolAnim;
    public string attackAnim;
    public string chaseAnim;

    // Health System
    public float health = 100f;

    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInDetectRange && !playerInAttackRange) Patrol();
        if (playerInDetectRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
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
        animator.Play(chaseAnim);
    }

    private void AttackPlayer()
    {
        // Stop moving
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Play attack animation **every time the enemy attacks**
            animator.Play(attackAnim);

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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
