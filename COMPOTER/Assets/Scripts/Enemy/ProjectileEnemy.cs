using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Security.Cryptography;

public class ProjectileEnemy : MonoBehaviour
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

    // Bullet Spawn Point
    public Transform firePoint;

    //Drop
    public GameObject healthBox;
    public GameObject ammoBox;

    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned in " + gameObject.name);
        }
    }

    private void Update()
    {
        if (isReloading) return;

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

    void DropItem(GameObject item)
    {
        if (item != null)
        {
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            GameObject droppedItem = Instantiate(item, dropPosition, Quaternion.identity);
            Destroy(droppedItem, 5f);
        }
    }

    void Die()
    {
        DropItem(healthBox);
        DropItem(ammoBox);
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
        if (firePoint == null) return; // Prevents errors if firePoint is not assigned

        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);

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

        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
        }
    }
}
