using System.Collections;
using UnityEngine;
using System.Linq;

public class GPUBoss : MonoBehaviour
{
    [Header("Core Settings")]
    public float detectionRange = 25f;
    public LayerMask playerLayer;
    public float maxHealth = 6000f;
    public BossHealthBar bossHealthBar;
    public GameObject turretHolder;
    private Animator animator;

    [Header("Attack Settings")]
    public float attackDuration = 10f;
    private float currentHealth;
    private Transform player;
    private Coroutine currentAttackCoroutine;
    private bool isAttacking = false;

    [Header("Main Attack Settings")]
    public GameObject[] mainAttackPrefabs;
    public Transform[] mainAttackPoints;
    public float mainSpawnInterval = 3f;
    private Coroutine mainAttackRoutine;

    [Header("Left Attack Settings")]
    public GameObject[] leftAttackPrefabs;
    public Transform[] leftAttackPoints;
    public float leftSpawnDelay = 1f;
    private Coroutine leftAttackRoutine;

    [Header("Right Attack Settings")]
    public GameObject[] rightAttackPrefabs;
    public Transform[] rightAttackPoints;
    public float rightSpawnDelay = 1f;
    private Coroutine rightAttackRoutine;

    [Header("Big Attack Settings")]
    public GameObject bigAttackPrefab;
    public Transform bigAttackPoint;
    public GameObject bigAttackPlatforms;

        [Header("Item Drop Settings")]
    public GameObject[] healthBoxPrefabs;  // Multiple health box prefabs
    public GameObject[] ammoBoxPrefabs;    // Multiple ammo box prefabs
    public Transform[] itemSpawnPoints;    // Multiple spawn points
    public float minSpawnInterval = 15f;
    public float maxSpawnInterval = 30f;
    private Coroutine itemSpawnRoutine;

