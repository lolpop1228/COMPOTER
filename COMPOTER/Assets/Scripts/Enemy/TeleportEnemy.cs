using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TeleportEnemy : MonoBehaviour
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
    public float health = 100f;

    //Teleport
    float teleportCooldown = 5f;
    float lastTeleportTime = -5f;

    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isReloading) return;

        bool playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if(playerInDetectRange && !playerInAttackRange && Time.time >= lastTeleportTime + teleportCooldown)
        {
            Teleport();
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

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Teleport()
    {
        lastTeleportTime = Time.time;
        transform.position = player.position - player.forward * 2f;
        transform.LookAt(player);
        AttackPlayer();
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
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        currentAmmo--;
        alreadyAttacked = true;
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
