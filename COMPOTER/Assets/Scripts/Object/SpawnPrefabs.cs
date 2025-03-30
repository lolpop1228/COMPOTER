using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPrefabs : MonoBehaviour
{
    public GameObject[] prefabs; // Assign two prefabs in the Inspector
    public Transform[] spawnPoints; // Assign multiple spawn points
    public float spawnInterval = 10f;

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Track spawned objects

    void OnEnable()
    {
        ClearPreviousSpawns(); // Clear old spawns when the scene reloads
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
            GameObject spawned = Instantiate(prefabToSpawn, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
            spawnedObjects.Add(spawned); // Track the spawned object
        }
        else
        {
            Debug.LogWarning("Prefabs or Spawn Points are not assigned!");
        }
    }

    void ClearPreviousSpawns()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear(); // Clear the list
    }
}