    [Header("Attack Timing Settings")]
    public float attackCooldown = 5f; // Delay between each attack

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        PlayIdleAnimation();
        itemSpawnRoutine = StartCoroutine(ItemSpawnRoutine()); // Start item spawn loop
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        if (turretHolder.transform.childCount <= 0 && 
            Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            StartAttackSequence();
        }
    }

    void StartAttackSequence()
    {
        if (currentAttackCoroutine != null)
            StopCoroutine(currentAttackCoroutine);

        currentAttackCoroutine = StartCoroutine(AttackSequence());
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        while (true)
        {
            StopAllAttackRoutines();
            int attackIndex = WeightedAttackSelection();

            switch (attackIndex)
            {
                case 0:
                    StartMainAttack();
                    break;
                case 1:
                    StartLeftAttack();
                    break;
                case 2:
                    StartRightAttack();
                    break;
                case 3:
                    StartBigAttack();
                    break;
            }

            yield return new WaitForSeconds(attackDuration); // Wait for attack duration
            StopAllAttackRoutines(); // Ensure attack stops before cooldown

            yield return new WaitForSeconds(attackCooldown); // Wait before the next attack

            if (Vector3.Distance(transform.position, player.position) > detectionRange)
            {
                isAttacking = false;
                PlayIdleAnimation();
                yield break;
            }
        }
    }

    int WeightedAttackSelection()
    {
        int[] weights = { 4, 4, 4, 4 };
        int totalWeight = 4 + 4 + 4 + 4;
        int randomValue = Random.Range(0, totalWeight);

        if (randomValue < weights[0]) return 0;
        if (randomValue < weights[0] + weights[1]) return 1;
        if (randomValue < weights[0] + weights[1] + weights[2]) return 2;
        return 3;
    }

    void StopAllAttackRoutines()
    {
        if (mainAttackRoutine != null) StopCoroutine(mainAttackRoutine);
        if (leftAttackRoutine != null) StopCoroutine(leftAttackRoutine);
        if (rightAttackRoutine != null) StopCoroutine(rightAttackRoutine);
    }

    #region Attack Pattern Functions
    void StartMainAttack()
    {
        StopAllAttackRoutines();
        animator.Play("MainAttack");
        mainAttackRoutine = StartCoroutine(MainAttackRoutine());
    }

    IEnumerator MainAttackRoutine()
    {
        while (true)
        {
            if (mainAttackPrefabs.Length == 0 || mainAttackPoints.Length < 2)
                yield break;

            Transform[] sortedPoints = mainAttackPoints
                .OrderBy(point => Vector3.Distance(player.position, point.position))
                .ToArray();

            // Get the two closest points
            Transform point1 = sortedPoints[0];
            Transform point2 = sortedPoints[1];

            // 50% chance to use the closest point, otherwise pick a random one
            if (Random.value > 0.5f)
            {
                point1 = mainAttackPoints[Random.Range(0, mainAttackPoints.Length)];
            }
            if (Random.value > 0.5f)
            {
                point2 = mainAttackPoints[Random.Range(0, mainAttackPoints.Length)];
            }

            // Select random attack prefabs
            GameObject prefab1 = mainAttackPrefabs[Random.Range(0, mainAttackPrefabs.Length)];
            GameObject prefab2 = mainAttackPrefabs[Random.Range(0, mainAttackPrefabs.Length)];

            // Instantiate at selected points
            Instantiate(prefab1, point1.position, point1.rotation);
            Instantiate(prefab2, point2.position, point2.rotation);

            yield return new WaitForSeconds(mainSpawnInterval);
        }
    }


    Transform GetNearestAttackPoint()
    {
        Transform nearest = mainAttackPoints[0];
        float minDistance = Vector3.Distance(player.position, nearest.position);

        foreach (Transform point in mainAttackPoints)
        {
            float distance = Vector3.Distance(player.position, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = point;
            }
        }

        return nearest;
    }

    void StartLeftAttack()
    {
        animator.Play("LeftAttack");
        leftAttackRoutine = StartCoroutine(SideAttackRoutine(leftAttackPrefabs, leftAttackPoints, leftSpawnDelay));
    }

    void StartRightAttack()
    {
        animator.Play("RightAttack");
        rightAttackRoutine = StartCoroutine(SideAttackRoutine(rightAttackPrefabs, rightAttackPoints, rightSpawnDelay));
    }

    IEnumerator SideAttackRoutine(GameObject[] prefabs, Transform[] points, float delay)
    {
        if (prefabs.Length == 0 || points.Length == 0) 
            yield break;

        while (true)
        {
            for (int i = 0; i < points.Length; i++)
            {
                GameObject prefab = prefabs[i % prefabs.Length];
                Instantiate(prefab, points[i].position, points[i].rotation);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    void StartBigAttack()
    {
        StopAllAttackRoutines();
        animator.Play("BigAttack");
        StartCoroutine(BigAttackRoutine());
    }

    IEnumerator BigAttackRoutine()
    {
        Instantiate(bigAttackPrefab, bigAttackPoint.position, bigAttackPoint.rotation);
        bigAttackPlatforms.SetActive(true);
        
        yield return new WaitForSeconds(5f); // Reduced platform active time

        bigAttackPlatforms.SetActive(false);

        yield return new WaitForSeconds(attackCooldown); // Wait for attack cooldown before the next attack

        StartAttackSequence(); // Start the next attack
    }
    #endregion

    public void TakeDamage(float amount)
    {
        // Check if the turret holder has no children before allowing damage
        if (turretHolder.transform.childCount == 0)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            bossHealthBar?.SetHealth(currentHealth);

            if (currentHealth <= 0f) Die();
        }
        else
        {
            Debug.Log("Cannot take damage because turret holder has children.");
        }
    }


    void Die()
    {
        animator.Play("Die");
        StopAllCoroutines();
        Destroy(gameObject, 3f);
    }

    void PlayIdleAnimation()
    {
        animator.Play("Idle");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    // 🔹 Randomly Spawn Items
    IEnumerator ItemSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            if (healthBoxPrefabs.Length == 0 || ammoBoxPrefabs.Length == 0 || itemSpawnPoints.Length == 0)
                yield break;

            // Pick a random item type (health or ammo)
            GameObject[] selectedArray = (Random.value > 0.5f) ? healthBoxPrefabs : ammoBoxPrefabs;
            GameObject itemToSpawn = selectedArray[Random.Range(0, selectedArray.Length)];

            // Pick a random spawn point
            Transform spawnPoint = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)];

            // Spawn the item
            Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
