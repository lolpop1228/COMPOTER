using UnityEngine;
using System.Collections;

public class SpawnPrefabs : MonoBehaviour
{
    public GameObject[] prefabs; // Assign two prefabs in the Inspector
    public Transform[] spawnPoints; // Assign multiple spawn points
    public float spawnInterval = 10f;

    void OnEnable()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Spawn()
    {
        if (prefabs.Length > 0 && spawnPoints.Length > 0)
        {
            // Pick a random prefab
            GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
            // Pick a random spawn point
            Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the chosen prefab at the chosen spawn point
            Instantiate(prefabToSpawn, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Prefabs or Spawn Points are not assigned!");
        }
    }
}
