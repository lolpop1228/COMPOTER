using System.Collections;
using UnityEngine;
using System.Linq;

public class FinalBoss : MonoBehaviour
{
    [Header("Core Settings")]
    public float detectionRange = 25f;
    public LayerMask playerLayer;
    public float maxHealth = 6000f;
    public BossHealthBar bossHealthBar;
    private Animator animator;

    [Header("Attack Settings")]
    public float attackCooldown = 3f; // Cooldown between attacks
    public float attackDuration = 2f; // Duration of the attack animation
    private float currentHealth;
    private Transform player;
    private Coroutine attackRoutine;
    private int lastAttackIndex = -1;
    private bool isAttacking = false;

    [Header("Left Attack Settings")]
    public GameObject[] leftAttackPrefabs;
    public Transform[] leftAttackPoints;
    public float leftAttackDelay = 0.3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        PlayIdleAnimation();
        StartAttackCycle();
    }

    void Update()
    {
        if (player == null || isAttacking) return;
    }

    void StartAttackCycle()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        isAttacking = true;

        while (true)
        {
            int attackIndex = GetRandomAttackIndex();
            string animName = GetAnimationNameFromIndex(attackIndex);

            // Play the animation for the selected attack
            animator.Play(animName);

            // Wait for the attack animation to complete (half duration before and after attack)
            yield return new WaitForSeconds(attackDuration / 2f);

            // Call the corresponding attack logic after the animation starts
            switch (attackIndex)
            {
                case 0: SlamAttack(); break;
                case 1: SwipeAttack(); break;
                case 2: SwipeAttack2(); break;
                case 3: SwipeAttack3(); break;
                case 4: LeftAttack(); break;
            }

            // Wait until the attack animation finishes before considering the cooldown
            yield return new WaitForSeconds(attackDuration / 2f);

            // Now apply the cooldown (wait before the next attack)
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    int GetRandomAttackIndex()
    {
        int[] weights = { 4, 4, 4, 1, 3 };

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

    string GetAnimationNameFromIndex(int index)
    {
        switch (index)
        {
            case 0: return "Slam";
            case 1: return "SwipeAttack";
            case 2: return "SwipeAttack2";
            case 3: return "SwipeAttack3";
            case 4: return "LeftAttack";
            default: return "Slam";
        }
    }

    // ------------------------------
    // Attack Logic Methods (placeholders for now)
    // ------------------------------
    void SlamAttack()
    {
        Debug.Log("Slam Attack Triggered");
        // TODO: Add slam logic (damage, effects, etc.)
    }

    void SwipeAttack()
    {
        Debug.Log("Swipe Attack Triggered");
        // TODO: Add swipe logic
    }

    void SwipeAttack2()
    {
        Debug.Log("Swipe Attack 2 Triggered");
        // TODO: Add swipe2 logic
    }

    void SwipeAttack3()
    {
        Debug.Log("Swipe Attack 3 Triggered");
        // TODO: Add swipe3 logic
    }

    void LeftAttack()
    {
        StartCoroutine(LeftAttackRoutine());
    }

    IEnumerator LeftAttackRoutine()
    {
        if (leftAttackPrefabs.Length == 0 || leftAttackPoints.Length == 0)
            yield break;

        if (isAttacking) // Check if an attack is already in progress
            yield break; // Exit if the previous attack hasn't finished yet

        isAttacking = true; // Set the flag to indicate an attack is in progress

        float attackTime = attackDuration; // Total time to run the side attack
        float timer = 0f;

        int prefabIndex = 0; // Start with the first prefab in the array

        while (timer < attackTime)
        {
            // Spawn the current prefab at the specified point
            GameObject prefab = leftAttackPrefabs[prefabIndex % leftAttackPrefabs.Length];
            Instantiate(prefab, leftAttackPoints[prefabIndex % leftAttackPoints.Length].position, 
                        leftAttackPoints[prefabIndex % leftAttackPoints.Length].rotation);

            // Move to the next prefab in the array
            prefabIndex++;

            // Wait for the delay before spawning the next prefab
            yield return new WaitForSeconds(leftAttackDelay);

            // Increment the timer
            timer += leftAttackDelay;
        }

        isAttacking = false; // Reset the flag after the attack finishes
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        bossHealthBar?.SetHealth(currentHealth);

        if (currentHealth <= 0f) Die();
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
}
