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
    private int lastAttackIndex = -1;
    private bool hasActivated = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        PlayIdleAnimation();
    }

    void Update()
    {
        if (player == null || isAttacking || hasActivated) return;

        if (turretHolder.transform.childCount <= 0 && 
            Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            hasActivated = true;
            StartCoroutine(ActivateAndStartAttack());
        }
    }

    IEnumerator ActivateAndStartAttack()
    {
        animator.Play("Activate"); // Make sure you have an "Activate" animation in Animator
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for animation to finish

        StartAttackSequence();
        itemSpawnRoutine = StartCoroutine(ItemSpawnRoutine()); // Start item spawn loop
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
                    yield return StartCoroutine(BigAttackRoutine());
                    break;
            }

            // Wait for duration unless BigAttack already waited
            if (attackIndex != 3)
            {
                yield return new WaitForSeconds(attackDuration);
            }

            StopAllAttackRoutines();
            yield return new WaitForSeconds(attackCooldown);

            if (Vector3.Distance(transform.position, player.position) > detectionRange)
            {
                isAttacking = false;
                PlayIdleAnimation();
                hasActivated = false; // optional reset
                yield break;
            }
        }
    }

    int WeightedAttackSelection()
    {
        // Weights: Main, Left, Right, Big
        int[] weights = { 4, 4, 4, 1 }; // Big attack is less likely (1/13 total weight)
        int totalWeight = weights.Sum();

        // Create a list of possible indices, excluding the last used one
        var validIndices = new System.Collections.Generic.List<int>();

        for (int i = 0; i < weights.Length; i++)
        {
            if (i != lastAttackIndex || weights[i] == 1) // Allow rare Big Attack to occasionally repeat
            {
                for (int j = 0; j < weights[i]; j++)
                {
                    validIndices.Add(i);
                }
            }
        }

        int selected = validIndices[Random.Range(0, validIndices.Count)];
        lastAttackIndex = selected;
        return selected;
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

    IEnumerator BigAttackRoutine()
    {
        animator.Play("BigAttack");
        Instantiate(bigAttackPrefab, bigAttackPoint.position, bigAttackPoint.rotation);
        bigAttackPlatforms.SetActive(true);

        yield return new WaitForSeconds(5f); // Platform duration
        bigAttackPlatforms.SetActive(false);

        yield return new WaitForSeconds(attackCooldown); // Optional cooldown, or let main loop handle
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
