using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;

public class GPUBoss : MonoBehaviour
{
    [Header("Core Settings")]
    public float detectionRange = 25f;
    public LayerMask playerLayer;
    public float maxHealth = 6000f;
    public BossHealthBar bossHealthBar;
    public GameObject healthBar;
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
    public GameObject[] healthBoxPrefabs;
    public GameObject[] ammoBoxPrefabs;
    public Transform[] itemSpawnPoints;
    public float minSpawnInterval = 15f;
    public float maxSpawnInterval = 30f;
    private Coroutine itemSpawnRoutine;

    [Header("Attack Timing Settings")]
    public float attackCooldown = 5f;
    private int lastAttackIndex = -1;
    private bool hasActivated = false;

    [Header("Audio Settings")]
    private AudioSource audioSource;
    public AudioClip mainAttackSound;
    public AudioClip sideAttackSound;
    public AudioClip bigAttackSound;
    public AudioClip activateSound;
    public AudioClip dieSound;
    public GameObject BGM;

    [Header("Explosion Settings")]
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;
    public Transform explosionPoint;

    [Header("Ending")]
    public PlayableDirector endingDialouge;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        // ✅ Hide health bar until all turrets are dead
        if (healthBar != null)
            healthBar.SetActive(false);

        PlayIdleAnimation();
    }

    void Update()
    {
        if (player == null || isAttacking || hasActivated)
            return;

        // ✅ Activate boss when all homing turrets are destroyed
        if (turretHolder.transform.childCount <= 0 &&
            Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            hasActivated = true;
            StartCoroutine(ActivateAndStartAttack());
        }
    }

    IEnumerator ActivateAndStartAttack()
    {
        animator.Play("Activate");

        // Wait for activation animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ✅ Show health bar when boss activates
        if (healthBar != null)
            healthBar.SetActive(true);

        if (audioSource != null && activateSound != null)
            audioSource.PlayOneShot(activateSound);

        StartAttackSequence();
        itemSpawnRoutine = StartCoroutine(ItemSpawnRoutine());
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
                case 0: StartMainAttack(); break;
                case 1: StartLeftAttack(); break;
                case 2: StartRightAttack(); break;
                case 3: yield return StartCoroutine(BigAttackRoutine()); break;
            }

            if (attackIndex != 3)
                yield return new WaitForSeconds(attackDuration);

            StopAllAttackRoutines();
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    int WeightedAttackSelection()
    {
        int[] weights = { 6, 3, 3, 1 };
        int totalWeight = weights.Sum();

        var validIndices = new System.Collections.Generic.List<int>();

        for (int i = 0; i < weights.Length; i++)
        {
            if (i != lastAttackIndex || weights[i] == 1)
            {
                for (int j = 0; j < weights[i]; j++)
                    validIndices.Add(i);
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

    #region Attack Patterns
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
                .OrderBy(p => Vector3.Distance(player.position, p.position))
                .ToArray();

            Transform p1 = sortedPoints[0];
            Transform p2 = sortedPoints[1];

            if (Random.value > 0.5f)
                p1 = mainAttackPoints[Random.Range(0, mainAttackPoints.Length)];
            if (Random.value > 0.5f)
                p2 = mainAttackPoints[Random.Range(0, mainAttackPoints.Length)];

            GameObject prefab1 = mainAttackPrefabs[Random.Range(0, mainAttackPrefabs.Length)];
            GameObject prefab2 = mainAttackPrefabs[Random.Range(0, mainAttackPrefabs.Length)];

            Instantiate(prefab1, p1.position, p1.rotation);
            Instantiate(prefab2, p2.position, p2.rotation);

            if (audioSource != null && mainAttackSound != null)
                audioSource.PlayOneShot(mainAttackSound);

            yield return new WaitForSeconds(mainSpawnInterval);
        }
    }

    void StartLeftAttack()
    {
        animator.Play("LeftAttack");
        if (audioSource != null && sideAttackSound != null)
            audioSource.PlayOneShot(sideAttackSound);
        leftAttackRoutine = StartCoroutine(SideAttackRoutine(leftAttackPrefabs, leftAttackPoints, leftSpawnDelay));
    }

    void StartRightAttack()
    {
        animator.Play("RightAttack");
        if (audioSource != null && sideAttackSound != null)
            audioSource.PlayOneShot(sideAttackSound);
        rightAttackRoutine = StartCoroutine(SideAttackRoutine(rightAttackPrefabs, rightAttackPoints, rightSpawnDelay));
    }

    IEnumerator SideAttackRoutine(GameObject[] prefabs, Transform[] points, float delay)
    {
        if (prefabs.Length == 0 || points.Length == 0)
            yield break;

        while (true)
        {
            foreach (var point in points)
            {
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                Instantiate(prefab, point.position, point.rotation);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator BigAttackRoutine()
    {
        animator.Play("BigAttack");
        if (audioSource != null && bigAttackSound != null)
            audioSource.PlayOneShot(bigAttackSound);

        Instantiate(bigAttackPrefab, bigAttackPoint.position, bigAttackPoint.rotation);
        bigAttackPlatforms.SetActive(true);

        yield return new WaitForSeconds(5f);
        bigAttackPlatforms.SetActive(false);
    }
    #endregion

    public void TakeDamage(float amount)
    {
        // ✅ Only take damage if turrets are gone
        if (turretHolder.transform.childCount <= 0)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            bossHealthBar?.SetHealth(currentHealth);

            if (currentHealth <= 0f)
                Die();
        }
        else
        {
            Debug.Log("Boss is invulnerable while turrets are alive.");
        }
    }

    void Die()
    {
        animator.Play("Die");
        if (BGM != null) BGM.SetActive(false);
        StopAllCoroutines();

        if (audioSource != null && dieSound != null)
            audioSource.PlayOneShot(dieSound);

        Destroy(gameObject, 9f);
    }

    void EndingScene()
    {
        if (endingDialouge != null)
            endingDialouge.Play();
    }

    void OnDestroy()
    {
        // Explosion effects and sound
        Vector3 pos = explosionPoint != null ? explosionPoint.position : transform.position;

        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, pos, Quaternion.identity);

        if (explosionSound != null)
        {
            GameObject soundObj = new GameObject("ExplosionSound");
            AudioSource temp = soundObj.AddComponent<AudioSource>();
            temp.clip = explosionSound;
            temp.Play();
            Destroy(soundObj, explosionSound.length);
        }

        EndingScene();
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

    IEnumerator ItemSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            if (healthBoxPrefabs.Length == 0 || ammoBoxPrefabs.Length == 0 || itemSpawnPoints.Length == 0)
                yield break;

            GameObject[] boxArray = (Random.value > 0.5f) ? healthBoxPrefabs : ammoBoxPrefabs;
            GameObject item = boxArray[Random.Range(0, boxArray.Length)];
            Transform spawnPoint = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)];

            Instantiate(item, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
