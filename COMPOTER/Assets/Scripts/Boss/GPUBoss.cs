using System.Collections;
using UnityEngine;

public class GPUBoss : MonoBehaviour
{
    [Header("Core Settings")]
    public float detectionRange = 25f;
    public LayerMask playerLayer;
    public float maxHealth = 6000f;
    public BossHealthBar bossHealthBar;
    public GameObject turretHolder;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
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

            int attackIndex = Random.Range(0, 4);
            
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

            float timer = 0f;
            while (timer < attackDuration)
            {
                if (Vector3.Distance(transform.position, player.position) > detectionRange)
                {
                    StopAllAttackRoutines();
                    isAttacking = false;
                    yield break;
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
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
        mainAttackRoutine = StartCoroutine(MainAttackRoutine());
    }

    IEnumerator MainAttackRoutine()
    {
        while (true)
        {
            if (mainAttackPrefabs.Length == 0 || mainAttackPoints.Length < 2) 
                yield break;

            GameObject prefab1 = mainAttackPrefabs[Random.Range(0, mainAttackPrefabs.Length)];
            GameObject prefab2 = mainAttackPrefabs[Random.Range(0, mainAttackPrefabs.Length)];

            Transform point1 = mainAttackPoints[Random.Range(0, mainAttackPoints.Length)];
            Transform point2;
            do {
                point2 = mainAttackPoints[Random.Range(0, mainAttackPoints.Length)];
            } while (point2 == point1);

            Instantiate(prefab1, point1.position, point1.rotation);
            Instantiate(prefab2, point2.position, point2.rotation);

            yield return new WaitForSeconds(mainSpawnInterval);
        }
    }

    void StartLeftAttack()
    {
        leftAttackRoutine = StartCoroutine(SideAttackRoutine(leftAttackPrefabs, leftAttackPoints, leftSpawnDelay));
    }

    void StartRightAttack()
    {
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
        StartCoroutine(BigAttackRoutine());
    }

    IEnumerator BigAttackRoutine()
    {
        Instantiate(bigAttackPrefab, bigAttackPoint.position, bigAttackPoint.rotation);
        bigAttackPlatforms.SetActive(true);
        
        yield return new WaitForSeconds(10f);
        
        bigAttackPlatforms.SetActive(false);
    }
    #endregion

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        bossHealthBar?.SetHealth(currentHealth);

        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}