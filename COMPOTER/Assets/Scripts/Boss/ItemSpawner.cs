using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabsToSpawn;   // Prefabs to choose from
    public float spawnRadius = 10f;       // Radius around this object
    public float spawnInterval = 1f;      // Time between each spawn
    public bool spawnOnStart = true;      // Start automatically

    [Header("Ground Detection")]
    public LayerMask groundLayer;         // Layers considered as ground
    public float raycastHeight = 20f;     // How high above to raycast from
    public float heightOffset = 0.3f;     // Height above the ground
    public bool alignToGroundNormal = true; // Align with slope of ground

    private Coroutine spawnCoroutine;

    private void Start()
    {
        if (spawnOnStart)
            StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null && prefabsToSpawn.Length > 0)
            spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOne()
    {
        if (prefabsToSpawn.Length == 0) return;

        // Choose random prefab
        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
        if (prefab == null) return;

        // Random position within radius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 rayOrigin = transform.position + new Vector3(randomCircle.x, raycastHeight, randomCircle.y);

        // Raycast down to find ground
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundLayer))
        {
            Vector3 spawnPos = hit.point + Vector3.up * heightOffset;

            Quaternion spawnRot = Quaternion.identity;
            if (alignToGroundNormal)
                spawnRot = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Instantiate(prefab, spawnPos, spawnRot);
        }
        else
        {
            Debug.LogWarning("EndlessPrefabSpawner: No ground detected under spawn point.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
